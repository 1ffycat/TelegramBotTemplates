namespace TelegramBotTemplate.Attributes;

/// <summary>
///     Text command for a Telegram bot
/// </summary>
/// <param name="name">The name of the command, for example /start</param>
/// <param name="description">The description of what the command does</param>
/// <param name="usage">Usage example. Optional</param>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class BotCommandAttribute(string name, string description, string? usage = null) : Attribute
{
    /// <summary>
    ///     Name of the command
    /// </summary>
    public string Name { get; } = name;

    /// <summary>
    ///     Description of what the command does
    /// </summary>
    public string Description { get; } = description;

    /// <summary>
    ///     Usage example
    /// </summary>
    public string? Usage { get; } = usage;
}