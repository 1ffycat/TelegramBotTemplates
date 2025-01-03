using Telegram.Bot.Types.Enums;

namespace TelegramBotTemplate.Utility;

/// <summary>
/// Class for formatting strings in messages
/// </summary>
public static class Formatter
{
    /// <summary>
    /// Format text in italics
    /// </summary>
    /// <param name="text">Text</param>
    /// <param name="parseMode">Formatting mode</param>
    /// <returns>String with added formatting characters for the corresponding formatting mode</returns>
    public static string Italic(string text, ParseMode parseMode = ParseMode.MarkdownV2)
    {
        return Format(text, "_", "i", parseMode);
    }

    /// <summary>
    /// Format text as bold
    /// </summary>
    /// <param name="text">Text</param>
    /// <param name="parseMode">Formatting mode</param>
    /// <returns>A string with formatting characters added for the corresponding formatting mode</returns>
    public static string Bold(string text, ParseMode parseMode = ParseMode.MarkdownV2)
    {
        return Format(text, "*", "b", parseMode);
    }

    /// <summary>
    /// Format text as underlined
    /// </summary>
    /// <param name="text">Text</param>
    /// <param name="parseMode">Formatting mode</param>
    /// <returns>A string with formatting characters added for the corresponding formatting mode</returns>
    public static string Underline(string text, ParseMode parseMode = ParseMode.MarkdownV2)
    {
        return Format(text, "__", "u", parseMode);
    }

    /// <summary>
    /// Format text as strikethrough
    /// </summary>
    /// <param name="text">Text</param>
    /// <param name="parseMode">Formatting mode</param>
    /// <returns>String with added formatting characters for the corresponding formatting mode</returns>
    public static string Strikethrough(string text, ParseMode parseMode = ParseMode.MarkdownV2)
    {
        return Format(text, "~", "s", parseMode);
    }

    /// <summary>
    /// Format text as a spoiler
    /// </summary>
    /// <param name="text">Text</param>
    /// <param name="parseMode">Formatting mode</param>
    /// <returns>String with added formatting characters for the corresponding formatting mode</returns>
    public static string Spoiler(string text, ParseMode parseMode = ParseMode.MarkdownV2)
    {
        return Format(text, "||", """<span class="tg-spoiler">""", "</span>", parseMode);
    }

    /// <summary>
    /// Format text in monospace
    /// </summary>
    /// <remarks>Allows copying by clicking or tapping</remarks>
    /// <remarks>Same as `text`</remarks>
    /// <param name="text">Text</param>
    /// <param name="parseMode">Formatting mode</param>
    /// <returns>A string with added formatting characters for the corresponding formatting mode</returns>
    public static string Monospace(string text, ParseMode parseMode = ParseMode.MarkdownV2)
    {
        return Format(text, "`", "pre", parseMode);
    }

    /// <summary>
    /// Format text as a single line of code
    /// </summary>
    /// <remarks>The result is the same as Monospace, but the HTML uses the code tag</remarks>
    /// <param name="text">Text</param>
    /// <param name="parseMode">Formatting mode</param>
    /// <returns>A string with formatting characters added for the corresponding formatting mode</returns>
    public static string Code(string text, ParseMode parseMode = ParseMode.MarkdownV2)
    {
        return Format(text, "`", "code", parseMode);
    }

    /// <summary>
    /// Format the code as multiline code
    /// </summary>
    /// <param name="text">Text</param>
    /// <param name="language">Code language</param>
    /// <param name="parseMode">Format mode</param>
    /// <returns>A string with formatting characters added for the corresponding formatting mode</returns>
    public static string MultilineCode(string text, string language = "", ParseMode parseMode = ParseMode.MarkdownV2)
    {
        return Format(text, $"```{language}\n", "\n```", $"""<pre><code class="{language}">""", "</code></pre>",
        parseMode);
    }

    /// <summary>
    /// Format text as a quote
    /// </summary>
    /// <param name="text">Text</param>
    /// <param name="parseMode">Formatting mode</param>
    /// <returns>A string with formatting characters added for the corresponding formatting mode</returns>
    public static string Quote(string text, ParseMode parseMode = ParseMode.MarkdownV2)
    {
        return Format(parseMode is ParseMode.MarkdownV2 ? '>' + text.Replace("\n", "\n>") : text,
        "", "blockquote", parseMode);
    }

    /// <summary>
    /// Format the text as an expandable quote
    /// </summary>
    /// <param name="text">Text</param>
    /// <param name="parseMode">Formatting mode</param>
    /// <returns>The string with the appended
    /// formatting characters for the corresponding formatting mode</returns>
    public static string ExpandableQuote(string text, ParseMode parseMode = ParseMode.MarkdownV2)
    {
        return Format(parseMode is ParseMode.MarkdownV2 ? '>' + text.Replace("\n", "\n>") : text, "**", "||",
        "<blockquote expandable>", "</blockquote>", parseMode);
    }

    /// <summary>
    /// Format code depending on the formatting mode
    /// </summary>
    /// <param name="text">Text</param>
    /// <param name="markdownSymbol">Symbol to wrap text in MarkdownV2</param>
    /// <param name="htmlOpeningTag">Opening tag in HTML</param>
    /// <param name="htmlClosingTag">Closing tag in HTML</param>
    /// <param name="parseMode">Formatting mode</param>
    /// <remarks>Unlike overloading with htmlTagName, here you need to add triangular brackets</remarks>
    /// <returns>A string with added formatting symbols for the corresponding formatting mode</returns>
    private static string Format(string text, string markdownSymbol, string htmlOpeningTag, string htmlClosingTag,
     ParseMode parseMode)
    {
        return Format(text, markdownSymbol, markdownSymbol, htmlOpeningTag, htmlClosingTag, parseMode);
    }

    /// <summary>
    /// Format code depending on the formatting mode
    /// </summary>
    /// <param name="text">Text</param>
    /// <param name="markdownOpeningSymbol">Opening symbol in MarkdownV2</param>
    /// <param name="markdownClosingSymbol">Closing symbol in MarkdownV2</param>
    /// <param name="htmlOpeningTag">Opening tag in HTML</param>
    /// <param name="htmlClosingTag">Closing tag in HTML</param>
    /// <param name="parseMode">Formatting mode</param>
    /// <remarks>Unlike overloading with htmlTagName, here you need to add triangular brackets</remarks>
    /// <returns>A string with added formatting symbols for the corresponding mode formatting</returns>
    private static string Format(string text, string markdownOpeningSymbol, string markdownClosingSymbol,
    string htmlOpeningTag, string htmlClosingTag, ParseMode parseMode)
    {
        return parseMode switch
        {
            ParseMode.MarkdownV2 => $"{markdownOpeningSymbol}{text}{markdownClosingSymbol}",
            ParseMode.Html => $"{htmlOpeningTag}{text}{htmlClosingTag}",
            ParseMode.Markdown => throw new NotSupportedException(
            "Markdown formatting is obsolete, please use MarkdownV2."),
            _ => text
        };
    }

    /// <summary>
    /// Format code depending on formatting mode
    /// </summary>
    /// <param name="text">Text</param>
    /// <param name="markdownSymbol">Symbol to wrap in MarkdownV2</param>
    /// <param name="htmlTagName">HTML tag name without angle brackets</param>
    /// <param name="parseMode">Formatting mode</param>
    /// <remarks>Unlike overloading with opening/closing HTML tags, you do NOT need to add angle brackets here</remarks>
    /// <returns>String with added formatting symbols for the corresponding formatting mode</returns>
    private static string Format(string text, string markdownSymbol, string htmlTagName,
    ParseMode parseMode)
    {
        return Format(text, markdownSymbol, $"<{htmlTagName}>", $"</{htmlTagName}>", parseMode);
    }
}