// See https://aka.ms/new-console-template for more information
using TelegramStatistics;
using TelegramStatistics.Interfaces;
using TelegramStatistics.Models;

string telegramChatFilePath = string.Empty;
string reportSavePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory));


bool isInputPathValid = false;
Console.WriteLine("Enter the file path. Example: C:\\Documents\\Telegram Desktop\\result.json\n");

while(!isInputPathValid)
{
    telegramChatFilePath = Console.ReadLine()!;

    if (File.Exists(telegramChatFilePath) && Path.GetExtension(telegramChatFilePath).Equals(".json", StringComparison.OrdinalIgnoreCase))
    {
        isInputPathValid = true;
        Console.Write("\nGreat! Your file report will be on the desktop. Press any key to continue...");
        Console.ReadKey();
    }
    else
    {
        Console.WriteLine("\nOops! The file path you entered either does not exist or does not point to a valid JSON file. Please check your input and try again.\n");
    }
}

Console.Clear();


IDeserializer deserializer = new JsonDeserializer();
Chat chat = await deserializer.DeserializeFile(telegramChatFilePath);
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
fileWriter.WriteFile(chat, reportSavePath, wordCounts, userWordCounts, userMessageCounts);

Console.Read();