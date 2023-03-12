using TelegramStatistics.Models;

namespace TelegramStatistics.Interfaces
{
    public interface IDeserializer
    {
        public Task<Chat> DeserializeFile(string path);
    }
}
