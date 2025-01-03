namespace TelegramBotTemplate.Configuration;

/// <summary>
///     Settings specific to Telegram and Telegram bots
/// </summary>
public class TelegramSettings
{
    /// <summary>
    ///     Telegram bot token
    /// </summary>
    public required string BotToken { get; set; }
}