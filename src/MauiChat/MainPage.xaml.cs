// <copyright file="MainPage.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Drastic.Markdown;
using Drastic.Tools;
using MauiChat.ViewModels;

namespace MauiChat;

public partial class MainPage : ContentPage
{
    private IServiceProvider provider;

    public MainPage(IServiceProvider provider)
    {
        this.provider = provider;
        this.InitializeComponent();

        this.BindingContext = this.VM = this.provider.GetService<ChatViewModel>()!;
        this.VM.OnMessageReceived += this.VM_OnMessageReceived;
    }

    public ChatViewModel VM { get; }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        this.VM.OnLoad().FireAndForgetSafeAsync();
    }

    private void UserEntry_Completed(object sender, System.EventArgs e)
    {
        this.VM.SendMessageCommand.ExecuteAsync().FireAndForgetSafeAsync();
    }

    private void VM_OnMessageReceived(object? sender, EventArgs e)
    {
        // Workaround for https://github.com/dotnet/maui/issues/13451
        // Force the height to redraw when the message is fully loaded.
#if WINDOWS
        var test = (Microsoft.UI.Xaml.Controls.ListView)this.Conversation.Handler!.PlatformView!;
        test.UpdateLayout();
#endif
    }
}

public class SpeakerSelector : DataTemplateSelector
{
    public DataTemplate ChatGPTTemplate { get; set; }

    public DataTemplate UserTemplate { get; set; }

    protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
    {
        var line = (Models.ChatMessage)item;

        return line.Type == Models.ChatMessageType.User
            ? this.UserTemplate
            : this.ChatGPTTemplate;
    }
}
