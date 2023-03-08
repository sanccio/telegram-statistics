using TelegramStatistics.Interfaces;
using TelegramStatistics.Models;

namespace TelegramStatistics
{
    public class ChatAnalyzer
    {        

        static IFileParser? _fileParser;
        
        public ChatAnalyzer(IFileParser fileParser)
        {
            _fileParser = fileParser;
        }



        public static int GetTotalMessageCount(Chat? chat)
        {            
            return chat!.Messages!.Count;
        }



        public static Dictionary<string, int> GetMessageCountOfEverySender(Chat? chat)
        {
            Dictionary<string, int> userMessageCounts = new();

            var users = _fileParser?.GroupAllMessagesBySender(chat);

            foreach (var user in users!)
            {
                userMessageCounts.Add(user.From!, user!.Messages!.Count);
            }

            return userMessageCounts;
        }



        public static IEnumerable<WordCount> CountWordUsage(IEnumerable<string> words, int minimumWordFrequency)
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
            
            ExcludeNoninformativeWordUnits(query);

            return query;
        }



        private static void ExcludeNoninformativeWordUnits(List<WordCount> query)
        {
            List<string> WordUnitsToExclude = new() 
            { 
                "без", "для", "под", "над", "при", 
                "чтобы", "что", "как", "будто", "когда",
                "чем", "это", "так", "там", "или",
                "про", "кто"
            };

            query.RemoveAll(w => WordUnitsToExclude.Contains(w.Text!));

        }


    }
}
