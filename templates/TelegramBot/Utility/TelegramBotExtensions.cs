using TelegramBotTemplate.Resources;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBotTemplate.Utility;

namespace FbkiBot.Utility;

public static class TelegramBotExtensions
{
    /// <summary>
    /// Try to send a private message to the user. Otherwise, send a warning to the chat the message was received in.
    /// </summary>
    /// <param name="bot">Telegram bot client</param>
    /// <param name="user">User to send the message to</param>
    /// <param name="text">Message text</param>
    /// <param name="chat">Fallback chat</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public static async Task TrySendMessageOrNotify(this ITelegramBotClient bot, User user, string text, Chat chat,
        CancellationToken cancellationToken = default)
    {
        // Try sending message to DMs
        try
        {
            await bot.SendMessage(user.Id, text, cancellationToken: cancellationToken);
        }
        // Or send message to the fallback chat
        catch
        {
            await bot.SendMessage(chat!.Id,
                $"{text}\n\n{Formatter.Italic(SystemStrings.CannotSendPrivateMessage, ParseMode.Html)}",
                cancellationToken: cancellationToken, parseMode: ParseMode.Html);
        }
    }
}