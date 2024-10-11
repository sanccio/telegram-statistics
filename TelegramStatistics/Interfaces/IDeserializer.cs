using TelegramStatistics.Models;

namespace TelegramStatistics.Interfaces
{
    public interface IDeserializer
    {
        public Chat DeserializeFile(string path);
    }
}
