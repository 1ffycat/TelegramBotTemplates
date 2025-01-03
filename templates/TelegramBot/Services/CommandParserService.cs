using TelegramBotTemplate.Models;
using Telegram.Bot.Types;

namespace TelegramBotTemplate.Services;

/// <summary>
///     Parses received text messages to a command context
/// </summary>
public class CommandParserService
{
    /// <summary>
    ///     Parse the message
    /// </summary>
    /// <param name="message">Received message</param>
    /// <returns>Command context for the given message</returns>
    public CommandContext BuildContext(Message message)
    {
        if (string.IsNullOrEmpty(message.Text)) return new CommandContext(message, null, []);

        var tokens = message.Text?.Split(' ', StringSplitOptions.RemoveEmptyEntries) ?? [];

        var command = tokens.First();

        if (!command.StartsWith('/')) return new CommandContext(message, null, tokens);

        // Handle /command@examplebot calls.
        // TODO: Check if the bot id matches!!
        if (command.IndexOf('@') is var atId && atId != -1)
            command = command[..atId];

        return new CommandContext(message, command, tokens[1..]);
    }
}