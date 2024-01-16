using System.IO;
using System.Linq;
using System.Text.Json;
using TelegramStatistics.Interfaces;
using TelegramStatistics.Models;

namespace TelegramStatistics.AvaloniaClient.Models
{
    //public static class ChatModel
    //{
    //    public static Chat Chat { get; set; }
    //    public static IChatStatistics ChatStats { get; set; }

    //    static ChatModel()
    //    {
    //        Chat = DeserializeChat();
    //        //Chat = new Chat();

    //        ChatStats = InitializeChatStatistics(Chat);
    //    }

    //    public static Chat DeserializeChat(string path = "C:\\Users\\sanch\\Desktop\\ChatExport_2024-01-03\\result.json")
    //    {
    //        using FileStream openStream = File.OpenRead(path);

    //        return JsonSerializer.Deserialize<Chat>(openStream)!;
    //    }

    //    public static IChatStatistics InitializeChatStatistics(Chat chat)
    //    {
    //        IChatService chatService = new ChatService(chat);
    //        ITextAnalyzer textAnalyzer = new TextAnalyzer();

    //        return new ChatStatistics(chatService, textAnalyzer);
    //    }
    //}

    public class ChatModel
    {
        public static Chat? Chat { get; set; }

        public static string? FilePath { get; set; }

        public static IChatStatistics ChatStats { get; set; }

        public ChatModel(Chat chat)
        {
            Chat = chat;
        }

        public static Chat DeserializeChat(string path)
        {
            FilePath = path;

            using FileStream openStream = File.OpenRead(path);
            var chat = JsonSerializer.Deserialize<Chat>(openStream)!;

            ChatStats = InitializeChatStatistics(chat);

            return chat;
        }

        public static IChatStatistics InitializeChatStatistics(Chat chat)
        {
            IChatService chatService = new ChatService(chat);
            ITextAnalyzer textAnalyzer = new TextAnalyzer();

            return new ChatStatistics(chatService, textAnalyzer);
        }

        public static int[] GetChatActiveYears()
        {
            var activeYears = Chat.Messages!
                .Select(x => x.Date.Year)
                .Distinct()
                .ToArray();

            return activeYears;
        }
    }
}
