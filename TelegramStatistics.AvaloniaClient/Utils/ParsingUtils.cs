namespace TelegramStatistics.AvaloniaClient.Utils
{
    public class ParsingUtils
    {
        public static int? StringToNumeric(string str)
        {
            return int.TryParse(str, out int result) ? result : null;
        }
    }
}
