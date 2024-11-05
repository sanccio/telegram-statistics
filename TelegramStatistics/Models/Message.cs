using Newtonsoft.Json;

namespace TelegramStatistics.Models
{
    public class Message
    {
        [JsonProperty("id")]
        public int Id { get; init; }

        [JsonProperty("type")]
        public string? Type { get; init; }

        [JsonProperty("date")]
        public DateTime Date { get; init; }

        [JsonProperty("from")]
        public string? From { get; init; }

        [JsonProperty("from_id")]
        public string? FromId { get; init; }
        
        [JsonProperty("media_type")]
        public string? MediaType { get; set; }

        [JsonProperty("forwarded_from")]
        public string? ForwardedFrom { get; init; }

        [JsonProperty("text")]
        public object? Text { get; init; }

        [JsonProperty("text_entities")]
        public IReadOnlyList<TextEntity>? TextEntities { get; init; } = new List<TextEntity>();
    }
}
