using System.Text.Json;
using TelegramStatistics.Interfaces;
using TelegramStatistics.Models;

namespace TelegramStatistics
{
    public class JsonDeserializer : IDeserializer
    {
        public async Task<Chat> DeserializeFile(string path)
        {
            using FileStream openStream = File.OpenRead(path);

            var chat = await JsonSerializer.DeserializeAsync<Chat>(openStream);

            return chat!;
        }
    }
}
