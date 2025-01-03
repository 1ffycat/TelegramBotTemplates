using System.Reflection;
using System.Text;
using TelegramBotTemplate.Attributes;
using TelegramBotTemplate.Models;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace TelegramBotTemplate.Commands;

[BotCommand("/help", "Sends a list of all available commands")]
public class HelpCommand(ILogger<HelpCommand> logger, Func<IEnumerable<IChatCommand>> commandFactory) : IChatCommand
{
    public bool CanExecute(CommandContext context)
    {
        return context.Command?.Equals("/help", StringComparison.OrdinalIgnoreCase) ?? false;
    }

    public async Task ExecuteAsync(ITelegramBotClient botClient, CommandContext context,
        CancellationToken cancellationToken)
    {
        logger.LogDebug("Received /help command. Building help message.");
        var helpBuilder =
            new StringBuilder();

        // Iterate over all registered commands
        foreach (var command in commandFactory())
        {
            // Check if the command has a BotCommand attribute
            var attr = command.GetType().GetCustomAttribute<BotCommandAttribute>();

            // Skip otherwise
            if (attr is null) continue;

            // Add line to the output
            helpBuilder.AppendLine(
                $"{attr.Name}: {attr.Description} {(attr.Usage is not null ? $"({attr.Usage})" : "")}");
        }

        await botClient.SendMessage(context.Message.Chat.Id, helpBuilder.ToString(),
            cancellationToken: cancellationToken);
    }
}