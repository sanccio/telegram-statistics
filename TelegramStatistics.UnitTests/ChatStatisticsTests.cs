using TelegramStatistics.Interfaces;
using TelegramStatistics.Models;

namespace TelegramStatistics.UnitTests
{
    public class ChatStatisticsTests
    {
        private IChatService _chatService;
        private ITextAnalyzer _textAnalyzer;
        private ChatStatistics _chatStatistics;
        private Chat _chat;

        [SetUp]
        public void Setup()
        {
            _chatService = new ChatService();
            _textAnalyzer = new TextAnalyzer();
            _chatStatistics = new ChatStatistics(_chatService, _textAnalyzer);
            _chat = JsonDeserializer.GetData(@"jsonTestFiles\test_data_2.json");
        }

        [Test]
        public void GetMessageCountOfEverySender_ReturnsUserMessagesCount_True()
        {
            _chatService.GroupAllMessagesBySender(_chat);
            var actualUsersMessageCounts = _chatStatistics.GetMessageCountPerUser(_chat);

            Dictionary<string, int> expectedUserMessageCounts = new()
            {
                { "Name_1", 2 },
                { "Name_2", 2 }
            };

            Assert.That(actualUsersMessageCounts, Is.EqualTo(expectedUserMessageCounts));
        }

    }
}
