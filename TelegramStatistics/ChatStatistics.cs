using System.Diagnostics.CodeAnalysis;
using TelegramStatistics.Interfaces;
using TelegramStatistics.Models;

namespace TelegramStatistics
{
    public class ChatStatistics : IChatStatistics
    {
        private Chat? chat;
        private readonly IChatService _chatService;
        private readonly ITextAnalyzer _textAnalyzer;

        public ChatStatistics(IChatService chatService, ITextAnalyzer textAnalyzer)
        {
            _chatService = chatService;
            _textAnalyzer = textAnalyzer;
        }


        public void SetChat(Chat chat)
        {
            this.chat = chat ?? throw new ArgumentNullException(nameof(chat), "Chat cannot be null.");
            _chatService.GroupAllMessagesBySender(chat);
        }

        [MemberNotNull(nameof(chat))]
        private void EnsureChatIsSet()
        {
            if (chat is null) 
            { 
                throw new InvalidOperationException("Chat has not been initialized."); 
            }
        }


        public int GetTotalMessageCount()
        {
            EnsureChatIsSet();

            return chat.Users.Sum(u => u.Messages.Count);
        }


        public IEnumerable<WordCount> GetGeneralWordsUsage(int? minimumWordFrequency = 1)
        {
            EnsureChatIsSet();

            return GetWordsUsageFromMessages(chat.Messages, minimumWordFrequency);
        }


        private IEnumerable<WordCount> GetWordsUsageFromMessages(IEnumerable<Message> messages, int? minimumWordFrequency = 1)
        {
            IEnumerable<string> plainTexts = _chatService.GetPlainTexts(messages);
            IEnumerable<string> words = _textAnalyzer.SplitTextsIntoWords(plainTexts);

            return _textAnalyzer.CountWordUsage(words, minimumWordFrequency);
        }


        public IEnumerable<UserWordCount> GetWordsUsagePerUser(int? minimumWordFrequency = 1)
        {
            EnsureChatIsSet();

            List<UserWordCount> userWordsStats = new();

            Dictionary<string, List<Message>> usersMessages = _chatService.GetUsersMessages(chat.Users);

            foreach (var userMessages in usersMessages)
            {
                UserWordCount wordCounts = new()
                {
                    UserName = userMessages.Key,
                    UserWordCounts = GetWordsUsageFromMessages(userMessages.Value!, minimumWordFrequency).ToList()
                };

                userWordsStats.Add(wordCounts);
            }

            return userWordsStats;
        }


        public Dictionary<string, int> GetMessageCountPerUser(int? year = null)
        {
            EnsureChatIsSet();

            Dictionary<string, int> messageCountPerUser = chat.Users.ToDictionary(
                u => u.From,
                u => year.HasValue 
                    ? u.Messages.Count(m => m.Date.Year == year) 
                    : u.Messages.Count);

            return messageCountPerUser;
        }


        public Dictionary<int, int> GetMessageCountPerYear()
        {
            EnsureChatIsSet();

            var messageCountPerYear = chat.Messages
                .Where(m => m.Type != "service")
                .GroupBy(m => m.Date.Year)
                .ToDictionary(yearGroup => yearGroup.Key, yearGroup => yearGroup.Count());

            return messageCountPerYear;
        }


        public Dictionary<int, int> GetMessageCountPerMonth(int year)
        {
            EnsureChatIsSet();

            var messageCountPerMonth = chat.Messages
                .Where(m => m.Type != "service"
                            && m.Date.Year == year)
                .GroupBy(m => m.Date.Month)
                .ToDictionary(monthGroup => monthGroup.Key, monthGroup => monthGroup.Count());

            return messageCountPerMonth;
        }


        public List<HourlyMessageCount> GetIndividualMessageCountPerHour(int? year = null,
                                                                         int? month = null,
                                                                         int? dayOfMonth = null)
        {
            EnsureChatIsSet();

            IEnumerable<Message> filteredMessages = chat.Messages.Where(m => m.Type != "service");

            if (year.HasValue)
            {
                filteredMessages = filteredMessages.Where(m => m.Date.Year == year);
            }

            if (month.HasValue)
            {
                filteredMessages = filteredMessages.Where(m => m.Date.Month == month);
            }

            if (dayOfMonth.HasValue)
            {
                filteredMessages = filteredMessages.Where(m => m.Date.Day == dayOfMonth);
            }

            var messageCountPerHour = filteredMessages
                .GroupBy(m => m.Date.Hour)
                .OrderBy(hourGroup => hourGroup.Key)
                .Select(hourGroup => new HourlyMessageCount()
                {
                    Hour = hourGroup.Key,
                    UserMessageCount = hourGroup
                        .GroupBy(m => m.From!)
                        .OrderBy(fromGroup => fromGroup.Key)
                        .ToDictionary(fromGroup => fromGroup.Key, fromGroup => fromGroup.Count())
                })
                .ToList();

            return messageCountPerHour;
        }


        public Dictionary<string, int> GetTopActiveDates(int count)
        {
            EnsureChatIsSet();

            var topActiveDays = chat.Messages
                    .Where(m => m.Type != "service")
                    .GroupBy(m => m.Date.ToShortDateString())
                    .Select(dateGroup => new { Date = dateGroup.Key, MessageCount = dateGroup.Count() })
                    .OrderByDescending(x => x.MessageCount)
                    .Take(count)
                    .ToDictionary(x => x.Date, x => x.MessageCount);

            return topActiveDays;
        }


        public int[] GetChatActiveYears()
        {
            EnsureChatIsSet();

            if (!chat.Messages.Any()) return Array.Empty<int>();

            int chatStartYear = chat.Messages[0].Date.Year;
            int chatEndYear = chat.Messages[^1].Date.Year;

            int[] activeYears = Enumerable
                .Range(chatStartYear, chatEndYear - chatStartYear + 1)
                .OrderByDescending(y => y)
                .ToArray();

            return activeYears;
        }
    }
}
