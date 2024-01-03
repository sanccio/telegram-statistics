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


        public IEnumerable<WordCount> GetWordsUsage(IEnumerable<Message> messages, int? minimumWordFrequency = 1)
        {
            IEnumerable<string> plainTexts = _chatService.GetPlainTexts(messages);
            IEnumerable<string> words = _textAnalyzer.SplitTextsIntoWords(plainTexts);

            return _textAnalyzer.CountWordUsage(words, minimumWordFrequency);
        }


        public IEnumerable<UserWordCount> GetWordsUsagePerUser(Chat chat, int? minimumWordFrequency = 1)
        {
            List<UserWordCount> userWordsStats = new();

            Dictionary<string, List<Message>?> usersMessages = _chatService.GetUsersMessages(chat.Users!);

            foreach (var userMessages in usersMessages)
            {
                UserWordCount wordCounts = new()
                {
                    UserName = userMessages.Key,
                    UserWordCounts = GetWordsUsage(userMessages.Value!, minimumWordFrequency).ToList()
                };

                userWordsStats.Add(wordCounts);
            }

            return userWordsStats;
        }


        public Dictionary<string, int> GetMessageCountPerUser(Chat chat)
        {
            Dictionary<string, int> messageCountPerUser = new();

            foreach (var user in chat!.Users!)
            {
                messageCountPerUser.Add(
                    key: user.From!,
                    value: user!.Messages!.Count);
            }

            return messageCountPerUser;
        }
    }
}
