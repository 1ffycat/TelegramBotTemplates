using TelegramBotTemplate.Models;
using Telegram.Bot;

namespace TelegramBotTemplate.Commands;

/// <summary>
///     Chatbot command
/// </summary>
public interface IChatCommand
{
    /// <summary>
    ///     Check if this command shall be executed for given update
    /// </summary>
    /// <param name="context">The context of the command</param>
    /// <returns>true if the command shall be executed, otherwise false</returns>
    bool CanExecute(CommandContext context);

    /// <summary>
    ///     Execute the command
    /// </summary>
    /// <param name="botClient">Telegram bot client</param>
    /// <param name="context">Command context</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task ExecuteAsync(ITelegramBotClient botClient, CommandContext context, CancellationToken cancellationToken);
}