// See https://aka.ms/new-console-template for more information
using TelegramStatistics;
using TelegramStatistics.Interfaces;
using TelegramStatistics.Models;

string fileReadingPath = @"C:\Users\sanch\source\repos\TelegramStatistics\TelegramStatistics.UnitTests\jsonTestFiles\test_data_2.json";

string fileWritingPath = @"C:\Users\sanch\Desktop";


IDeserializer deserializer = new JsonDeserializer();
Chat chat = await deserializer.DeserializeFile(fileReadingPath);
IChatService chatService = new ChatService(chat);
ITextAnalyzer textAnalyzer = new TextAnalyzer();
IChatStatistics chatStatistics = new ChatStatistics(chatService, textAnalyzer);


ConsoleOutput.PrintChatInfo(chat);


Dictionary<string, int> userMessageCounts = chatStatistics.GetMessageCountOfEverySender(chat);
ConsoleOutput.PrintUserMessagesNumber(userMessageCounts);


const int minimumWordFrequency = 1;
IEnumerable<WordCount> wordCounts = chatStatistics.GetWordsUsage(chat.Messages!, minimumWordFrequency);
ConsoleOutput.PrintTable(wordCounts/*.Take(10).ToList()*/); // Remove .Take()


IEnumerable<UserWordCount> userWordCounts = chatStatistics.GetWordsUsagePerUser(chat, minimumWordFrequency);
ConsoleOutput.PrintTable(userWordCounts);


TXTFileWriter fileWriter = new();
fileWriter.WriteFile(chat, fileWritingPath, wordCounts, userWordCounts, userMessageCounts);

Console.Read();