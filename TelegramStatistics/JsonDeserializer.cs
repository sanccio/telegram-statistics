using Newtonsoft.Json;
using TelegramStatistics.Interfaces;
using TelegramStatistics.Models;

namespace TelegramStatistics
{
    public class JsonDeserializer : IDeserializer
    {
        public Chat DeserializeFile(string path)
        {
            using FileStream openStream = File.OpenRead(path);
            using StreamReader file = new(openStream);
            string json = file.ReadToEnd();

            var chat = JsonConvert.DeserializeObject<Chat>(json)
                ?? throw new InvalidOperationException($"Failed to deserialize file {path}");

            return chat;
        }
    }
}
