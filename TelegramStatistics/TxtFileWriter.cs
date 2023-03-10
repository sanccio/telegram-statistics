using System.Text;
using TelegramStatistics.Interfaces;
using TelegramStatistics.Models;

namespace TelegramStatistics
{
    public class TXTFileWriter : IFileWriter
    {

        public void WriteFile(Chat? chat, IEnumerable<WordCount> result, Dictionary<string, int> numberOfMessagesByUsers)
        {
            string dateTime = DateTime.Now.ToString("dd-MM-HH-mm");
            string filePath = $@"C:\Users\sanch\Desktop\Telegram-stats-{dateTime}.txt";
            int delimiterLength = 45;

            using (StreamWriter writer = new(filePath, false, Encoding.UTF8))
            {
                WriteGeneralChatInfo(chat, delimiterLength, writer);

                WriteUsersMessagesStats(numberOfMessagesByUsers, delimiterLength, writer);

                WriteGeneralWordsUsageStats(result, delimiterLength, writer);

            }
        }

        private static void WriteGeneralWordsUsageStats(IEnumerable<WordCount> result, int delimiterLength, StreamWriter writer)
        {
            writer.WriteLine($"General words usage stats - {result.Count()} records below:\n");
            writer.WriteLine("| {0,-30} | {1,-8} |", "WORD", "COUNT");
            writer.WriteLine(new string('-', delimiterLength));

            foreach (var word in result)
            {
                writer.WriteLine("| {0,-30} | {1,-8} |", word.Text, word.Number);
                writer.WriteLine(new string('-', delimiterLength));
            }
        }

        private static void WriteUsersMessagesStats(Dictionary<string, int> numberOfMessagesByUsers, int delimiterLength, StreamWriter writer)
        {
            writer.WriteLine("| {0, -19} | {1, -19} |", "FROM", "MESSAGE COUNT");
            writer.WriteLine(new string('-', delimiterLength));

            foreach (var userStats in numberOfMessagesByUsers)
            {
                writer.WriteLine("| {0, -19} | {1,-19} |", userStats.Key, userStats.Value);
                writer.WriteLine(new string('-', delimiterLength));
            }
            writer.WriteLine("\n\n\n");
        }

        private static void WriteGeneralChatInfo(Chat? chat, int delimiterLength, StreamWriter writer)
        {
            writer.WriteLine(new string('-', delimiterLength));
            writer.WriteLine("GENERAL INFORMATION ABOUT THE CHAT");
            writer.WriteLine(new string('-', delimiterLength));
            writer.WriteLine($"Chat type             : {chat?.Type?.Replace('_', ' ').ToLower()}");

            writer.WriteLine($"Total message count   : {chat?.Messages?.Count}");
            writer.WriteLine(new string('-', delimiterLength));
            writer.WriteLine("\n\n\n");
        }
    }
}
