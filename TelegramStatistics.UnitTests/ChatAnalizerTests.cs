using TelegramStatistics.Interfaces;
using TelegramStatistics.Models;

namespace TelegramStatistics.UnitTests
{
    internal class ChatAnalizerTests
    {
        private static IFileParser _fileParser;
        private static ChatAnalyzer _chatAnalyzer;

        public Chat _chat;

        [SetUp]
        public void Setup()
        {
            _fileParser = new FileParser();
            _chatAnalyzer = new ChatAnalyzer(_fileParser);
            _chat = JsonDeserializer.GetData(@"jsonTestFiles\test_data_2.json");
        }

        [Test]
        public void GetMessageCountOfEverySender_ReturnsUserMessagesCount_True()
        {
            var actualUsersMessageCounts = ChatAnalyzer.GetMessageCountOfEverySender(_chat);

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

            var actualWordUsage = ChatAnalyzer.CountWordUsage(words, 1);

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
