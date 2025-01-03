using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBotTemplate.Models;

/// <summary>
///     Context of the Telegram bot update
/// </summary>
/// <param name="update">The received update</param>
/// <param name="client">The Telegram bot instance that received the update</param>
/// <param name="services">Registered application services</param>
/// <param name="ct">Cancellation token</param>
public class UpdateContext(Update update, ITelegramBotClient client, IServiceProvider services, CancellationToken ct)
{
    /// <summary>
    ///     The received update
    /// </summary>
    public Update Update { get; set; } = update;

    /// <summary>
    ///     The Telegram bot instance that received the update
    /// </summary>
    public ITelegramBotClient Client { get; set; } = client;

    /// <summary>
    ///     Registered application service provider
    /// </summary>
    public IServiceProvider Services { get; set; } = services;

    /// <summary>
    ///     Cancellation token
    /// </summary>
    public CancellationToken CancellationToken { get; set; } = ct;
}