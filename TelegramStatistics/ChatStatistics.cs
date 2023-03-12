using TelegramStatistics.Interfaces;
using TelegramStatistics.Models;

namespace TelegramStatistics
{
    public class ChatStatistics : IChatStatistics
    {        

        private readonly IChatService _chatService;
        private readonly ITextAnalyzer _textAnalyzer;

        public ChatStatistics(IChatService chatService, ITextAnalyzer textAnalyzer)
        {
            _chatService = chatService;
            _textAnalyzer = textAnalyzer;
        }



        public int GetTotalMessageCount(Chat chat)
        {            
            return chat.Messages!.Count;
        }



        public IEnumerable<WordCount> GetWordsUsage(IEnumerable<Message> messages, int? minimumWordFrequency)
        {
            minimumWordFrequency ??= 1;

            List<string> plainTexts = new();
            List<string> words = new();
            List<WordCount> wordCounts = new();

            plainTexts.AddRange(_chatService.GetPlainTexts(messages));

            words.AddRange(_textAnalyzer.SplitTextsIntoWords(plainTexts));

            wordCounts.AddRange(_textAnalyzer.CountWordUsage(words, minimumWordFrequency));

            return wordCounts;
        }



        public IEnumerable<UserWordCount> GetWordsUsagePerUser(Chat chat, int minimumWordFrequency)
        {
            List<UserWordCount> userWordsStats = new();

            Dictionary<string, List<Message>?> usersMessages = _chatService!.GetUsersMessages(chat.Users!);

            foreach (var userMessages in usersMessages)
            {
                UserWordCount wordCounts = new(); 

                wordCounts.UserWordCounts.AddRange(GetWordsUsage(userMessages.Value!, minimumWordFrequency));

                wordCounts.UserName = userMessages.Key;

                userWordsStats.Add(wordCounts);
            }

            return userWordsStats;
        }



        public Dictionary<string, int> GetMessageCountOfEverySender(Chat chat)
        {
            Dictionary<string, int> userMessageCounts = new();

            foreach (var user in chat!.Users!)
            {
                userMessageCounts.Add(user.From!, user!.Messages!.Count);
            }

            return userMessageCounts;
        }

    }
}
