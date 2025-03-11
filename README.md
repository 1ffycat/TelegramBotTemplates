# 1ffycat.TelegramBotTemplates

Welcome to **1ffycat.TelegramBotTemplates**, a modern, middleware-based framework for building Telegram bots in .NET. Designed to feel instantly familiar to ASP.NET developers, this framework brings a powerful, flexible, and intuitive development experience to Telegram bot creation.

## Why Use This Framework?

Building Telegram bots can be complex, but **1ffycat.TelegramBotTemplates** simplifies the process without sacrificing power. Whether you're a seasoned .NET developer or just getting started, this framework offers:

- A **streamlined development experience** inspired by ASP.NET's middleware pipeline.
- **Batteries-included functionality** with built-in middleware, dependency injection, and localization support.
- **Extensibility** to adapt to your unique use cases, from simple bots to enterprise-grade solutions.
- A **robust foundation** built on top of the trusted `Telegram.Bot` package and .NET's application host model.

With this framework, you can focus on building features, not boilerplate.

## Features

- 🚀 **ASP.NET-like Middleware Pipeline**: Process updates with a clean, modular pipeline.
- 💉 **Dependency Injection**: Seamlessly integrate services using .NET's built-in DI system.
- 🌐 **Internationalization (i18n)**: Manage resources for multi-language support out of the box.
- ⚙️ **Configuration Management**: Leverage `IConfiguration` for flexible, ASP.NET-style configuration.
- 🎯 **Automatic Command Parsing**: Parse commands and create context effortlessly.
- 🛡️ **Built-in Middleware** for common needs, including:
  - ⚠️ Exception handling
  - ⏱️ Performance monitoring
  - 📖 Update logging
  - ✂️ Command matching and execution
- 🔐 **User Secrets Management**: Securely handle sensitive data like bot tokens.
- ⚡ **Extensible Utilities**: Includes extensions for text formatting, requesting DM permissions, and more.
- 🏗️ **Modern Architecture**: Built on .NET's application host model for reliability and scalability.
- 📦 **Trusted Foundation**: Powered by the `Telegram.Bot` package.

## Installation

Get started by installing the template package:

```bash
dotnet new install 1ffycat.TelegramBotTemplates
```

## Quick Start

Create a new Telegram bot project in seconds:

```bash
dotnet new telegram-bot --name MyTelegramBot --token "<YOUR TOKEN>"
```

... or set the bot token later:
```bash
dotnet new telegram-bot --name MyTelegramBot

cd MyTelegramBot

dotnet user-secrets set Telegram:BotToken "<TOKEN>"
```

Run your bot with a single command:

```bash
dotnet run
```

And just like that, your bot is live and ready to chat!

## How To

> ℹ️ **Note**: As this project grows, these guides may move to a dedicated wiki for even more detailed documentation.

### Add a Text Command

Commands are the heart of any Telegram bot. Here's how to create one using this framework. For inspiration, check out the [StartCommand.cs](templates/TelegramBot/Commands/StartCommand.cs) example.

1. **Create a Command Class**  
   Implement the `IChatCommand` interface to define your command's behavior:

   ```csharp
   public class TestCommand : IChatCommand 
   {
       public bool CanExecute(CommandContext context) => 
           context.Command?.Equals("/help", StringComparison.OrdinalIgnoreCase) ?? false;

       public async Task ExecuteAsync(ITelegramBotClient botClient, CommandContext context, 
           CancellationToken cancellationToken)
       {
           await botClient.SendMessageAsync(context.Message.Chat.Id, "Hiiii!!", 
               cancellationToken: cancellationToken);
       }
   }
   ```

2. **Decorate with BotCommand Attribute**  
   Add metadata to your command, including its name, description, and an optional usage example:

   ```csharp
   [BotCommand("/test", "Description for the /test command", "/test <arg>")]
   ```

3. **Register the Command**  
   Add your command to the service collection in [Program.cs](templates/TelegramBot/Program.cs):

   ```csharp
   builder.Services.AddCommand<TestCommand>();
   ```

4. **Celebrate Your Success**  
   🤗 Great job! Give yourself a pat on the head—you've just added a new command!

### Add Middleware

Middleware lets you intercept and process updates in a modular way, much like ASP.NET. This framework supports two types of middleware: **anonymous** (for simple tasks) and **typed** (for complex scenarios). Remember, middleware executes in the order it's registered, so plan your pipeline carefully.

#### Anonymous Middleware

Perfect for quick, lightweight tasks. Check out the example in [Program.cs](templates/TelegramBot/Program.cs) for reference.

1. **Register Your Middleware**  
   Add your middleware to the pipeline in [Program.cs](templates/TelegramBot/Program.cs):

   ```csharp
   app.Use(async (context, next) => 
   {
       // Pre-processing: Log or modify the update before passing it along
       await context.Client.SendMessageAsync(context.Message.Chat.Id, 
           "Update received, processing...", cancellationToken: context.CancellationToken);

       // Pass control to the next middleware in the pipeline
       await next(context);

       // Post-processing: Perform actions after the pipeline completes
       await context.Client.SendMessageAsync(context.Message.Chat.Id, 
           "Done processing the update!", cancellationToken: context.CancellationToken);
   });
   ```

2. **You're Done!**  
   🤗 That's it—your middleware is ready to roll!

#### Typed Middleware

Ideal for more complex logic or reusable middleware. Use [RequestTimerMiddleware.cs](templates/TelegramBot/Middleware/RequestTimerMiddleware.cs) as a reference.

1. **Create a Middleware Class**  
   Implement the `IBotMiddleware` interface:

   ```csharp
   public class TestMiddleware : IBotMiddleware
   {
       public async Task InvokeAsync(UpdateContext context, BotMiddlewareDelegate next)
       {
           // Pre-processing: Log or modify the update
           await context.Client.SendMessageAsync(
                context.MessageChat.Id, 
                "Update received, processing...",
                cancellationToken: context.CancellationToken);

           // Pass control to the next middleware
           await next(context);

           // Post-processing: Actions after the pipeline completes
           await context.Client.SendMessageAsync(
                context.Message.Chat.Id, 
                "Done processing the update!",
                cancellationToken: context.CancellationToken);
       }
   }
   ```

2. **Register Your Middleware**  
   Add it to the pipeline in [Program.cs](templates/TelegramBot/Program.cs):

   ```csharp
   app.Use<TestMiddleware>();
   ```

3. **You're Done!**  
   🤗 That's it—your typed middleware is now part of the pipeline!

### Add Localization

This framework makes it easy to support multiple languages using RESX-based localization. For detailed guidance, refer to the [official Microsoft documentation](https://learn.microsoft.com/en-us/dotnet/core/extensions/create-resource-files#resources-in-resx-files).

Once you've added your strings and locales, access them in your code like this:

```csharp
using YourNamespace.Resources;

// Access command-specific strings
Console.WriteLine(CommandStrings.Start_Welcome);

// Access system-wide strings
Console.WriteLine(SystemStrings.ErrorHandling_UnexpectedError);
```

### Add Configuration Options

This framework uses the same powerful configuration system as ASP.NET, supporting multiple sources like `appsettings.json`, environment variables, `.ini` files, command-line arguments, and user secrets.

You can bind configuration sections to models or access values directly using string identifiers. For example:

```csharp
// Bind a configuration section to a model
var myOptions = builder.Configuration.GetSection("MyOptions").Get<MyOptionsModel>();

// Access a value directly
var myValue = builder.Configuration["MySection:MyKey"];
```

## Contributing

This is a solo passion project, but I warmly welcome contributions from the community! Whether it's a bug fix, a new feature, or just a suggestion, feel free to submit a Pull Request. I'll review it as quickly as possible and work with you to get it merged.

## License

This project is licensed under the **Mozilla Public License 2.0**. See the [LICENSE](LICENSE) file for full details.

#### 🤔 TL;DR – What This Means for You

- ✅ **Use Freely**: You can use this framework in your projects, including commercial ones.
- ✅ **Modify**: Feel free to tweak the code to suit your needs.
- ✅ **Distribute**: Share your modified versions with others.
- ⚠️ **Share Modifications**: If you modify the project's files, you must distribute those changes under the same license.
- ❌ **No Rebranding**: You can't simply rebrand and sell this project as-is.