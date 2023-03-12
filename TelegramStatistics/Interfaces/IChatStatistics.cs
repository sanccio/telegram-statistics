using TelegramStatistics.Models;

namespace TelegramStatistics.Interfaces
{
    public interface IChatStatistics
    {
        public int GetTotalMessageCount(Chat chat);

        public IEnumerable<WordCount> GetWordsUsage(IEnumerable<Message> messages, int? minimumWordFrequency);

        public IEnumerable<UserWordCount> GetWordsUsagePerUser(Chat chat, int minimumWordFrequency);

        public Dictionary<string, int> GetMessageCountOfEverySender(Chat chat);
    }
}
