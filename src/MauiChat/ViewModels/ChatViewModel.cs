﻿// <copyright file="ChatViewModel.cs" company="Drastic Actions">
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

        private async Task StartSessionAsync()
        {
            this.sessionStarted = true;
            var message = await this.wrapper.StartAsync();
            var chatMessage = new ChatMessage(this.placeholderIcon, message, ChatMessageType.AI);
            this.Messages.Add(chatMessage);
        }

        private async Task EndSessionAsync()
        {
            this.sessionStarted = false;
            var message = await this.wrapper.StopAsync();
            var chatMessage = new ChatMessage(this.placeholderIcon, message, ChatMessageType.AI);
            this.Messages.Add(chatMessage);
        }

        private async Task SendCommandAsync(string message)
        {
            this.Message = string.Empty;
            this.OnSendingMessage?.Invoke(this, new EventArgs());
            this.Messages.Add(new ChatMessage(this.placeholderIcon, message, ChatMessageType.User));
            await this.PerformBusyAsyncTask(() => this.SendQueryToModelAsync(message));
        }

        private async Task SendQueryToModelAsync(string message)
        {
            var aiMessage = string.Empty;

            var result = await this.wrapper.QueryAsync(message);
            if (!string.IsNullOrEmpty(result))
            {
                aiMessage = result;
            }
            else
            {
                // If we get an empty response from the model, something went wrong.
                // Remove the loading message.
                return;
            }

            var chatMessage = new ChatMessage(this.placeholderIcon, aiMessage, ChatMessageType.AI);
            this.Messages.Add(chatMessage);
        }
    }
}
