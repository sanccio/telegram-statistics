using TelegramStatistics.Interfaces;
using TelegramStatistics.Models;

namespace TelegramStatistics
{
    public class ChatService : IChatService
    {
        public IEnumerable<string> GetPlainTexts(IEnumerable<Message> messages)
        {
            List<string> plainTexts = new();

            foreach (var message in messages)
            {
                if (message.ForwardedFrom is not null)
                    continue;

                foreach (var textEntity in message.TextEntities!)
                {
                    if (textEntity.Type == "plain" && !string.IsNullOrEmpty(textEntity.Text))
                        plainTexts.Add(textEntity.Text);
                }
            }

            return plainTexts;
        }
    }
}