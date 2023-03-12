using NUnit.Framework.Internal;
using TelegramStatistics.Interfaces;

namespace TelegramStatistics.UnitTests
{
    public class FileParserTests
    {
        private IChatService _chatService;
        private Interfaces.ITextAnalyzer _wordService;

        [SetUp]
        public void Setup()
        {
            _chatService = new ChatService();
            _wordService = new TextAnalyzer();
        }

        [Test]
        public void SplitTextsIntoWords_ReturnsSeparateWords_True()
        {
            List<string> texts = new();
            texts.AddRange(
            new string[] {
                "Hey! It's me, random text. I'm here to help you test your regex pattern. " +
                "I can't wait to see how well it performs! Are you ready? Let's go! " +
                "1234567890 !@#$%^&*()_-+={}[]|:;\"\"'<>,.?/"
                });

            var actual = _wordService.SplitTextsIntoWords(texts);

            List<string> expected = new();
            expected.AddRange(
                new string[] {
                    "hey","it's","me","random","text","i'm","here",
                    "to","help","you","test","your","regex","pattern",
                    "i","can't","wait","to","see","how","well","it",
                    "performs","are","you","ready","let's","go", "1234567890"
                });


            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void ExtractAllPlainTextsFromMessages_ReturnsPlainTypeTexts_True()
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
        public void ExtractAllPlainTextsFromMessages_ReturnsOnlyPlainTypeTexts_True()
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
        public void DeserializeFile_ReturnsTotalMessageCount_True()
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