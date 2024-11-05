using TelegramStatistics.Models;

namespace TelegramStatistics.Interfaces
{
    public interface IChatService
    {
        IEnumerable<string> GetPlainTexts(IEnumerable<Message> messages);
    }
}
