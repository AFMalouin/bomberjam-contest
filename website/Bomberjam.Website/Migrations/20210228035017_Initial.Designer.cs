﻿// <auto-generated />
using System;
using Bomberjam.Website.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Bomberjam.Website.Migrations
{
    [DbContext(typeof(BomberjamContext))]
    [Migration("20210228035017_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.3");

            modelBuilder.Entity("Bomberjam.Website.Database.DbBot", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Errors")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Language")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("Created");

                    b.HasIndex("Status");

                    b.HasIndex("Updated");

                    b.HasIndex("UserId");

                    b.ToTable("App_Bots");

                    b.HasData(
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000001"),
                            Created = new DateTime(2021, 2, 28, 3, 50, 16, 681, DateTimeKind.Utc).AddTicks(3680),
                            Errors = "",
                            Language = "",
                            Status = 1,
                            Updated = new DateTime(2021, 2, 28, 3, 50, 16, 681, DateTimeKind.Utc).AddTicks(3862),
                            UserId = new Guid("00000000-0000-0000-0000-000000000001")
                        },
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000002"),
                            Created = new DateTime(2021, 2, 28, 3, 50, 16, 681, DateTimeKind.Utc).AddTicks(4639),
                            Errors = "",
                            Language = "",
                            Status = 1,
                            Updated = new DateTime(2021, 2, 28, 3, 50, 16, 681, DateTimeKind.Utc).AddTicks(4644),
                            UserId = new Guid("00000000-0000-0000-0000-000000000002")
                        },
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000003"),
                            Created = new DateTime(2021, 2, 28, 3, 50, 16, 681, DateTimeKind.Utc).AddTicks(4647),
                            Errors = "",
                            Language = "",
                            Status = 1,
                            Updated = new DateTime(2021, 2, 28, 3, 50, 16, 681, DateTimeKind.Utc).AddTicks(4648),
                            UserId = new Guid("00000000-0000-0000-0000-000000000003")
                        },
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000004"),
                            Created = new DateTime(2021, 2, 28, 3, 50, 16, 681, DateTimeKind.Utc).AddTicks(4649),
                            Errors = "",
                            Language = "",
                            Status = 1,
                            Updated = new DateTime(2021, 2, 28, 3, 50, 16, 681, DateTimeKind.Utc).AddTicks(4650),
                            UserId = new Guid("00000000-0000-0000-0000-000000000004")
                        },
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000005"),
                            Created = new DateTime(2021, 2, 28, 3, 50, 16, 681, DateTimeKind.Utc).AddTicks(4651),
                            Errors = "",
                            Language = "",
                            Status = 1,
                            Updated = new DateTime(2021, 2, 28, 3, 50, 16, 681, DateTimeKind.Utc).AddTicks(4652),
                            UserId = new Guid("00000000-0000-0000-0000-000000000005")
                        },
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000006"),
                            Created = new DateTime(2021, 2, 28, 3, 50, 16, 681, DateTimeKind.Utc).AddTicks(4653),
                            Errors = "",
                            Language = "",
                            Status = 1,
                            Updated = new DateTime(2021, 2, 28, 3, 50, 16, 681, DateTimeKind.Utc).AddTicks(4654),
                            UserId = new Guid("00000000-0000-0000-0000-000000000006")
                        },
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000007"),
                            Created = new DateTime(2021, 2, 28, 3, 50, 16, 681, DateTimeKind.Utc).AddTicks(4655),
                            Errors = "",
                            Language = "",
                            Status = 1,
                            Updated = new DateTime(2021, 2, 28, 3, 50, 16, 681, DateTimeKind.Utc).AddTicks(4656),
                            UserId = new Guid("00000000-0000-0000-0000-000000000007")
                        });
                });

            modelBuilder.Entity("Bomberjam.Website.Database.DbGame", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Errors")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("GameDuration")
                        .HasColumnType("float");

                    b.Property<double?>("InitDuration")
                        .HasColumnType("float");

                    b.Property<string>("Stderr")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Stdout")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Created");

                    b.ToTable("App_Games");
                });

            modelBuilder.Entity("Bomberjam.Website.Database.DbGameUser", b =>
                {
                    b.Property<Guid>("GameId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BotId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<float>("DeltaPoints")
                        .HasColumnType("real");

                    b.Property<string>("Errors")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Rank")
                        .HasColumnType("int");

                    b.Property<int>("Score")
                        .HasColumnType("int");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("datetime2");

                    b.HasKey("GameId", "UserId");

                    b.HasIndex("BotId");

                    b.HasIndex("UserId");

                    b.ToTable("App_GameUsers");
                });

            modelBuilder.Entity("Bomberjam.Website.Database.DbQueuedTask", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Data")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Created");

                    b.HasIndex("Status");

                    b.HasIndex("Type");

                    b.ToTable("App_Tasks");

                    b.HasData(
                        new
                        {
                            Id = new Guid("0adc087c-f2e6-428a-9381-a4fa3b2c000d"),
                            Created = new DateTime(2021, 2, 28, 3, 50, 16, 681, DateTimeKind.Utc).AddTicks(7627),
                            Data = "00000000-0000-0000-0000-000000000001",
                            Status = 0,
                            Type = 1,
                            Updated = new DateTime(2021, 2, 28, 3, 50, 16, 681, DateTimeKind.Utc).AddTicks(7802)
                        },
                        new
                        {
                            Id = new Guid("63b0e61b-9062-4b72-a094-b9a795272d9d"),
                            Created = new DateTime(2021, 2, 28, 3, 50, 16, 681, DateTimeKind.Utc).AddTicks(8484),
                            Data = "00000000-0000-0000-0000-000000000002",
                            Status = 0,
                            Type = 1,
                            Updated = new DateTime(2021, 2, 28, 3, 50, 16, 681, DateTimeKind.Utc).AddTicks(8486)
                        },
                        new
                        {
                            Id = new Guid("bd1d2b4b-5c9b-4cfb-a420-23a9c8098b65"),
                            Created = new DateTime(2021, 2, 28, 3, 50, 16, 681, DateTimeKind.Utc).AddTicks(8490),
                            Data = "00000000-0000-0000-0000-000000000003",
                            Status = 0,
                            Type = 1,
                            Updated = new DateTime(2021, 2, 28, 3, 50, 16, 681, DateTimeKind.Utc).AddTicks(8491)
                        },
                        new
                        {
                            Id = new Guid("9ba9871d-cc45-4d4a-b452-ced7e72446e7"),
                            Created = new DateTime(2021, 2, 28, 3, 50, 16, 681, DateTimeKind.Utc).AddTicks(8493),
                            Data = "00000000-0000-0000-0000-000000000004",
                            Status = 0,
                            Type = 1,
                            Updated = new DateTime(2021, 2, 28, 3, 50, 16, 681, DateTimeKind.Utc).AddTicks(8494)
                        },
                        new
                        {
                            Id = new Guid("297c7377-c487-4f77-a125-772e3224b393"),
                            Created = new DateTime(2021, 2, 28, 3, 50, 16, 681, DateTimeKind.Utc).AddTicks(8496),
                            Data = "00000000-0000-0000-0000-000000000005",
                            Status = 0,
                            Type = 1,
                            Updated = new DateTime(2021, 2, 28, 3, 50, 16, 681, DateTimeKind.Utc).AddTicks(8497)
                        },
                        new
                        {
                            Id = new Guid("2eb88f49-f37f-47cc-ad6c-bf2996900d4f"),
                            Created = new DateTime(2021, 2, 28, 3, 50, 16, 681, DateTimeKind.Utc).AddTicks(8513),
                            Data = "00000000-0000-0000-0000-000000000006",
                            Status = 0,
                            Type = 1,
                            Updated = new DateTime(2021, 2, 28, 3, 50, 16, 681, DateTimeKind.Utc).AddTicks(8513)
                        },
                        new
                        {
                            Id = new Guid("db610158-ad68-46e0-bcdc-ac7ffe5d8e47"),
                            Created = new DateTime(2021, 2, 28, 3, 50, 16, 681, DateTimeKind.Utc).AddTicks(8516),
                            Data = "00000000-0000-0000-0000-000000000007",
                            Status = 0,
                            Type = 1,
                            Updated = new DateTime(2021, 2, 28, 3, 50, 16, 681, DateTimeKind.Utc).AddTicks(8517)
                        });
                });

            modelBuilder.Entity("Bomberjam.Website.Database.DbUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("GithubId")
                        .HasColumnType("int");

                    b.Property<float>("Points")
                        .HasColumnType("real");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasFilter("[Email] IS NOT NULL");

                    b.HasIndex("GithubId")
                        .IsUnique();

                    b.HasIndex("Points");

                    b.HasIndex("UserName")
                        .IsUnique()
                        .HasFilter("[UserName] IS NOT NULL");

                    b.ToTable("App_Users");

                    b.HasData(
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000001"),
                            Created = new DateTime(2021, 2, 28, 3, 50, 16, 680, DateTimeKind.Utc).AddTicks(2826),
                            Email = "simmon.anthony@gmail.com",
                            GithubId = 14242083,
                            Points = 1500f,
                            Updated = new DateTime(2021, 2, 28, 3, 50, 16, 680, DateTimeKind.Utc).AddTicks(3093),
                            UserName = "Askaiser"
                        },
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000002"),
                            Created = new DateTime(2021, 2, 28, 3, 50, 16, 680, DateTimeKind.Utc).AddTicks(3682),
                            Email = "falgar@gmail.com",
                            GithubId = 36072624,
                            Points = 1500f,
                            Updated = new DateTime(2021, 2, 28, 3, 50, 16, 680, DateTimeKind.Utc).AddTicks(3684),
                            UserName = "Falgar"
                        },
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000003"),
                            Created = new DateTime(2021, 2, 28, 3, 50, 16, 680, DateTimeKind.Utc).AddTicks(3686),
                            Email = "xenure@gmail.com",
                            GithubId = 9208753,
                            Points = 1500f,
                            Updated = new DateTime(2021, 2, 28, 3, 50, 16, 680, DateTimeKind.Utc).AddTicks(3686),
                            UserName = "Xenure"
                        },
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000004"),
                            Created = new DateTime(2021, 2, 28, 3, 50, 16, 680, DateTimeKind.Utc).AddTicks(3688),
                            Email = "minty@gmail.com",
                            GithubId = 26142591,
                            Points = 1500f,
                            Updated = new DateTime(2021, 2, 28, 3, 50, 16, 680, DateTimeKind.Utc).AddTicks(3689),
                            UserName = "Minty"
                        },
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000005"),
                            Created = new DateTime(2021, 2, 28, 3, 50, 16, 680, DateTimeKind.Utc).AddTicks(3690),
                            Email = "kalmera@gmail.com",
                            GithubId = 5122918,
                            Points = 1500f,
                            Updated = new DateTime(2021, 2, 28, 3, 50, 16, 680, DateTimeKind.Utc).AddTicks(3691),
                            UserName = "Kalmera"
                        },
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000006"),
                            Created = new DateTime(2021, 2, 28, 3, 50, 16, 680, DateTimeKind.Utc).AddTicks(3693),
                            Email = "pandarf@gmail.com",
                            GithubId = 1035273,
                            Points = 1500f,
                            Updated = new DateTime(2021, 2, 28, 3, 50, 16, 680, DateTimeKind.Utc).AddTicks(3694),
                            UserName = "Pandarf"
                        },
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000007"),
                            Created = new DateTime(2021, 2, 28, 3, 50, 16, 680, DateTimeKind.Utc).AddTicks(3695),
                            Email = "mire@gmail.com",
                            GithubId = 5489330,
                            Points = 1500f,
                            Updated = new DateTime(2021, 2, 28, 3, 50, 16, 680, DateTimeKind.Utc).AddTicks(3696),
                            UserName = "Mire"
                        });
                });

            modelBuilder.Entity("Bomberjam.Website.Database.DbBot", b =>
                {
                    b.HasOne("Bomberjam.Website.Database.DbUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Bomberjam.Website.Database.DbGameUser", b =>
                {
                    b.HasOne("Bomberjam.Website.Database.DbBot", "Bot")
                        .WithMany()
                        .HasForeignKey("BotId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Bomberjam.Website.Database.DbGame", "Game")
                        .WithMany()
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Bomberjam.Website.Database.DbUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bot");

                    b.Navigation("Game");

                    b.Navigation("User");
                });
#pragma warning restore 612, 618
        }
    }
}