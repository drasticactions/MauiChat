// <copyright file="App.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace MauiChat;

public partial class App : Application
{
    private IServiceProvider provider;

    public App(IServiceProvider provider)
    {
        this.provider = provider;

        this.InitializeComponent();

        this.MainPage = new NavigationPage(new MainPage(provider));
    }
}
