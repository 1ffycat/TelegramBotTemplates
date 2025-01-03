using TelegramBotTemplate.Middleware;
using TelegramBotTemplate.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TelegramBotTemplate.Utility;

public static class HostExtensions
{
    /// <summary>
    ///     Use typed middleware
    /// </summary>
    /// <param name="host">Application</param>
    /// <typeparam name="T">Middleware type</typeparam>
    /// <exception cref="NullReferenceException">
    ///     Middleware pipeline service could not be found. Please call builder.AddMiddlewarePipeline() before building the application
    /// </exception>
    public static IHost Use<T>(this IHost host) where T : IBotMiddleware, new()
    {
        var pipeline = host.Services.GetService<BotMiddlewarePipeline>();

        if (pipeline is null) throw new NullReferenceException("Middleware pipeline is not found in the DI container.");

        pipeline.Register(new T());

        return host;
    }

    /// <summary>
    ///     Use an anonymous middleware
    /// </summary>
    /// <param name="host">Application</param>
    /// <param name="middleware">Middleware handler</param>
    /// <exception cref="NullReferenceException">
    ///     Middleware pipeline service could not be found. Please call builder.AddMiddlewarePipeline() before building the application
    /// </exception>
    public static IHost Use(this IHost host, Func<UpdateContext, BotMiddlewareDelegate, Task> middleware)
    {
        var pipeline = host.Services.GetService<BotMiddlewarePipeline>();

        if (pipeline is null) throw new NullReferenceException("Middleware pipeline is not found in the DI container.");

        pipeline.Register(middleware);

        return host;
    }

    /// <summary>
    ///     Handle Telegram bot text commands
    /// </summary>
    /// <param name="host">Application</param>
    public static IHost UseTextCommands(this IHost host)
    {
        return host.Use<TextCommandExecuterMiddleware>();
    }

    /// <summary>
    ///     Log received Telegram bot updates
    /// </summary>
    /// <param name="host">Application</param>
    public static IHost UseUpdateLogger(this IHost host)
    {
        return host.Use<UpdateLoggerMiddleware>();
    }

    /// <summary>
    ///     Send error messages to the users upon uncatched exceptions
    /// </summary>
    /// <param name="host">Application</param>
    /// <returns></returns>
    public static IHost UseErrorHandler(this IHost host)
    {
        return host.Use<ErrorHandlerMiddleware>();
    }

    /// <summary>
    ///     Measure and log execution time for Telegram bot updates
    /// </summary>
    /// <param name="host">Application</param>
    public static IHost UseRequestTimer(this IHost host)
    {
        return host.Use<RequestTimerMiddleware>();
    }
}