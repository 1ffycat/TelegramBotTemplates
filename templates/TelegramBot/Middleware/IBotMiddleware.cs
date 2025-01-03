using TelegramBotTemplate.Models;

namespace TelegramBotTemplate.Middleware;

/// <summary>
///     Middleware that handles Telegram bot updates
/// </summary>
public interface IBotMiddleware
{
    /// <summary>
    ///     Perform actions and, if needed, continue execution to the next middleware
    /// </summary>
    /// <param name="context">Update context</param>
    /// <param name="next">Next middleware</param>
    Task InvokeAsync(UpdateContext context, BotMiddlewareDelegate next);
}

/// <summary>
///     Delegate that represents the middleware
/// </summary>
public delegate Task BotMiddlewareDelegate(UpdateContext context);