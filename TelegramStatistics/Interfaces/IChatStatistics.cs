using TelegramStatistics.Models;

namespace TelegramStatistics.Interfaces
{
    public interface IChatStatistics
    {
        public int GetTotalMessageCount(Chat chat);

        public IEnumerable<WordCount> GetWordsUsage(IEnumerable<Message> messages, int? minimumWordFrequency = 1);

        public IEnumerable<UserWordCount> GetWordsUsagePerUser(Chat chat, int? minimumWordFrequency = 1);

        public Dictionary<string, int> GetMessageCountPerUser(Chat chat, int? year = null);

        public Dictionary<int, int> GetMessageCountPerYear(Chat chat);

        public Dictionary<int, int> GetMessageCountPerMonth(Chat chat, int year);

        public Dictionary<int, int> GetAggregateMessageCountPerHour(Chat chat, int? year = null, int? month = null, int? dayOfMonth = null);
        
        public List<HourlyMessageCount> GetIndividualMessageCountPerHour(Chat chat, int? year = null, int? month = null, int? dayOfMonth = null);

        public Dictionary<string, int> GetTopActiveDates(Chat chat, int count);
    }
}
