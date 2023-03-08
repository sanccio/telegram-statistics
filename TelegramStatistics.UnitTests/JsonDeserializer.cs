using System.Text.Json;
using TelegramStatistics.Models;

namespace TelegramStatistics.UnitTests
{
    internal class JsonDeserializer
    {
        public static Chat GetData(string fileName)
        {
            string jsonString = File.ReadAllText(fileName);
            Chat chat = JsonSerializer.Deserialize<Chat>(jsonString)!;

            return chat;
        }
    }
}
