using Newtonsoft.Json;

namespace TelegramStatistics.Models
{
    public class TextEntity
    {
        [JsonProperty("type")]
        public string? Type { get; init; }

        [JsonProperty("text")]
        public string? Text { get; init; }
    }
}
