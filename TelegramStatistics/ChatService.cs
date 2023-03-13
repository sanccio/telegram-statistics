using System;
using TelegramStatistics.Interfaces;
using TelegramStatistics.Models;

namespace TelegramStatistics
{
    public class ChatService : IChatService
    {
        
        public ChatService(Chat chat)
        {
            GroupAllMessagesBySender(chat);
        }

        public ChatService()
        {

        }

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



        public Dictionary<string, List<Message>?> GetUsersMessages(List<User> users)
        {
            Dictionary<string, List<Message>?> usersMessages = users
                 .ToDictionary(x => x.From, x => x.Messages);

            return usersMessages;
        }


        
        public void GroupAllMessagesBySender(Chat chat)
        {
            IEnumerable<string?> senderNames = GetSendersNames(chat.Messages!);

            var groups = chat.Messages!
                .GroupBy(x => x.From);

            foreach (var name in senderNames)
            {
                var senderMessages = groups
                    .Where(x => x.Key == name)
                    .SelectMany(x => x)
                    .ToList();

                chat.Users!.Add(new User { From = name!, Messages = senderMessages });
            }
        }



        public IEnumerable<string?> GetSendersNames(List<Message> messages)
        {
            IEnumerable<string?> senderNames = new List<string>();

            senderNames = messages
                            .Select(m => m.From)
                            .Where(m => !String.IsNullOrEmpty(m))
                            .Distinct();

            return senderNames;
        }

    }
}
