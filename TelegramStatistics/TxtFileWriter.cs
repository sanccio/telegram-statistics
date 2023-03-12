using System.Text;
using TelegramStatistics.Interfaces;
using TelegramStatistics.Models;

namespace TelegramStatistics
{
    public class TXTFileWriter : IFileWriter
    {
        public void WriteFile(Chat chat, string filePath, IEnumerable<WordCount> generalWordsUsage, IEnumerable<UserWordCount> usersWordCounts, Dictionary<string, int> numberOfMessagesByUsers)
        {
            string dateTime = DateTime.Now.ToString("dd-MM-HH-mm");
            string fileName = $@"\Telegram-stats-{dateTime}.txt";
            int delimiterLength = 45;

            using (StreamWriter writer = new(filePath + fileName, false, Encoding.UTF8))
            {
                WriteGeneralChatInfo(chat, delimiterLength, writer);

                WriteUsersMessagesStats(numberOfMessagesByUsers, delimiterLength, writer);

                WriteGeneralWordsUsageStats(generalWordsUsage, delimiterLength, writer);

                WriteWordsUsageStatsForEachUser(usersWordCounts, delimiterLength, writer);
            }
        }

        private static void WriteWordsUsageStatsForEachUser(IEnumerable<UserWordCount> result, int delimiterLength, StreamWriter writer)
        {
            foreach (var userWordCount in result)
            {
                writer.WriteLine("\n\n\n");
                writer.WriteLine($"Stats for {userWordCount.UserName} - {userWordCount.UserWordCounts.Count} records below:\n");

                writer.WriteLine($"|{" WORD",-31} |{" COUNT",-9} |");
                writer.WriteLine(new string('-', delimiterLength));

                foreach (var wordCount in userWordCount.UserWordCounts)
                {
                    writer.WriteLine($"| {wordCount.Text,-30} | {wordCount.Number,-8} |");
                    writer.WriteLine(new string('-', delimiterLength));
                }

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
