// <copyright file="AppDispatcherService.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using Drastic.Services;

namespace MauiChat.Services
{
    /// <summary>
    /// MAUI App Dispatcher Service.
    /// </summary>
    public class AppDispatcherService : IAppDispatcher
    {
        /// <inheritdoc/>
        public bool Dispatch(Action action)
        {
            return Microsoft.Maui.Controls.Application.Current!.Dispatcher.Dispatch(action);
        }
    }
}