using System.Reflection;
using TelegramBotTemplate.Attributes;
using TelegramBotTemplate.Commands;
using TelegramBotTemplate.Configuration;
using TelegramBotTemplate.Middleware;
using TelegramBotTemplate.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace TelegramBotTemplate.Services;

/// <summary>
///     Telegram bot service
/// </summary>
/// <param name="tgSettings">Telegram-specific settings (like bot token)</param>
/// <param name="serviceProvider">Service provider</param>
/// <param name="logger">Logger for inner usage</param>
/// <param name="pipeline">Telegram update middleware pipeline</param>
public class TelegramBotService(
    IOptions<TelegramSettings> tgSettings,
    IServiceProvider serviceProvider,
    ILogger<TelegramBotService> logger,
    BotMiddlewarePipeline pipeline) : IHostedService
{
    private readonly TelegramBotClient _botClient = new(tgSettings.Value.BotToken);

    private BotMiddlewareDelegate? _pipeline;

    /// <summary>
    ///     Start listening to updates
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _pipeline = pipeline.Build();

        await SetBotCommandsAsync(cancellationToken);

        var receiverOptions = new ReceiverOptions();

        _botClient.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            receiverOptions,
            cancellationToken
        );
        logger.LogInformation("Bot started receiving");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("The bot is shutting down");
        return Task.CompletedTask;

        // Polling mode doesn't requite finalization
    }

    /// <summary>
    ///     Announce available commands to Telegram
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    private async Task SetBotCommandsAsync(CancellationToken cancellationToken)
    {
        logger.LogDebug("Evaluating all available commands and their descriptions");

        await using var scope = serviceProvider.CreateAsyncScope();

        var commands = scope.ServiceProvider.GetServices<IChatCommand>();

        var botCommands = commands.Select(c => c.GetType().GetCustomAttribute<BotCommandAttribute>())
            .Where(c => c is not null)
            .Select(c => new BotCommand
            {
                Command = c!.Name,
                Description = $"{c.Description} {(c.Usage is null ? "" : $"({c.Usage})")}"
            }).ToArray();

        logger.LogDebug("Found {commandCount} commands", botCommands.Length);

        await _botClient.SetMyCommands(botCommands, cancellationToken: cancellationToken);

        logger.LogInformation("Advertised {commandCount} available commands to Telegram", botCommands.Length);
    }

    /// <summary>
    ///     Handle update
    /// </summary>
    /// <param name="botClient">Telegram bot client</param>
    /// <param name="update">Received update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <exception cref="ArgumentNullException">Middleware pipeline was not found in the DI container. Did you run builder.AddMiddlewarePipeline()?</exception>
    /// <exception cref="NullReferenceException">Middleware pipeline was not built. Did you run builder.AddMiddlewarePipeline()?</exception>
    private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        if (_pipeline is null) throw new NullReferenceException("Pipeline was not built.");

        await _pipeline(new UpdateContext(update, botClient, serviceProvider, cancellationToken));
    }

    /// <summary>
    ///     Handle bot errors
    /// </summary>
    /// <param name="botClient">Telegram bot client</param>
    /// <param name="exception">Thrown exception</param>
    /// <param name="cancellationToken">Cancellation token</param>
    private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception,
        CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException =>
                $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        logger.LogError("Telegram bot error: {errorMessage}", errorMessage);
        return Task.CompletedTask;
    }
}