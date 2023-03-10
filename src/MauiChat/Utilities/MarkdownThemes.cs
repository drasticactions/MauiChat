// <copyright file="MarkdownThemes.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Markdig;

namespace MauiChat.Utilities {
    public class DarkMarkdownTheme : Drastic.Markdown.DarkMarkdownTheme
    {
        public DarkMarkdownTheme() {
#if WINDOWS
            this.Paragraph.FontSize = 15;
#endif
        }
    }

    public class LightMarkdownTheme : Drastic.Markdown.LightMarkdownTheme {
        public LightMarkdownTheme() {
#if WINDOWS
            this.Paragraph.FontSize = 15;
#endif
        }
    }
}
