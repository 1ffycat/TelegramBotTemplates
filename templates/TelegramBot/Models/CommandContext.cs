using Telegram.Bot.Types;

namespace TelegramBotTemplate.Models;

/// <summary>
///     Text command context
/// </summary>
/// <param name="message">Received message</param>
/// <param name="command">Command (first token of the message text)</param>
/// <param name="arguments">Command arguments (other tokens of the message text)</param>
public class CommandContext(Message message, string? command, string[] arguments)
{
    /// <summary>
    ///     Message text without the command.
    /// </summary>
    /// <remarks>This field is lazily-evaluated.</remarks>
    private readonly Lazy<string?> _argument = new(arguments.Length > 0 ? string.Join(' ', arguments) : null);

    /// <summary>
    ///     Received message
    /// </summary>
    public Message Message { get; set; } = message;

    /// <summary>
    ///     The received message is a command
    /// </summary>
    public bool IsCommand => Command is not null;

    /// <summary>
    ///     Command (first token of the message text)
    /// </summary>
    /// <remarks>It may, but doesn't have to, start with '/'</remarks>
    public string? Command { get; set; } = command;

    /// <summary>
    ///     Command arguments (other tokens of the message text)
    /// </summary>
    public string[] Arguments { get; set; } = arguments;

    /// <summary>
    ///     All arguments after the command as a single string. Null if no arguments were provided
    /// </summary>
    /// <remarks>This property is lazily-evaluated.</remarks>
    public string? Argument => _argument.Value;
}