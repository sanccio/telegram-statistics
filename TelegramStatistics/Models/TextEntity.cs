using System.Text.Json.Serialization;

namespace TelegramStatistics.Models
{
    public class TextEntity
    {
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("text")]
        public string? Text { get; set; }
    }
}
