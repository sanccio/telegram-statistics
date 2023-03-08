// See https://aka.ms/new-console-template for more information
using TelegramStatistics;
using TelegramStatistics.Interfaces;
using TelegramStatistics.Models;

string path = @"C:\Users\sanch\source\repos\TelegramStatistics\TelegramStatistics.UnitTests\jsonTestFiles\test_data_2.json";


IFileParser fileParser = new FileParser();
Chat chat = await fileParser.DeserializeFile(path);
ChatAnalyzer _ = new(fileParser);
IEnumerable<string> messageTexts = fileParser.ExtractAllPlainTextsFromMessages(chat);
fileParser.GroupAllMessagesBySender(chat);
ConsoleOutput.PrintChatInfo(chat);


Dictionary<string, int> userStats = ChatAnalyzer.GetMessageCountOfEverySender(chat);
ConsoleOutput.PrintUserMessagesNumber(userStats);


IEnumerable<string> words = fileParser.SplitTextsIntoWords(messageTexts);
const int minimumWordFrequency = 1;
IEnumerable<WordCount> wordsWithNumber = ChatAnalyzer.CountWordUsage(words, minimumWordFrequency);
Console.WriteLine($"{wordsWithNumber.Count()} records below:\n");
ConsoleOutput.PrintTable(wordsWithNumber/*.Take(10).ToList()*/); // Remove .Take()



TXTFileWriter fileWriter = new();
fileWriter.WriteFile(chat, wordsWithNumber, userStats);

Console.Read();