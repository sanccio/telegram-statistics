using System.Text.Json;
using TelegramStatistics.Interfaces;
using TelegramStatistics.Models;

namespace TelegramStatistics
{
    public class JsonDeserializer : IDeserializer
    {
        public Chat DeserializeFile(string path)
        {
            using FileStream openStream = File.OpenRead(path);

            var chat = JsonSerializer.Deserialize<Chat>(openStream)
                ?? throw new InvalidOperationException($"Failed to deserialize file {path}");

            return chat;
        }
    }
}
