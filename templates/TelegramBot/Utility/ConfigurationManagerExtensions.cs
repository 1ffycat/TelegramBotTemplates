using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace TelegramBotTemplate.Utility;

public static class ConfigurationManagerExtensions
{
    /// <summary>
    /// Add a JSON file to the configuration with the lowest priority (any other config added earlier will have priority
    /// over it)
    /// </summary>
    /// <param name="configuration">Configuration manager</param>
    /// <param name="path">Path to file</param>
    /// <param name="optional">Is the file optional?</param>
    /// <param name="reloadOnChange">Automatically update the configuration when the file changes?</param>
    /// <remarks>
    /// The method sets the file to the lowest priority available. If you call this method on two different files, the one that was added earlier will have the highest priority
    /// </remarks>
    /// <returns></returns>
    public static IConfigurationManager AddDefaultsJsonFile(this IConfigurationManager configuration, string path,
        bool optional = false, bool reloadOnChange = false)
    {
        configuration.Sources.Insert(0,
            new JsonConfigurationSource { Path = path, Optional = optional, ReloadOnChange = reloadOnChange });

        return configuration;
    }
}