using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace TelegramStatistics.Models
{
    public class Chat
    {
        [JsonProperty("name")]
        public string? Name { get; init; }

        [JsonProperty("type")]
        public string? Type { get; init; }

        [JsonProperty("messages")]
        public IReadOnlyList<Message> Messages { get; init; } = new List<Message>();

        public IReadOnlyList<User> Users { get; private set; } = default!;

        [OnDeserialized]
        private void AssignMessagesToUsers(StreamingContext context)
        {
            var users = new List<User>();

            var senderMessages = Messages
                .Where(m => !string.IsNullOrEmpty(m.From))
                .GroupBy(m => m.From);

            foreach (var group in senderMessages)
            {
                users.Add(new User { From = group.Key!, Messages = group.ToList() });
            }

            Users = users.AsReadOnly();
        }
    }
}
