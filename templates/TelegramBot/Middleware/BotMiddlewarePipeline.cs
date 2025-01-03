using TelegramBotTemplate.Models;
using Microsoft.Extensions.Logging;

namespace TelegramBotTemplate.Middleware;

/// <summary>
///     Middleware pipeline for Telegram bots
/// </summary>
/// <param name="logger">Logger for inner usage</param>
public class BotMiddlewarePipeline(ILogger<BotMiddlewarePipeline> logger)
{
    private readonly List<Func<BotMiddlewareDelegate, BotMiddlewareDelegate>> _components = [];

    /// <summary>
    ///     Add anonymous middleware (delegate-based rather than class-based)
    /// </summary>
    /// <param name="middleware">Middleware handler</param>
    public void Register(Func<UpdateContext, BotMiddlewareDelegate, Task> middleware)
    {
        _components.Add(next => { return context => middleware(context, next); });
    }

    /// <summary>
    ///     Add typed middleware (class-based)
    /// </summary>
    /// <param name="middleware">Middleware instance</param>
    public void Register(IBotMiddleware middleware)
    {
        _components.Add(next => { return context => middleware.InvokeAsync(context, next); });
    }

    /// <summary>
    ///     Build the pipeline
    /// </summary>
    /// <returns>First middleware in the pipeline</returns>
    public BotMiddlewareDelegate Build()
    {
        logger.LogDebug("Building pipeline with {middlewareCount} components: {@components}", _components.Count,
            _components);

        BotMiddlewareDelegate pipeline = _ => Task.CompletedTask;

        // Iterate over all registered middlewares in reverse and add them to the pipeline
        for (var i = _components.Count - 1; i >= 0; i--) pipeline = _components[i](pipeline);

        logger.LogInformation("Pipeline built with {middlewareCount} components. Starting with {@firstMiddleware}",
            _components.Count, pipeline);

        return pipeline;
    }
}