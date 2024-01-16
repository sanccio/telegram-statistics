using System.Text;
using TelegramStatistics;
using TelegramStatistics.Interfaces;
using TelegramStatistics.Models;

Console.OutputEncoding = Encoding.UTF8;

Console.WriteLine("To get statistics, export your Telegram chat history in .json format without specifying any parameters.\n");

IDeserializer deserializer = new JsonDeserializer();
//string telegramChatFilePath = RequestFilePathFromUser();
Chat chat = await deserializer.DeserializeFile("C:\\Users\\sanch\\Desktop\\ChatExport_2024-01-02\\result.json");

IChatService chatService = new ChatService(chat);
ITextAnalyzer textAnalyzer = new TextAnalyzer();
IChatStatistics chatStatistics = new ChatStatistics(chatService, textAnalyzer);

Console.WriteLine("\nPress any key to start analyzing the chat... This may take a moment.");
Console.ReadKey();

var userMessageCounts = chatStatistics.GetMessageCountPerUser(chat);
var wordCounts = chatStatistics.GetWordsUsage(chat.Messages!);
var userWordCounts = chatStatistics.GetWordsUsagePerUser(chat);

IFileWriter fileWriter = new TXTFileWriter();
string reportSavingPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory));
fileWriter.WriteFile(chat, reportSavingPath, wordCounts, userWordCounts, userMessageCounts);

Console.WriteLine("\nDone! Your file report should now appear on the desktop. You can close the app.");


static string RequestFilePathFromUser()
{
    Console.WriteLine("Enter the file path. Example: C:\\Documents\\Telegram Desktop\\ChatExport_XXXX-XX-XX\\result.json\n");

    string telegramChatFilePath = string.Empty;
    bool isInputPathValid = false;

    while (!isInputPathValid)
    {
        telegramChatFilePath = Console.ReadLine()!;

        if (File.Exists(telegramChatFilePath) && Path.GetExtension(telegramChatFilePath).Equals(".json", StringComparison.OrdinalIgnoreCase))
        {
            isInputPathValid = true;
            Console.WriteLine("\nFile found!");
        }
        else
        {
            Console.WriteLine("\nOops! The file path you entered either does not exist or does not point to a valid JSON file. Please check your input and try again.\n");
        }
    }

    return telegramChatFilePath;
}