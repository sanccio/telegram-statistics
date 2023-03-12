using NUnit.Framework.Internal;
using TelegramStatistics.Interfaces;

namespace TelegramStatistics.UnitTests
{
    internal class ChatServiceTests
    {
        private IChatService _chatService;
        private ITextAnalyzer _textAnalyzer;

        [SetUp]
        public void Setup()
        {
            _chatService = new ChatService();
            _textAnalyzer = new TextAnalyzer();
        }

        [Test]
        public void GetPlainTexts_ReturnsPlainTypeTexts_True()
        {
            List<string> expectedPlainTexts = new();

            var chat = JsonDeserializer
                .GetData(@"jsonTestFiles\test_data_1.json");

            var actualPlainTexts = _chatService.GetPlainTexts(chat.Messages!);

            expectedPlainTexts.AddRange(
                new string[]{
                    "Посмотрю потом",
                    "Комментарии в телеге на уровне",
                    "В АС тоже не очень таки молодёжные саундтреки"
                });

            Assert.That(actualPlainTexts, Is.EqualTo(expectedPlainTexts));

        }

        [Test]
        public void GetPlainTexts_ReturnsOnlyPlainTypeTexts_True()
        {
            List<string> expectedPlainTexts = new();

            var chat = JsonDeserializer
                .GetData(@"jsonTestFiles\test_data_2.json");

            var actualPlainTexts = _chatService.GetPlainTexts(chat.Messages!);

            expectedPlainTexts.AddRange(
                new string[]{
                    "Посмотрю потом",
                    "Комментарии в телеге на уровне",
                    "В АС тоже не очень таки молодёжные саундтреки",
                    "Поздравляю тебя с ",
                    " первым днём весны.\nБажаю гарно провести цей день! \n"
                });

            Assert.That(actualPlainTexts, Is.EqualTo(expectedPlainTexts));

        }

        [Test]
        public void CheckChatMessageCount_ReturnsTotalMessageCount_True()
        {
            var chat = JsonDeserializer
                .GetData(@"jsonTestFiles\test_data_2.json");

            var actualMessageCount = chat!.Messages!.Count;

            int expectedMessageCount = 4;

            Assert.That(actualMessageCount, Is.EqualTo(expectedMessageCount));

        }

        [Test]
        public void GroupAllMessagesBySender_ReturnsUserAndTheirMessagesCount_True()
        {
            var chat = JsonDeserializer
                .GetData(@"jsonTestFiles\test_data_2.json");

            _chatService.GroupAllMessagesBySender(chat);

            var actualUserCount = chat!.Users!.Count();
            var expectedUserCount = 2;

            int actualUser1MessageCount = chat!.Users!.Select(u => u!.Messages!.Select(m => m.From).Where(f => f == "Name_1")).Count();
            var expectedUser1MessageCount = 2;

            var actualUser2MessageCount = chat!.Users!.Select(u => u!.Messages!.Select(m => m.From).Where(f => f == "Name_2")).Count();
            var expectedUser2MessageCount = 2;
            
            Assert.Multiple(() =>
            {
                Assert.That(actualUserCount, Is.EqualTo(expectedUserCount));
                Assert.That(actualUser1MessageCount, Is.EqualTo(expectedUser1MessageCount));
                Assert.That(actualUser2MessageCount, Is.EqualTo(expectedUser2MessageCount));
            });
        }

    }
}