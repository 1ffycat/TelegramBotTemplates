using System.Diagnostics;
using TelegramBotTemplate.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TelegramBotTemplate.Middleware;

/// <summary>
///     Middleware to execute update processing time
/// </summary>
public class RequestTimerMiddleware : IBotMiddleware
{
    /// <summary>
    ///     Measure and log execution time
    /// </summary>
    public async Task InvokeAsync(UpdateContext context, BotMiddlewareDelegate next)
    {
        var logger = context.Services.GetRequiredService<ILogger<Program>>();

        // Start the stopwatch
        var sw = new Stopwatch();
        sw.Start();

        // Proceed execution
        await next(context);

        // Log time taken
        sw.Stop();
        logger.LogDebug("Update processed in {time}", sw.Elapsed);
    }
}