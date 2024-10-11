using System.Text.Json.Serialization;

namespace TelegramStatistics.Models
{
    public class Chat
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("messages")]
        public List<Message> Messages { get; set; } = new List<Message>();

        public List<User> Users { get; set; } = new List<User>();
    }
}
