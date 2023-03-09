// <copyright file="ChatViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.ObjectModel;
using Drastic.Tools;
using Drastic.ViewModels;
using MauiChat.Models;
using MauiChat.Services;
using MauiChat.Utilities;

namespace MauiChat.ViewModels
{
    public class ChatViewModel : BaseViewModel
    {
        private string message;
        private ChatGPTClientWrapper wrapper;
        private byte[] placeholderIcon;
        private bool sessionStarted;

        public ChatViewModel(IServiceProvider services)
            : base(services)
        {
            this.placeholderIcon = LibraryUtilities.GetPlaceholderIcon();
            this.wrapper = services.GetService(typeof(ChatGPTClientWrapper)) as ChatGPTClientWrapper ?? throw new NullReferenceException(nameof(ChatGPTClientWrapper));
            this.SendMessageCommand = new AsyncCommand(async () => await this.SendCommandAsync(this.Message), () => !string.IsNullOrEmpty(this.Message) && this.sessionStarted, this.Dispatcher, this.ErrorHandler);
            this.SendMessageWithStringCommand = new AsyncCommand<string>(this.SendCommandAsync, (x) => !string.IsNullOrEmpty(x) && this.sessionStarted, this.ErrorHandler);
        }

        public event EventHandler<EventArgs>? OnSendingMessage;

        public event EventHandler<EventArgs>? OnMessageReceived;

        public ObservableCollection<ChatMessage> Messages { get; private set; } = new ObservableCollection<ChatMessage>();

        public string Message
        {
            get
            {
                return this.message;
            }

            set
            {
                this.SetProperty(ref this.message, value);
                this.SendMessageCommand.RaiseCanExecuteChanged();
            }
        }

        public AsyncCommand SendMessageCommand { get; }

        public AsyncCommand<string> SendMessageWithStringCommand { get; }

        public override Task OnLoad()
        {
            if (!this.Messages.Any())
            {
                this.StartSessionAsync().FireAndForgetSafeAsync();
            }

            return Task.CompletedTask;
        }

        private async Task StartSessionAsync() {
            this.sessionStarted = true;
            var chatMessage = new ChatMessage(this.placeholderIcon, ChatMessageType.AI);
            this.Messages.Add(chatMessage);
            var message = await this.wrapper.StartAsync();
            await Task.Delay(1000);
            chatMessage.Message = message;
            this.OnMessageReceived?.Invoke(this, EventArgs.Empty);
        }

        private async Task EndSessionAsync() {
            this.sessionStarted = false;
            var chatMessage = new ChatMessage(this.placeholderIcon, ChatMessageType.AI);
            this.Messages.Add(chatMessage);
            chatMessage.Message = await this.wrapper.StopAsync();
            this.OnMessageReceived?.Invoke(this, EventArgs.Empty);
        }

        private async Task SendCommandAsync(string message) {
            this.Message = string.Empty;
            this.OnSendingMessage?.Invoke(this, new EventArgs());
            this.Messages.Add(new ChatMessage(this.placeholderIcon, message, ChatMessageType.User));
            await Task.Delay(1000);
            await this.PerformBusyAsyncTask(() => this.SendQueryToModelAsync(message));
        }

        private async Task SendQueryToModelAsync(string message) {
            var chatMessage = new ChatMessage(this.placeholderIcon, ChatMessageType.AI);
            this.Messages.Add(chatMessage);

            var result = await this.wrapper.QueryAsync(message);
            if (!string.IsNullOrEmpty(result)) {
                chatMessage.Message = result;
            }
            else {
                // If we get an empty response from the model, something went wrong.
                // Remove the loading message.
                this.Messages.Remove(chatMessage);
            }

            this.OnMessageReceived?.Invoke(this, EventArgs.Empty);
        }
    }
}
