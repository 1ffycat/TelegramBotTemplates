using TelegramBotTemplate.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TelegramBotTemplate.Utility;

public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Register a chatbot command
    /// </summary>
    /// <typeparam name="T">Command type</typeparam>
    /// <param name="services">Initial service collection</param>
    /// <returns>Service collection with the command registered</returns>
    public static IServiceCollection AddCommand<T>(this IServiceCollection services) where T : class, IChatCommand
    {
        services.AddTransient<IChatCommand, T>();

        return services;
    }

    /// <summary>
    ///     Add application settings
    /// </summary>
    /// <typeparam name="T">Settings model type</typeparam>
    /// <param name="services">Initial service collection</param>
    /// <param name="section">Configuration section to bind the settings model to</param>
    /// <returns>Service collection with the settings model bound</returns>
    public static IServiceCollection AddSettings<T>(this IServiceCollection services, IConfigurationSection section)
        where T : class
    {
        services.Configure<T>(section);

        return services;
    }
}