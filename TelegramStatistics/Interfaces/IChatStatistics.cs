using TelegramStatistics.Models;

namespace TelegramStatistics.Interfaces
{
    public interface IChatStatistics
    {
        public int GetTotalMessageCount(Chat chat);

        public IEnumerable<WordCount> GetWordsUsage(IEnumerable<Message> messages, int? minimumWordFrequency = 1);

        public IEnumerable<UserWordCount> GetWordsUsagePerUser(Chat chat, int? minimumWordFrequency = 1);

        public Dictionary<string, int> GetMessageCountPerUser(Chat chat);
    }
}
