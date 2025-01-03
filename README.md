# 1ffycat.TelegramBotTemplates

A modern, middleware-based framework for building Telegram bots in .NET, offering a familiar development experience similar to ASP.NET.

## Features

- 🚀 ASP.NET-like middleware pipeline for processing updates
- 💉 Built-in Dependency Injection support
- 🌐 Resource management for internationalization
- ⚙️ Configuration management using IConfiguration
- 🎯 Automatic command parsing and context creation
- 🛡️ Built-in middleware for:
  - ⚠️ Exception handling
  - ⏱️ Performance monitoring
  - 📖 Update logging
  - ✂️ Command matching and execution
- 🔐 Integrated user-secrets management
- ⚡ Many extensions for common use cases (text formatting, asking for DM permissions)
- 🏗️ Based on .NET's application host model
- 📦 Built on top of the Telegram.Bot package

## Installation
```bash
dotnet new install 1ffycat.TelegramBotTemplates
```

## Quick Start
```bash
dotnet new telegram-bot --name MyTelegramBot --token "<YOUR TOKEN>"
```

And you're good to go! `dotnet run` and your bot is up and running.

## How to:
(Might need a dedicated wiki later...)
### Add a text command
You may use [StartCommand.cs](templates/TelegramBot/Commands/StartCommand.cs) as a reference.

1. Create a command class that implements `IChatCommand`:
    ```csharp
    public class TestCommand : IChatCommand 
    {
        public bool CanExecute(CommandContext context) => context.Command?.Equals("/help", StringComparison.OrdinalIgnoreCase) ?? false;

        public async Task ExecuteAsync(ITelegramBotClient botClient, CommandContext context,
        CancellationToken cancellationToken)
        {
            await botClient.SendMessage(context.Message.Chat.Id, "Hiiii!!", cancellationToken: cancellationToken);
        }
    }
    ```
2. Add a BotCommand attribute to the class and specify the command's name, description and optionally a usage example:
    ```csharp
    [BotCommand("/test", "Description for the /test command", "/test <arg>")]`).
    ```
3. Register the command in [Program.cs](templates/TelegramBot/Program.cs):
    ```csharp
    builder.Services.AddCommand<TestCommand>();
    ```

4. 🤗 Good job, pat yourself on the head!

### Add a middleware
This template has two types of middleware (just like in ASP.NET): typed and anonymous.

Please keep in mind that the middleware is executed in the order you registered it in the pipeline.

#### Anonymous middleware
Generally more suitable for something simple. [Program.cs](templates/TelegramBot/Program.cs) already has an example on how to use anonymous middleware, check it out.

1. Register your middleware in [Program.cs](templates/TelegramBot/Program.cs):
    ```csharp
    app.Use(async (context, next) => 
    {
        // Here you can write exactly the same code as in a typed middleware!

        // Send a message to the chat
        await context.Client.SendMessage(context.Message.Chat.Id, "Update received, processing...", cancellationToken: cancellationToken);

        // Continue the execution. You can skip this if you want to skip the update.
        await next(context);

        // You can even do things after the next middleware is done (which means all middleware further down the pipeline is also done)
        await context.Client.SendMessage(context.Message.Chat.Id, "Done processing the update!", cancellationToken: cancellationToken);
    });
    ```
2. 🤗 That's it! 

#### Typed middleware
Works better for more complex scenarios. You can use the [RequestTimerMiddleware](templates/TelegramBot/Middleware/RequestTimerMiddleware.cs) as an example.

1. Create a middleware class that implements IBotMiddleware:
    ```csharp
    public class TestMiddleware : IBotMiddleware
    {
        public async Task InvokeAsync(UpdateContext context, BotMiddlewareDelegate next)
        {
            // Here you can write exactly the same code as in an anonymous middleware!

            // Send a message to the chat
            await context.Client.SendMessage(context.Message.Chat.Id, "Update received, processing...", cancellationToken: cancellationToken);

            // Continue the execution. You can skip this if you want to skip the update.
            await next(context);

            // You can even do things after the next middleware is done (which means all middleware further down the pipeline is also done)
            await context.Client.SendMessage(context.Message.Chat.Id, "Done processing the update!", cancellationToken: cancellationToken);
        }
    }
    ```
2. Register your middleware in [Program.cs](templates/TelegramBot/Program.cs):
    ```csharp
    app.Use<TestMiddleware>();
    ```
3. 🤗 That's it! 

### Add localization
This templates offers a built-in support for RESX localization. Please refer to the [official Microsoft documentation](https://learn.microsoft.com/en-us/dotnet/core/extensions/create-resource-files#resources-in-resx-files).

After adding new strings and locales, you can easily access the strings from your code:
```csharp
using YourNamespace.Resources;

//                    SECTION        NAME
Console.WriteLine(CommandStrings.Start_Welcome);
//                   SECTION               NAME
Console.WriteLine(SystemStrings.ErrorHandling_UnexpectedError);
```

### Add configuration optinos
This template uses the same configuration framework as ASP.NET does.

You can bind sections to models or use string identifiers.

All configuration sources (appsettings.json, env, .ini, args, user secrets, etc.) are supported.

## Contributing
It's a solo pet project, so all contributions are welcome! Please feel free to submit a Pull Request and I'll do my best to review it ASAP.

## License
This project is licensed under the **Mozilla Public License 2.0** - see the [LICENSE](LICENSE) file for details.

#### 🤔 TL;DR
- ✅ You can use it in your projects (including commercial ones)
- ✅ You can modify the code
- ✅ You can distribute your modified version
- ⚠️ If you modify project's files, you must share these modifications under the same license
- ❌ You can't just rebrand and sell the project as-is