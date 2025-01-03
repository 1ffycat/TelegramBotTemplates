using TelegramBotTemplate.Middleware;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TelegramBotTemplate.Utility;

/// <summary>
///     Application builder extensions
/// </summary>
public static class HostApplicationBuilderExtensions
{
    /// <summary>
    /// Add middleware pipeline to the application
    /// </summary>
    /// <param name="builder">The initial application builder</param>
    /// <returns>The builder with middleware pipeline service registered</returns>
    public static IHostApplicationBuilder AddMiddlewarePipeline(this IHostApplicationBuilder builder)
    {
        builder.Services.AddSingleton<BotMiddlewarePipeline>();

        return builder;
    }
}