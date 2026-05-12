using System;
using System.IO;

namespace ApiClient.Core.Services
{
    public sealed class FileApiResponseWriter
    {
        public string Write(string filePath, string content)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("File path is required.", nameof(filePath));
            }

            File.WriteAllText(filePath, content ?? string.Empty);
            return filePath;
        }
    }
}
