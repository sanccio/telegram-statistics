using SkiaSharp;
using System.Collections.Generic;

namespace TelegramStatistics.AvaloniaClient.Models
{
    public class PieChartColorPalette
    {
        public static List<SKColor> Colors { get; } = new()
        {
            new SKColor(206, 255, 26), // Lime
            new SKColor(85, 214, 190), // Turquoise
            new SKColor(185, 131, 137) // Old rose
        };
    }
}
