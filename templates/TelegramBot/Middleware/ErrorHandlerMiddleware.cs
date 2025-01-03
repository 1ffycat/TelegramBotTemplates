using System.Text;
using TelegramBotTemplate.Models;
using TelegramBotTemplate.Resources;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TelegramBotTemplate.Middleware;

/// <summary>
///     Middleware pipeline that sends error messages to the user who executed the command
/// </summary>
public class ErrorHandlerMiddleware : IBotMiddleware
{
    /// <summary>
    ///     Execute the middleware
    /// </summary>
    public async Task InvokeAsync(UpdateContext context, BotMiddlewareDelegate next)
    {
        var config = context.Services.GetRequiredService<IConfiguration>();

        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            if (context.Update.Message is null) return;

            var sb = new StringBuilder();

            // Message header
            sb.Append(SystemStrings.ErrorHandling_UnexpectedError);

            // Error details
            sb.Append(config.GetValue<bool>("ErrorHandling:SendTraces")
                    ? ex.ToString()
                    : $"{ex.GetType().Name}: {ex.Message}"
            );

            // Send error message to the original chat
            await context.Client.SendMessage(context.Update.Message.Chat.Id, sb.ToString(),
                cancellationToken: context.CancellationToken,
                replyParameters: new ReplyParameters()
                { ChatId = context.Update.Message.Chat.Id, MessageId = context.Update.Message.MessageId },
                parseMode: ParseMode.Html);
        }
    }
}