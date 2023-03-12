using TelegramStatistics.Models;

namespace TelegramStatistics.Interfaces
{
    public interface IFileWriter
    {
        public void WriteFile(Chat chat,string filePath, IEnumerable<WordCount> generalWordsUsage, IEnumerable<UserWordCount> usersWordCounts, Dictionary<string, int> numberOfMessagesByUsers);
    }
}
