using TelegramStatistics.Interfaces;
using TelegramStatistics.Models;

namespace TelegramStatistics.UnitTests
{
    internal class TextAnalyzerTests
    {
        private ITextAnalyzer _textAnalyzer;

        [SetUp]
        public void Setup()
        {
            _textAnalyzer = new TextAnalyzer();
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

            var actual = _textAnalyzer.SplitTextsIntoWords(texts);

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
        public void CountWordUsage_ReturnsWordCounts()
        {
            List<string> words = new() { "Весна", "Лето", "Весна", "Весна", "Зима", "Зима" };

            var actualWordUsage = _textAnalyzer!.CountWordUsage(words, 1);

            List<WordCount> expectedWordUsage = new()
            {
                new WordCount(){ Text = "Весна", Count = 3},
                new WordCount(){ Text = "Зима", Count = 2},
                new WordCount(){ Text = "Лето", Count = 1}
            };

            Assert.That(actualWordUsage.Select(x => x.Text), Is.EqualTo(expectedWordUsage.Select(x => x.Text)));
        }

    }
}
