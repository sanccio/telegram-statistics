namespace TelegramStatistics.Models
{
    public class User
    {
        public string From { get; init; } = default!;

        public IReadOnlyList<Message> Messages { get; init; } = default!;
    }
}
