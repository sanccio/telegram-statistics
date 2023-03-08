using TelegramStatistics.Models;

namespace TelegramStatistics
{
    public class ConsoleOutput
    {
        const int delimiterLength = 45;

        public static void PrintChatInfo(Chat? chat)
        {
            Console.WriteLine(new string('-', delimiterLength));
            Console.WriteLine("GENERAL INFORMATION ABOUT THE CHAT");
            Console.WriteLine(new string('-', delimiterLength));
            Console.WriteLine($"Chat type             : {chat?.Type?.Replace('_', ' ').ToLower()}");

            Console.WriteLine($"Total message count   : {chat?.Messages?.Count}");
            Console.WriteLine(new string('-', delimiterLength));
            Console.WriteLine("\n\n\n");

        }

        public static void PrintUserMessagesNumber(Dictionary<string, int> numberOfMessagesByUsers)
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("| {0, -19} | {1, -19} |", "FROM", "MESSAGE COUNT");
            Console.ResetColor();

            Console.WriteLine(new string('-', delimiterLength));

            foreach (var userStats in numberOfMessagesByUsers)
            {
                Console.WriteLine("| {0, -19} | {1,-19} |", userStats.Key, userStats.Value);
                Console.WriteLine(new string('-', delimiterLength));
            }
            Console.Write("\n\n\n\n");
        }

        public static void PrintTable(IEnumerable<WordCount> wordsWithNumber)
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("| {0,-30} | {1,-8} |", "WORD", "COUNT");

            Console.ResetColor();

            Console.WriteLine(new string('-', delimiterLength));

            foreach (var word in wordsWithNumber)
            {
                Console.WriteLine("| {0,-30} | {1,-8} |", word.Text, word.Number);
                Console.WriteLine(new string('-', delimiterLength));
            }

        }

    }
}
