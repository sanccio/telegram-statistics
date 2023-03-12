namespace TelegramStatistics.Models
{
    public class UserWordCount
    {
        public string? UserName { get; set; }

        public List<WordCount> UserWordCounts { get; set; } = new List<WordCount>();
    }
}
