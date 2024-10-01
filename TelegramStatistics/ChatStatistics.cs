using System.Diagnostics;
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
            return chat.Users.Sum(x => x.Messages.Count);
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


        public Dictionary<string, int> GetMessageCountPerUser(Chat chat, int? year = null)
        {
            Dictionary<string, int> messageCountPerUser = new();

            if (year != null)
            {
                foreach (var user in chat!.Users!)
                {
                    messageCountPerUser.Add(
                        key: user.From!,
                        value: user!.Messages!.Count(m => m.Date.Year == year));
                }
            }
            else
            {

                foreach (var user in chat!.Users!)
                {
                    messageCountPerUser.Add(
                        key: user.From!,
                        value: user!.Messages!.Count);
                }

            }

            return messageCountPerUser;
        }


        public Dictionary<int, int> GetMessageCountPerYear(Chat chat)
        {
            var messageCountPerYear = chat.Messages!
                .GroupBy(message => message.Date.Year)
                .ToDictionary(group => group.Key, group => group.Count());

            return messageCountPerYear;
        }


        public Dictionary<int, int> GetMessageCountPerMonth(Chat chat, int year)
        {
            var messageCountPerMonth = chat.Messages!
                .Where(message => message.Date.Year == year)
                .GroupBy(message => message.Date.Month)
                .ToDictionary(group => group.Key, group => group.Count());

            return messageCountPerMonth;
        }


        public Dictionary<int, int> GetAggregateMessageCountPerHour(Chat chat,
                                                           int? year = null,
                                                           int? month = null,
                                                           int? dayOfMonth = null)
        {
            IEnumerable<Message> filteredMessages = chat.Messages!;

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
                .GroupBy(message => message.Date.Hour)
                .OrderBy(message => message.Key)
                .ToDictionary(group => group.Key, group => group.Count());

            return messageCountPerHour;
        }


        public List<HourlyMessageCount> GetIndividualMessageCountPerHour(Chat chat,
                                                           int? year = null,
                                                           int? month = null,
                                                           int? dayOfMonth = null)
        {
            IEnumerable<Message> filteredMessages = chat.Messages!.Where(m => m.Type != "service");

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
                .GroupBy(message => message.Date.Hour)
                .OrderBy(message => message.Key)
                .Select(group => new HourlyMessageCount()
                {
                    Hour = group.Key,
                    UserMessageCount = group
                        .GroupBy(m => m.From!)
                        .OrderBy(group => group.Key)
                        .ToDictionary(group => group.Key, group => group.Count())
                })
                .ToList();

            return messageCountPerHour;
        }


        public Dictionary<string, int> GetTopActiveDates(Chat chat, int count)
        {
            var topActiveDays = chat.Messages
                    .GroupBy(m => m.Date.ToShortDateString())
                    .Select(g => new { Date = g.Key, MessageCount = g.Count() })
                    .OrderByDescending(x => x.MessageCount)
                    .Take(count)
                    .ToDictionary(x => x.Date, x => x.MessageCount);

            return topActiveDays;
        }
    }
}
