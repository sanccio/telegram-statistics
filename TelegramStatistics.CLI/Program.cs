// See https://aka.ms/new-console-template for more information
using TelegramStatistics;
using TelegramStatistics.Interfaces;
using TelegramStatistics.Models;

string fileReadingPath = string.Empty;
string fileWritingPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory));


bool flag = false;
Console.WriteLine("Please, enter the file path. Example: C:\\Documents\\Telegram Desktop\\result.json\n");

while(!flag)
{

    fileReadingPath = Console.ReadLine()!;

    if (File.Exists(fileReadingPath))
    {
        flag = true;
        Console.WriteLine("\nGreat! Your file report will be on the desktop.\n");
        Thread.Sleep(4000);
    }
    else
    {
        Console.WriteLine("\nOops... the file not found! Check the input and try again!\n");
    }
}

Console.Clear();


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