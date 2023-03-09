using System.Text.Json;
using System.Text.RegularExpressions;
using TelegramStatistics.Interfaces;
using TelegramStatistics.Models;

namespace TelegramStatistics
{
    public class FileParser : IFileParser
    {

        public async Task<Chat> DeserializeFile(string path)
        {
            using FileStream openStream = File.OpenRead(path);

            var chat = await JsonSerializer.DeserializeAsync<Chat>(openStream);

            return chat!;
        }



        public IEnumerable<string> ExtractAllPlainTextsFromMessages(Chat? chat)
        {
            List<string> plainTexts = new();

            if (chat is null)
                return plainTexts;

            foreach (var message in chat!.Messages!)
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



        public IEnumerable<User> GroupAllMessagesBySender(Chat? chat)
        {
            List<User> users = new();

            if (chat is null)
                return users;

            IEnumerable<string?> senderNames = ExtractSenderNames(chat!);

            var groups = chat!.Messages!
                .GroupBy(x => x.From);

            foreach (var name in senderNames)
            {
                var senderMessages = groups
                    .Where(x => x.Key == name)
                    .SelectMany(x => x);

                users.Add(new User { From = name, Messages = senderMessages.ToList() });
            }

            chat.Users = users;

            return users;
        }



        public static IEnumerable<string?> ExtractSenderNames(Chat chat)
        {
            IEnumerable<string?> senderNames = new List<string>();

            if (chat is not null)
            {
                senderNames = chat!.Messages!
                                .Select(m => m.From)
                                .Where(m => !String.IsNullOrEmpty(m))
                                .Distinct();
            }

            return senderNames;
        }



        public IEnumerable<string> SplitTextsIntoWords(IEnumerable<string> plainTexts)
        {
            string pattern = @"[^\p{L}'0-9]|(?<!\w)'(?!\w)";

            return plainTexts
                .SelectMany(textLine => Regex.Split(ClearTextFromEmoji(textLine), pattern))
                .Where(word => !string.IsNullOrEmpty(word))
                .Select(word => word.ToLower());
        }



        public string ClearTextFromEmoji(string text)
        {
            return Regex.Replace(text, @"\p{Cs}", "");
        }

    }
}
