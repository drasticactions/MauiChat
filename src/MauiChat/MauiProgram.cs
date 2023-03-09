// <copyright file="MauiProgram.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Brushtail.FontAwesome.Mobile;
using Drastic.Services;
using MauiChat.Services;
using MauiChat.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenAI.GPT3.Extensions;

namespace MauiChat;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        Microsoft.Maui.Handlers.ButtonHandler.Mapper.AppendToMapping("CatalystButton", (handler, view) =>
        {
#if MACCATALYST
#pragma warning disable CA1416 // プラットフォームの互換性を検証
            handler.PlatformView.PreferredBehavioralStyle = UIKit.UIBehavioralStyle.Pad;
#pragma warning restore CA1416 // プラットフォームの互換性を検証
#endif
        });
        var builder = MauiApp.CreateBuilder();

        builder.Configuration.AddUserSecrets<App>();

        builder.Services!
       .AddSingleton<IErrorHandlerService, ErrorHandlerService>()
       .AddSingleton<IAppDispatcher, AppDispatcherService>()
       .AddOpenAIService()
       .AddSingleton<ChatGPTClientWrapper>()
       .AddSingleton<ChatViewModel>()
       ;
        builder
            .UseMauiApp<App>()
            .UseFontAwesome()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
