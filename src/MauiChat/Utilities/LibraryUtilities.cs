// <copyright file="LibraryUtilities.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Reflection;

namespace MauiChat.Utilities
{
    internal static class LibraryUtilities
    {
        /// <summary>
        /// Get the Default Placeholder Icon.
        /// </summary>
        /// <returns>Image Byte Array.</returns>
        /// <exception cref="Exception">Thrown if can't get the image.</exception>
        public static byte[] GetPlaceholderIcon()
        {
            var resource = GetResourceFileContent("Icon.dotnet_bot.png");
            if (resource is null)
            {
                throw new Exception("Failed to get placeholder icon.");
            }

            using MemoryStream ms = new MemoryStream();
            resource.CopyTo(ms);
            return ms.ToArray();
        }

        /// <summary>
        /// Get Resource File Content via FileName.
        /// </summary>
        /// <param name="fileName">Filename.</param>
        /// <returns>Stream.</returns>
        public static Stream? GetResourceFileContent(string fileName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "MauiChat." + fileName;
            if (assembly is null)
            {
                return null;
            }

            return assembly.GetManifestResourceStream(resourceName);
        }

        public static string GetResourceFileContentAsString(string fileName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            if (assembly is null)
            {
                return string.Empty;
            }

            var resourceName = "ThoughtBot." + fileName;

            string? resource = null;
            using (Stream? stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream is null)
                {
                    return string.Empty;
                }

                using StreamReader reader = new StreamReader(stream);
                resource = reader.ReadToEnd();
            }

            return resource ?? string.Empty;
        }
    }
}
