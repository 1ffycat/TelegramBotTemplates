using TelegramBotTemplate.Attributes;
using TelegramBotTemplate.Models;
using TelegramBotTemplate.Resources;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace TelegramBotTemplate.Commands;

[BotCommand("/start", "Sends a greeting message")]
public class StartCommand(ILogger<StartCommand> logger) : IChatCommand
{
    public bool CanExecute(CommandContext context)
    {
        return context.Command?.Equals("/start", StringComparison.OrdinalIgnoreCase) ?? false;
    }

    public async Task ExecuteAsync(ITelegramBotClient botClient, CommandContext context,
        CancellationToken cancellationToken)
    {
        logger.LogDebug("Sending greeting message...");

        await botClient.SendMessage(context.Message.Chat.Id, CommandStrings.Start_Welcome,
            cancellationToken: cancellationToken);
    }
}