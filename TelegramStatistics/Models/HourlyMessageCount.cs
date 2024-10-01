namespace TelegramStatistics.Models
{
    public class HourlyMessageCount
    {
        public int Hour { get; set; }

        public Dictionary<string, int> UserMessageCount { get; set; } = new();
    }
}
