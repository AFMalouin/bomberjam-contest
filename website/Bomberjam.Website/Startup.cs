using System;
using System.IO;
using Bomberjam.Website.Authentication;
using Bomberjam.Website.Database;
using Bomberjam.Website.Jobs;
using Bomberjam.Website.Storage;
using Bomberjam.Website.Utils;
using Hangfire;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Bomberjam.Website
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            this.Configuration = configuration;
            this.Environment = environment;
        }

        private IConfiguration Configuration { get; }

        private IWebHostEnvironment Environment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting();
            services.AddMvc();
            services.AddScoped<PushFileStreamResultExecutor>();

            services.AddResponseCompression(opts =>
            {
                opts.EnableForHttps = true;
            });

            if (this.Environment.IsDevelopment())
            {
                services.AddControllersWithViews().AddRazorRuntimeCompilation();

                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Bomberjam API", Version = "v1" });
                });
            }
            else
            {
                services.AddControllersWithViews();
            }

            this.ConfigureAuthentication(services);

            var botStorage = this.ConfigureBotStorage();
            services.AddSingleton(botStorage);

            var dbConnStr = this.Configuration.GetConnectionString("BomberjamContext");

            this.ConfigureDatabase(services, dbConnStr);

            services.AddScoped<IBomberjamRepository, DatabaseRepository>();

            ConfigureHangfire(services, dbConnStr);

            if (this.Environment.IsDevelopment())
            {
                UploadTestBots(botStorage);
            }
        }

        private void ConfigureDatabase(IServiceCollection services, string dbConnStr) => services.AddDbContext<BomberjamContext>(options =>
        {
            var dbBuilder = options.UseSqlServer(dbConnStr);

            if (this.Environment.IsDevelopment())
            {
                dbBuilder.EnableSensitiveDataLogging();
            }
        });

        private static void ConfigureHangfire(IServiceCollection services, string dbConnStr)
        {
            services.AddHangfire(config =>
            {
                config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseSqlServerStorage(dbConnStr);
            });

            services.AddHangfireServer();
        }

        private IBomberjamStorage ConfigureBotStorage()
        {
            return this.Configuration.GetConnectionString("BomberjamStorage") is { Length: > 0 } storageConnStr
                ? (IBomberjamStorage)new AzureStorageBomberjamStorage(storageConnStr)
                : new LocalFileBomberjamStorage(Path.GetTempPath());
        }

        private void ConfigureAuthentication(IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/signin";
                    options.LogoutPath = "/signout";
                })
                .AddGitHub(options =>
                {
                    options.ClientId = this.Configuration["GitHub:ClientId"];
                    options.ClientSecret = this.Configuration["GitHub:ClientSecret"];
                    options.Scope.Add("user:email");
                    options.CallbackPath = "/signin-github-callback";
                })
                .AddSecret(this.Configuration["SecretAuth:Secret"]);
        }

        private static void UploadTestBots(IBomberjamStorage bomberjamStorage)
        {
            var zippedBotFileStream = typeof(Startup).Assembly.GetManifestResourceStream("Bomberjam.Website.MyBot.zip");
            if (zippedBotFileStream != null)
            {
                using (zippedBotFileStream)
                using (var zippedBotFileMs = new MemoryStream())
                {
                    zippedBotFileStream.CopyTo(zippedBotFileMs);
                    var zippedBotFileBytes = zippedBotFileMs.ToArray();

                    bomberjamStorage.UploadBotSourceCode(Constants.UserAskaiserId, new MemoryStream(zippedBotFileBytes)).GetAwaiter().GetResult();
                    bomberjamStorage.UploadBotSourceCode(Constants.UserFalgarId, new MemoryStream(zippedBotFileBytes)).GetAwaiter().GetResult();
                    bomberjamStorage.UploadBotSourceCode(Constants.UserXenureId, new MemoryStream(zippedBotFileBytes)).GetAwaiter().GetResult();
                    bomberjamStorage.UploadBotSourceCode(Constants.UserMintyId, new MemoryStream(zippedBotFileBytes)).GetAwaiter().GetResult();
                    bomberjamStorage.UploadBotSourceCode(Constants.UserKalmeraId, new MemoryStream(zippedBotFileBytes)).GetAwaiter().GetResult();
                    bomberjamStorage.UploadBotSourceCode(Constants.UserPandarfId, new MemoryStream(zippedBotFileBytes)).GetAwaiter().GetResult();
                    bomberjamStorage.UploadBotSourceCode(Constants.UserMireId, new MemoryStream(zippedBotFileBytes)).GetAwaiter().GetResult();
                }
            }
        }

        public void Configure(IApplicationBuilder app, IRecurringJobManager recurringJobs, ILogger<Startup> logger)
        {
            app.UseResponseCompression();

            if (this.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Bomberjam API"));
            }
            else
            {
                app.UseExceptionHandler("/Web/Error");
                app.UseHsts();
            }

            // Required to serve files with no extension in the .well-known folder
            var options = new StaticFileOptions
            {
                ServeUnknownFileTypes = true
            };

            app.UseHttpsRedirection();
            app.UseStaticFiles(options);

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapHangfireDashboard(new DashboardOptions
                {
                    Authorization = new[] { new HangfireDashboardAuthorizationFilter(this.Configuration) },
                });
            });

            var matchmakingCronExpr = this.Configuration["JobCrons:Matchmaking"];
            var orphanedTasksCronExpr = this.Configuration["JobCrons:OrphanedTasks"];

            logger.Log(LogLevel.Information, "Matchmaking cron expression: " + matchmakingCronExpr);
            logger.Log(LogLevel.Information, "Orphaned tasks cron expression: " + orphanedTasksCronExpr);

            recurringJobs.AddOrUpdate<MatchmakingJob>("matchmaking", job => job.Run(), matchmakingCronExpr);
            recurringJobs.AddOrUpdate<OrphanedTaskFixingJob>("orphanedTasks", job => job.Run(), orphanedTasksCronExpr);
        }
    }
}