using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramStatistics.Models;

namespace TelegramStatistics.Interfaces
{
    public interface IFileParser
    {
        public Task<Chat> DeserializeFile(string path);

        public IEnumerable<string> GetPlainTexts(IEnumerable<Message> messages);

        public Dictionary<string, List<Message>?> GetUsersMessages(Chat chat);

        public IEnumerable<User> GroupAllMessagesBySender(Chat? chat);

        public IEnumerable<string> SplitTextsIntoWords(IEnumerable<string> plainTexts);

        public string ClearTextFromEmoji(string text);
    }
}
