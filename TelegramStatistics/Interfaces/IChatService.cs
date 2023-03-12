using TelegramStatistics.Models;

namespace TelegramStatistics.Interfaces
{
    public interface IChatService
    {
        public IEnumerable<string> GetPlainTexts(IEnumerable<Message> messages);

        public Dictionary<string, List<Message>?> GetUsersMessages(List<User> users);

        public void GroupAllMessagesBySender(Chat chat);

        public IEnumerable<string?> GetSendersNames(List<Message> messages);
    }
}
