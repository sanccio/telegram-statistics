using LiveChartsCore;
using System.Collections.Generic;
using System.Linq;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.VisualElements;
using System.Collections.ObjectModel;
using TelegramStatistics.AvaloniaClient.Models;
using LiveChartsCore.Drawing;
using System;
using System.Globalization;
using DynamicData.Aggregation;
using System.Xml.Serialization;

namespace TelegramStatistics.AvaloniaClient.ViewModels
{
    public partial class GeneralInfoViewModel : ViewModelBase
    {
        const int MaxPieCount = 3;

        private Dictionary<string, int> _messageCountPerSenderStats;

        private Dictionary<int, int> _messageCountPerYearStats;

        public ObservableCollection<ActiveDay> ActiveDays { get; set; }

        public string TotalMessageCount { get; set; }

        public List<ISeries> MessageCountPerSenderSeries { get; set; } = new();

        public ISeries[] YerlyActivitySeries { get; set; }

        public List<Axis> XAxes { get; set; }

        public List<Axis> YAxes { get; set; }


        public GeneralInfoViewModel()
        {
            TotalMessageCount = GetTotalMessageCountStats();
            
            _messageCountPerSenderStats = GetSendersMessageCount();
            SetMessageCountPerSenderSeries(_messageCountPerSenderStats, PieChartColorPalette.Colors);
            XAxes = SetXAxes();
            YAxes = SetYAxes();
            
            _messageCountPerYearStats = GetYearlyMessageStats();
            YerlyActivitySeries = CreateLineSeries();

            var activeDaysRange = GetTopActiveDatesStats(count: 7);
            ActiveDays = new ObservableCollection<ActiveDay>(activeDaysRange);
        }


        private static string GetTotalMessageCountStats()
        {
            return ChatModel.ChatStats.GetTotalMessageCount(ChatModel.Chat).ToString("n0");
        }


        private static List<ActiveDay> GetTopActiveDatesStats(int count)
        {
            var topActiveDates = ChatModel.ChatStats.GetTopActiveDates(ChatModel.Chat, count);

            var activeDays = new List<ActiveDay>();

            int placeNumber = 1;

            foreach (var activeDate in topActiveDates)
            {
                string placeNumberEmoji = GetPlaceNumberEmoji(placeNumber);
                string formattedDate = DateTime.Parse(activeDate.Key).ToString("dd MMM yyyy", CultureInfo.InvariantCulture);
                string messageCount = activeDate.Value.ToString() + " messages";

                ActiveDay activeDay = new(placeNumberEmoji, formattedDate, messageCount);
                activeDays.Add(activeDay);

                placeNumber++;
            }

            return activeDays;
        }


        private static string GetPlaceNumberEmoji(int placeNumber)
        {
            return (placeNumber) switch
            {
                1 => "\U0001f947",
                2 => "\U0001f948",
                3 => "\U0001f949",
                _ => ""
            };
        }


        private static Dictionary<int, int> GetYearlyMessageStats()
        {
            return ChatModel.ChatStats.GetMessageCountPerYear(ChatModel.Chat);
        }


        private static Dictionary<string, int> GetSendersMessageCount()
        {
            return ChatModel.ChatStats.GetMessageCountPerUser(ChatModel.Chat);
        }


        private static PieSeries<T> CreatePieSeries<T>(IEnumerable<T> pieValue, SKColor? pieColor, string chatSenderName)
        {
            var pieSeries = new PieSeries<T>
            {
                Name = "Messages",
                Values = pieValue,
                Pushout = 5,
                MaxRadialColumnWidth = 60,
                InnerRadius = 37,
                DataLabelsPaint = new SolidColorPaint(SKColors.Black),
                DataLabelsFormatter = point => point.Coordinate.PrimaryValue + " from " + $"{chatSenderName}",
                DataLabelsSize = 14,
            };

            if (pieColor.HasValue)
            {
                pieSeries.Fill = new SolidColorPaint(pieColor.Value);
            }

            return pieSeries;
        }


        private void SetMessageCountPerSenderSeries(Dictionary<string, int> sendersMessageCount, List<SKColor>? colors = null)
        {
            int colorIndex = 0;

            foreach (var senderStats in sendersMessageCount.Take(MaxPieCount))
            {
                var pieSeriesValue = new List<int> { senderStats.Value };

                var pieSeries = CreatePieSeries(pieValue: pieSeriesValue,
                                                pieColor: colors?[colorIndex],
                                                chatSenderName: senderStats.Key);

                MessageCountPerSenderSeries.Add(pieSeries);

                colorIndex++;
            }
        }


        private ISeries[] CreateLineSeries()
        {
            return new ISeries[]
            {
                new LineSeries<int>
                {
                    Name = "Messages",
                    Values = _messageCountPerYearStats.Values.ToArray(),
                    Fill = new SolidColorPaint(SKColors.Blue.WithAlpha(10)),
                }
            };
        }


        private static List<Axis> SetXAxes()
        {
            return new List<Axis>
            {
                new() {
                    Labels = GetXAxes(),
                    Padding = new Padding(10, 0, 0, 0),
                    TextSize = 12,
                }
            };
        }


        private static string[] GetXAxes()
        {
            return ChatModel.GetChatActiveYears()
                .Select(x => x.ToString())
                .ToArray();
        }


        private static List<Axis> SetYAxes()
        {
            return new List<Axis> 
            {
                new() {
                    Padding = new Padding(0,0,5,0),
                    TextSize = 12,
                }
            };
        }
    }


    public class ActiveDay
    {
        public string PlaceNumber { get; set; }
        public string Date { get; set; }
        public string MessageCount { get; set; }

        public ActiveDay(string placeNumber, string date, string messageCount)
        {
            PlaceNumber = placeNumber;
            Date = date;
            MessageCount = messageCount;
        }
    }
}
