﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Bomberjam.Common;
using Bomberjam.Website.Common;
using Bomberjam.Website.Database;
using Bomberjam.Website.Models;
using Bomberjam.Website.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Bomberjam.Website.Controllers
{
    [Authorize(AuthenticationSchemes = Constants.SupportedAuthenticationSchemes)]
    [ApiController]
    [Route("~/api")]
    public class ApiController : BaseBomberjamController<ApiController>
    {
        private static readonly SemaphoreSlim GetNextTaskMutex = new(1);

        public ApiController(IBomberjamRepository repository, IBomberjamStorage storage, ILogger<ApiController> logger)
            : base(repository, storage, logger)
        {
        }

        [HttpGet("bot/{botId}/download")]
        public async Task<IActionResult> DownloadBot(Guid botId, [FromQuery(Name = "compiled")] bool isCompiled)
        {
            var fileStream = isCompiled ? await this.Storage.DownloadCompiledBot(botId) : await this.Storage.DownloadBotSourceCode(botId);
            var fileBytes = await StreamToByteArray(fileStream);
            return this.File(fileBytes, "application/octet-stream", $"bot-{botId:D}.zip");
        }

        private static async Task<byte[]> StreamToByteArray(Stream stream)
        {
            await using (stream)
            {
                await using var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }

        [HttpPost("bot/{botId}/upload")]
        public async Task<IActionResult> UploadCompiledBot(Guid botId)
        {
            await this.Storage.UploadCompiledBot(botId, this.Request.Body);
            return this.Ok();
        }

        [HttpPost("bot/{botId}/compilation-result")]
        public async Task<IActionResult> SetCompilationResult([FromBody] BotCompilationResult compilationResult)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(GetAllErrors(this.ModelState));

            var bot = await this.Repository.GetBot(compilationResult.BotId);

            bot.Status = compilationResult.DidCompile ? CompilationStatus.CompilationSucceeded : CompilationStatus.CompilationFailed;
            bot.Language = compilationResult.Language ?? string.Empty;
            bot.Errors = compilationResult.Errors ?? string.Empty;

            await this.Repository.UpdateBot(bot);

            return this.Ok();
        }

        [HttpGet("user/{userId}/bot")]
        public async Task<IActionResult> GetUserCompiledBotId(Guid userId)
        {
            var bots = await this.Repository.GetBots(userId);
            var compiledBots = bots.Where(b => b.Status == CompilationStatus.CompilationSucceeded);
            return compiledBots.OrderByDescending(b => b.Updated).FirstOrDefault() is { } latestCompiledBot
                ? this.Ok(latestCompiledBot.Id.ToString("D"))
                : this.NotFound();
        }

        [HttpGet("task/{taskId}")]
        public async Task<IActionResult> GetTask(Guid taskId)
        {
            try
            {
                var task = await this.Repository.GetTask(taskId);
                return this.Ok(task);
            }
            catch (EntityNotFound)
            {
                return this.NotFound();
            }
        }

        [HttpGet("task/next")]
        public async Task<IActionResult> GetNextTask()
        {
            await GetNextTaskMutex.WaitAsync();

            try
            {
                var task = await this.Repository.PopNextTask();
                return this.Ok(task);
            }
            catch (EntityNotFound)
            {
                return this.NotFound();
            }
            finally
            {
                GetNextTaskMutex.Release();
            }
        }

        [HttpGet("task/{taskId}/started")]
        public async Task<IActionResult> MarkTaskAsStarted(Guid taskId)
        {
            try
            {
                using (var transaction = await this.Repository.CreateTransaction())
                {
                    await this.Repository.MarkTaskAsStarted(taskId);
                    var task = await this.Repository.GetTask(taskId);
                    await transaction.CommitAsync();
                    return this.Ok(task);
                }
            }
            catch (EntityNotFound)
            {
                return this.NotFound();
            }
        }

        [HttpGet("task/{taskId}/finished")]
        public async Task<IActionResult> MarkTaskAsFinished(Guid taskId)
        {
            try
            {
                using (var transaction = await this.Repository.CreateTransaction())
                {
                    await this.Repository.MarkTaskAsFinished(taskId);
                    var task = await this.Repository.GetTask(taskId);
                    await transaction.CommitAsync();
                    return this.Ok(task);
                }
            }
            catch (EntityNotFound)
            {
                return this.NotFound();
            }
        }

        [AllowAnonymous]
        [HttpGet("game/{gameId}")]
        public async Task<IActionResult> GetGame(Guid gameId)
        {
            var fileStream = await this.Storage.DownloadGameResult(gameId);
            return this.File(fileStream, MediaTypeNames.Application.Json, $"game-{gameId:D}.json");
        }

        [HttpPost("game")]
        public async Task<IActionResult> AddGameResult([FromBody] ApiGameResult gameResult)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(GetAllErrors(this.ModelState));

            GameHistory gameHistory;
            try
            {
                gameHistory = JsonSerializer.Deserialize<GameHistory>(gameResult.SerializedHistory);
                if (gameHistory == null)
                    throw new Exception("JSON game result parsing returned null");
            }
            catch (Exception ex)
            {
                return this.BadRequest("Not a valid JSON game result: " + ex);
            }

            gameHistory.Summary.StandardOutput = gameResult.StandardOutput ?? string.Empty;
            gameHistory.Summary.StandardError = gameResult.StandardError ?? string.Empty;

            await this.ComputeNewUserPoints(gameHistory);
            ComputeBotResponsiveness(gameHistory);

            using (var transaction = await this.Repository.CreateTransaction())
            {
                var gameId = await this.Repository.AddGame(gameHistory.Summary);

                var jsonGameHistoryStream = SerializeGameHistoryToJsonStream(gameHistory);
                await this.Storage.UploadGameResult(gameId, jsonGameHistoryStream);
                await transaction.CommitAsync();
            }

            return this.Ok();
        }

        private async Task ComputeNewUserPoints(GameHistory gameHistory)
        {
            var users = new Dictionary<string, User>();

            foreach (var (playerId, player) in gameHistory.Summary.Players)
            {
                users[playerId] = await this.Repository.GetUserById(player.WebsiteId!.Value).ConfigureAwait(false);
            }

            var match = new EloMatch();

            var eloPlayers = gameHistory.Summary.Players.Aggregate(new Dictionary<string, IEloPlayer>(), (acc, kvp) =>
            {
                var user = users[kvp.Key];
                acc[kvp.Key] = match.AddPlayer(kvp.Value.Rank, user.Points);
                return acc;
            });

            match.ComputeElos();

            foreach (var (playerId, player) in gameHistory.Summary.Players)
            {
                player.Points = eloPlayers[playerId].NewElo;
                player.DeltaPoints = eloPlayers[playerId].EloChange;
            }
        }

        private static void ComputeBotResponsiveness(GameHistory gameHistory)
        {
            var botResponsivenesses = gameHistory.Summary.Players.Keys.ToDictionary(idx => idx, _ => new List<int>());

            foreach (var playerIndex in gameHistory.Summary.Players.Keys)
            {
                foreach (var tick in gameHistory.Ticks)
                {
                    if (tick.State.Players.TryGetValue(playerIndex, out var player) && player.IsAlive)
                    {
                        botResponsivenesses[playerIndex].Add(player.IsTimedOut ? 0 : 1);
                    }
                }
            }

            foreach (var (playerIndex, botResponsiveness) in botResponsivenesses)
            {
                gameHistory.Summary.Players[playerIndex].Responsiveness = botResponsiveness.Count > 0
                    ? 1f * botResponsiveness.Sum() / botResponsiveness.Count
                    : 0f;
            }
        }

        private static MemoryStream SerializeGameHistoryToJsonStream(GameHistory gh)
        {
            var memoryStream = new MemoryStream();

            using (var jsonWriter = new Utf8JsonWriter(memoryStream, Bomberjam.Common.Constants.DefaultJsonWriterOptions))
            {
                JsonSerializer.Serialize(jsonWriter, gh);
            }

            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }

        //TODO GetCompiledBotIdForUser
    }
}