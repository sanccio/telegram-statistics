// See https://aka.ms/new-console-template for more information
using System.Text;
using TelegramStatistics;
using TelegramStatistics.Interfaces;
using TelegramStatistics.Models;

string telegramChatFilePath = string.Empty;
string reportSavePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory));

Console.OutputEncoding = Encoding.UTF8;

bool isInputPathValid = false;
Console.WriteLine("To get statistics, export your Telegram chat history in .json format without specifying any parameters.\n");
Console.WriteLine("Enter the file path. Example: C:\\Documents\\Telegram Desktop\\ChatExport_XXXX-XX-XX\\result.json\n");

while(!isInputPathValid)
{
    telegramChatFilePath = Console.ReadLine()!;

    if (File.Exists(telegramChatFilePath) && Path.GetExtension(telegramChatFilePath).Equals(".json", StringComparison.OrdinalIgnoreCase))
    {
        isInputPathValid = true;
        Console.Write("\nFile found!\n");
        //Console.ReadKey();
    }
    else
    {
        Console.WriteLine("\nOops! The file path you entered either does not exist or does not point to a valid JSON file. Please check your input and try again.\n");
    }
}


IDeserializer deserializer = new JsonDeserializer();
Chat chat = await deserializer.DeserializeFile(telegramChatFilePath);
IChatService chatService = new ChatService(chat);
ITextAnalyzer textAnalyzer = new TextAnalyzer();
IChatStatistics chatStatistics = new ChatStatistics(chatService, textAnalyzer);


Console.WriteLine("\nTo start analyzing the chat, press any key... This may take a moment.");
Console.ReadKey();


//ConsoleOutput.PrintChatInfo(chat);


Dictionary<string, int> userMessageCounts = chatStatistics.GetMessageCountOfEverySender(chat);
//ConsoleOutput.PrintUserMessagesNumber(userMessageCounts);


const int minimumWordFrequency = 1;
IEnumerable<WordCount> wordCounts = chatStatistics.GetWordsUsage(chat.Messages!, minimumWordFrequency);
//ConsoleOutput.PrintTable(wordCounts);


IEnumerable<UserWordCount> userWordCounts = chatStatistics.GetWordsUsagePerUser(chat, minimumWordFrequency);
//ConsoleOutput.PrintTable(userWordCounts);


TXTFileWriter fileWriter = new();
fileWriter.WriteFile(chat, reportSavePath, wordCounts, userWordCounts, userMessageCounts);

Console.WriteLine("\nDone! Your file report should now appear on the desktop. You can close the app.");
Console.Read();