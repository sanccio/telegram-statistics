using DynamicData;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using TelegramStatistics.Interfaces;
using TelegramStatistics.Models;

namespace TelegramStatistics.AvaloniaClient.ViewModels
{
    public partial class WordFrequencyViewModel : ViewModelBase
    {
        const int MinWordFrequency = 10;
        const string UnknownUserName = "Unknown";

        private readonly IChatStatistics _chatStatistics;

        private IEnumerable<UserWordCount> _userWordCounts = new List<UserWordCount>();

        public ObservableCollection<WordCount> GeneralWordUsage { get; set; } = new();

        public ObservableCollection<WordCount> FirstSenderWordUsage { get; set; } = new();

        public ObservableCollection<WordCount> SecondSenderWordUsage { get; set; } = new();

        public string FirstSenderName { get; set; } = string.Empty;

        public string SecondSenderName { get; set; } = string.Empty;


        public WordFrequencyViewModel(IChatStatistics chatStatistics)
        {
            _chatStatistics = chatStatistics;

            SetGeneralWordStats();

            _userWordCounts = GetWordFrequencyPerUser(MinWordFrequency);
            SetSenderStats(0);
            SetSenderStats(1);
        }


        private void SetGeneralWordStats(int? minWordFrequency = 1)
        {
            IEnumerable<WordCount> wordsUsage = _chatStatistics.GetGeneralWordsUsage(minWordFrequency);
            GeneralWordUsage = new ObservableCollection<WordCount>(wordsUsage);
        }


        private void SetSenderStats(int senderIndex)
        {
            UserWordCount senderUserWordCount = _userWordCounts.ElementAtOrDefault(senderIndex) ?? new UserWordCount();

            if (senderIndex == 0)
            {
                FirstSenderWordUsage.AddRange(senderUserWordCount.UserWordCounts);
                FirstSenderName = senderUserWordCount.UserName ?? UnknownUserName;
            }
            else
            {
                SecondSenderWordUsage.AddRange(senderUserWordCount.UserWordCounts);
                SecondSenderName = senderUserWordCount.UserName ?? UnknownUserName;
            }
        }

        private IEnumerable<UserWordCount> GetWordFrequencyPerUser(int? minWordFrequency)
        {
            return _chatStatistics.GetWordsUsagePerUser(minWordFrequency);
        }
    }
}
