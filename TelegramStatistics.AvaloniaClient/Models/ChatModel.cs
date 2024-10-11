using TelegramStatistics.Interfaces;

namespace TelegramStatistics.AvaloniaClient.Models
{
    public static class ChatModel
    {
        public static IChatStatistics ChatStats { get; set; }

        public static IChatStatistics InitializeChatStatistics()
        {
            IChatService chatService = new ChatService();
            ITextAnalyzer textAnalyzer = new TextAnalyzer();

            return new ChatStatistics(chatService, textAnalyzer);
        }
    }
}
