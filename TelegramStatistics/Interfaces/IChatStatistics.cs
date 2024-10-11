using TelegramStatistics.Models;

namespace TelegramStatistics.Interfaces
{
    public interface IChatStatistics
    {
        void SetChat(Chat chat);
        
        int GetTotalMessageCount();

        IEnumerable<WordCount> GetGeneralWordsUsage(int? minimumWordFrequency = 1);

        IEnumerable<UserWordCount> GetWordsUsagePerUser(int? minimumWordFrequency = 1);

        Dictionary<string, int> GetMessageCountPerUser(int? year = null);

        Dictionary<int, int> GetMessageCountPerYear();

        Dictionary<int, int> GetMessageCountPerMonth(int year);

        List<HourlyMessageCount> GetIndividualMessageCountPerHour(int? year = null, int? month = null, int? dayOfMonth = null);

        Dictionary<string, int> GetTopActiveDates(int count);

        int[] GetChatActiveYears();
    }
}
