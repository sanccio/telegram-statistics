using System.Text.RegularExpressions;
using TelegramStatistics.Interfaces;
using TelegramStatistics.Models;

namespace TelegramStatistics
{
    public class TextAnalyzer : ITextAnalyzer
    {

        public IEnumerable<string> SplitTextsIntoWords(IEnumerable<string> texts)
        {
            string pattern = @"[^\p{L}'0-9]|(?<!\w)'(?!\w)";

            return texts
                .SelectMany(textLine => Regex.Split(ClearTextFromEmoji(textLine), pattern))
                .Where(word => !string.IsNullOrEmpty(word))
                .Select(word => word.ToLower());
        }



        public string ClearTextFromEmoji(string text)
        {
            return Regex.Replace(text, @"\p{Cs}", "");
        }



        public IEnumerable<WordCount> CountWordUsage(IEnumerable<string> words, int? minimumWordFrequency)
        {
            int minWordLength = 3;

            var query = words
                .GroupBy(x => x)
                .Where(x => x.Key.Length >= minWordLength)
                .Select(x => new WordCount
                {
                    Text = x.Key,
                    Number = x.Count()
                })
                .Where(x => x.Number >= minimumWordFrequency)
                .OrderByDescending(x => x.Number)
                .ToList();

            ExcludeNoninformativeWords(query);

            return query;
        }



        private static void ExcludeNoninformativeWords(List<WordCount> query)
        {
            List<string> WordUnitsToExclude = new()
            {
                "без", "для", "под", "над", "при", "чтобы", 
                "что", "как", "будто", "когда", "чем", "это", 
                "так", "там", "или", "про", "кто"
            };

            query.RemoveAll(w => WordUnitsToExclude.Contains(w.Text!));
        }

    }
}
