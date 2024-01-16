using DynamicData;
using MicroCom.Runtime;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using TelegramStatistics.AvaloniaClient.Models;
using TelegramStatistics.Models;

namespace TelegramStatistics.AvaloniaClient.ViewModels
{
    public partial class WordFrequencyViewModel : ViewModelBase
    {
        const int MinWordFrequency = 10;
        const string UnknownUserName = "Unknown";

        public ObservableCollection<WordCount> GeneralWordUsage { get; set; } = new();

        public ObservableCollection<WordCount> FirstSenderWordUsage { get; set; } = new();

        public ObservableCollection<WordCount> SecondSenderWordUsage { get; set; } = new();

        public string FirstSenderName { get; set; } = string.Empty;

        public string SecondSenderName { get; set; } = string.Empty;


        public WordFrequencyViewModel()
        {
            SetGeneralWordStats();
            SetSenderStats(0, MinWordFrequency);
            SetSenderStats(1, MinWordFrequency);
        }


        private void SetGeneralWordStats(int? minWordFrequency = 1)
        {
            IEnumerable<WordCount> wordsUsage = ChatModel.ChatStats.GetWordsUsage(ChatModel.Chat.Messages, minWordFrequency);
            GeneralWordUsage = new ObservableCollection<WordCount>(wordsUsage);
        }


        private void SetSenderStats(int senderIndex, int? minWordFrequency = 1)
        {
            UserWordCount senderUserWordCount = ChatModel.ChatStats
                .GetWordsUsagePerUser(ChatModel.Chat, minWordFrequency)
                .ElementAtOrDefault(senderIndex) ?? new UserWordCount();

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
    }
}
