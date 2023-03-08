using System.Text.Json.Serialization;

namespace TelegramStatistics.Models
{
    public class Message
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("from")]
        public string? From { get; set; }

        [JsonPropertyName("from_id")]
        public string? FromId { get; set; }

        [JsonPropertyName("forwarded_from")]
        public string? ForwardedFrom { get; set; }

        [JsonPropertyName("text")]
        public object? Text { get; set; }

        [JsonPropertyName("text_entities")]
        public List<TextEntity>? TextEntities { get; set; } = new List<TextEntity>();
    }
}
