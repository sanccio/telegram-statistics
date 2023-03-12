using TelegramStatistics.Interfaces;
using TelegramStatistics.Models;

namespace TelegramStatistics.UnitTests
{
    internal class ChatAnalizerTests
    {
        private static IChatService _chatService;
        private static ITextAnalyzer _textAnalyzer;
        private static ChatStatistics _chatStatistics;
        private Chat _chat;

        [SetUp]
        public void Setup()
        {
            _chatService = new ChatService();
            _textAnalyzer = new TextAnalyzer();
            _chatStatistics = new ChatStatistics(_chatService, _textAnalyzer!);
            _chat = JsonDeserializer.GetData(@"jsonTestFiles\test_data_2.json");
        }

        [Test]
        public void GetMessageCountOfEverySender_ReturnsUserMessagesCount_True()
        {
            _chatService.GroupAllMessagesBySender(_chat);
            var actualUsersMessageCounts = ChatStatistics.GetMessageCountOfEverySender(_chat);

            Dictionary<string, int> expectedUserMessageCounts = new()
            {
                { "Name_1", 2 },
                { "Name_2", 2 }
            };

            Assert.That(actualUsersMessageCounts, Is.EqualTo(expectedUserMessageCounts));

        }

        [Test]
        public void CountWordUsage_ReturnsWordCounts()
        {
            List<string> words = new() { "Весна", "Лето", "Весна", "Весна", "Зима", "Зима" };

            var actualWordUsage = _textAnalyzer!.CountWordUsage(words, 1);

            List<WordCount> expectedWordUsage = new()
            {
                new WordCount(){ Text = "Весна", Number = 3},
                new WordCount(){ Text = "Зима", Number = 2},
                new WordCount(){ Text = "Лето", Number = 1}
            };

            Assert.That(actualWordUsage.Select(x => x.Text), Is.EqualTo(expectedWordUsage.Select(x => x.Text)));
        }

    }
}
