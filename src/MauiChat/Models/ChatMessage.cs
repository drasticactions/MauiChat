// <copyright file="ChatMessage.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MauiChat.Models
{
    public class ChatMessage : INotifyPropertyChanged
    {
        private bool isBusy;
        private string message;

        public ChatMessage(byte[] icon, string message, ChatMessageType type)
        {
            this.Icon = icon;
            this.Message = message;
            this.Type = type;
        }

        public ChatMessage(byte[] icon, ChatMessageType type)
        {
            this.Icon = icon;
            this.Type = type;
            this.IsBusy = true;
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler? PropertyChanged;

        public bool IsBusy
        {
            get { return this.isBusy; }
            set { this.SetProperty(ref this.isBusy, value); }
        }

        public string Message
        {
            get
            {
                return this.message;
            }

            set
            {
                this.IsBusy = string.IsNullOrEmpty(value);
                this.SetProperty(ref this.message, value);
            }
        }

        public ChatMessageType Type { get; }

        public byte[] Icon { get; }

#pragma warning disable SA1600 // Elements should be documented
        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "", Action? onChanged = null)
#pragma warning restore SA1600 // Elements should be documented
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
            {
                return false;
            }

            backingStore = value;
            onChanged?.Invoke();
            this.OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// On Property Changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = this.PropertyChanged;
            if (changed == null)
            {
                return;
            }

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
