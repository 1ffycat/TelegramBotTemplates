using TelegramBotTemplate.Commands;
using TelegramBotTemplate.Models;
using TelegramBotTemplate.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TelegramBotTemplate.Middleware;

/// <summary>
///     Middleware that matches and executes text commands
/// </summary>
public class TextCommandExecuterMiddleware : IBotMiddleware
{
    /// <summary>
    ///     Find and execute all text commands
    /// </summary>
    public async Task InvokeAsync(UpdateContext context, BotMiddlewareDelegate next)
    {
        if (context.Update.Message?.Text is null)
        {
            await next(context);
            return;
        }

        await using var scope = context.Services.CreateAsyncScope();

        var logger = scope.ServiceProvider.GetRequiredService<ILogger<TextCommandExecuterMiddleware>>();

        var commands = scope.ServiceProvider.GetServices<IChatCommand>().ToList();

        logger.LogDebug("Found {count} commands.", commands.Count);

        var cmdParser = scope.ServiceProvider.GetRequiredService<CommandParserService>();
        var ctx = cmdParser.BuildContext(context.Update.Message!);

        var matchingCommands = commands.Where(command => command.CanExecute(ctx)).ToList();

        logger.LogDebug("{count} commands match", matchingCommands.Count);

        foreach (var command in matchingCommands)
        {
            logger.LogDebug("Executing command: {name}", command.GetType().Name);
            await command.ExecuteAsync(context.Client, ctx, context.CancellationToken);
            logger.LogDebug("Done executing command {name}", command.GetType().Name);
        }

        await next(context);
    }
}