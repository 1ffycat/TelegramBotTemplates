using TelegramBotTemplate.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TelegramBotTemplate.Middleware;

/// <summary>
///     Middleware that logs all incoming Telegram bot updates
/// </summary>
public class UpdateLoggerMiddleware : IBotMiddleware
{
    /// <summary>
    ///     Log the update and continue to the next middleware
    /// </summary>
    /// <param name="context">Update context</param>
    /// <param name="next">Next middleware in the pipeline</param>
    public async Task InvokeAsync(UpdateContext context, BotMiddlewareDelegate next)
    {
        var logger = context.Services.GetRequiredService<ILogger<UpdateLoggerMiddleware>>();

        logger.LogInformation("Received update: {@update}", context.Update);

        try
        {
            // Proceed execution
            await next(context);
        }
        catch (Exception ex)
        {
            // Log error
            logger.LogError(ex, "Error while processing update {@update}", context.Update);
            throw;
        }
    }
}