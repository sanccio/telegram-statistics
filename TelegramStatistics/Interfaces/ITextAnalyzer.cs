using TelegramStatistics.Models;

namespace TelegramStatistics.Interfaces
{
    public interface ITextAnalyzer
    {
        public IEnumerable<string> SplitTextsIntoWords(IEnumerable<string> plainTexts);

        public string ClearTextFromEmoji(string text);

        public IEnumerable<WordCount> CountWordUsage(IEnumerable<string> words, int? minimumWordFrequency);
    }
}
