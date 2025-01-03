using System.Reflection;
using TelegramBotTemplate.Commands;
using TelegramBotTemplate.Configuration;
using TelegramBotTemplate.Services;
using TelegramBotTemplate.Utility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Types;

var builder = Host.CreateApplicationBuilder(args);

// Add user secrets
builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly(), true);

// Parse settings
builder.Services.AddSettings<TelegramSettings>(builder.Configuration.GetSection("Telegram"));

// Add Telegram update middleware pipeline
builder.AddMiddlewarePipeline();

// Add Telegram bot services to the DI container
builder.Services.AddHostedService<TelegramBotService>();
builder.Services.AddSingleton<CommandParserService>();

// Register commands
builder.Services.AddCommand<StartCommand>();
builder.Services.AddCommand<HelpCommand>();

// Avoid circular dependency in /help
builder.Services.AddTransient<Func<IEnumerable<IChatCommand>>>(sp =>
    sp.GetServices<IChatCommand>);

// Build the application
var app = builder.Build();

// Add middlewares
app.UseErrorHandler(); // Send error messages to users
app.UseUpdateLogger(); // Log received Telegram bot updates
if (builder.Environment.IsDevelopment()) app.UseRequestTimer(); // Measure update processing time
app.UseTextCommands(); // Handle text commands

// Anonymous middleware example
app.Use(async (context, next) =>
{
    // Only proceed with sticker messages
    if (context.Update.Message?.Sticker is not { } sticker)
    {
        await next(context);
        return;
    }

    // Echo the sticker
    await context.Client.SendSticker(context.Update.Message.Chat.Id, InputFile.FromFileId(sticker.FileId));

    // Continue the execution
    await next(context);
});

// Start the application
app.Run();