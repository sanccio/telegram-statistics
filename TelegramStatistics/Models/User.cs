namespace TelegramStatistics.Models
{
    public class User
    {
        public string From { get; set; } = "";

        public List<Message> Messages { get; set; } = new();
    }
}
