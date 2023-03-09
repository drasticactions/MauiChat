// <copyright file="ChatGPTClientWrapper.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Globalization;
using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3.Managers;
using OpenAI.GPT3.ObjectModels.RequestModels;

namespace MauiChat.Services
{
    public class ChatGPTClientWrapper
    {
        private IOpenAIService openAIService;

        public ChatGPTClientWrapper(IOpenAIService openAIService)
        {
            this.openAIService = openAIService;
        }

        public async Task<string> StartAsync()
        {
            var language = CultureInfo.CurrentUICulture.ToString();
            var completionResult = await this.openAIService.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
            {
                Messages = new List<ChatMessage>
                    {
                        ChatMessage.FromSystem("You are a helpful assistant called MauiBot. " +
                        $"You speak in {language}. " +
                        "You are in a program written using the MAUI UI Framework, " +
                        "where users can talk to you to see a demostration of how MAUI works."),
                        ChatMessage.FromUser("Hello MauiBot. Please introduce yourself."),
                    },
                Model = OpenAI.GPT3.ObjectModels.Models.ChatGpt3_5Turbo,
                MaxTokens = 250,
            });
            if (completionResult.Successful)
            {
                return completionResult.Choices.FirstOrDefault()?.Message.Content ?? string.Empty;
            }
            else
            {
                if (completionResult.Error == null)
                {
                    throw new Exception("Unknown Error");
                }

                return $"{completionResult.Error.Code}: {completionResult.Error.Message}";
            }
        }

        public async Task<string> StopAsync()
        {
            var language = CultureInfo.CurrentUICulture.ToString();
            var completionResult = await this.openAIService.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
            {
                Messages = new List<ChatMessage>
                    {
                        ChatMessage.FromSystem("You are a helpful assistant called MauiBot. " +
                        $"You speak in {language}. " +
                        "You are in a program written using the MAUI UI Framework, " +
                        "where users can talk to you to see a demostration of how MAUI works."),
                        ChatMessage.FromUser("Goodbye!"),
                    },
                Model = OpenAI.GPT3.ObjectModels.Models.ChatGpt3_5Turbo,
                MaxTokens = 2500,
            });
            if (completionResult.Successful)
            {
                return completionResult.Choices.FirstOrDefault()?.Message.Content ?? string.Empty;
            }
            else
            {
                if (completionResult.Error == null)
                {
                    throw new Exception("Unknown Error");
                }

                return $"{completionResult.Error.Code}: {completionResult.Error.Message}";
            }
        }

        public async Task<string> QueryAsync(string q)
        {
            var completionResult = await this.openAIService.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
            {
                Messages = new List<ChatMessage>
                    {
                        ChatMessage.FromSystem("You are a helpful assistant called MauiBot. " +
                        "You speak in whatever language the user speaks. " +
                        "You are in a program written using the MAUI UI Framework, " +
                        "where users can talk to you to see a demostration of how MAUI works."),
                        ChatMessage.FromUser(q),
                    },
                Model = OpenAI.GPT3.ObjectModels.Models.ChatGpt3_5Turbo,
                MaxTokens = 2500,
            });
            if (completionResult.Successful)
            {
                return completionResult.Choices.FirstOrDefault()?.Message.Content ?? string.Empty;
            }
            else
            {
                if (completionResult.Error == null)
                {
                    throw new Exception("Unknown Error");
                }

                return $"{completionResult.Error.Code}: {completionResult.Error.Message}";
            }
        }
    }
}
