using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using System.Collections.Generic;
using TelegramStatistics.AvaloniaClient.Models;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Reactive.Linq;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.Globalization;
using System;
using Avalonia.Controls;
using LiveChartsCore.Drawing;

namespace TelegramStatistics.AvaloniaClient.ViewModels
{
    public partial class HourlyStatsPageViewModel : ViewModelBase
    {
        private Dictionary<int, int> _hourlyStats = new();

        public int[] ChatActiveYears { get; set; }

        public List<MessageCountPerHour> HourlyMessageCounts { get; set; }

        public List<Axis> XAxes { get; set; }

        public List<Axis> YAxes { get; set; }

        [ObservableProperty] private ISeries[] _series;
        

        [ObservableProperty] 
        private int _selectedYearCombobox;

        partial void OnSelectedYearComboboxChanged(int value)
        {
            int? month = ConvertMonthToNumeric(SelectedMonthCombobox?.Content?.ToString()!);

            _hourlyStats = GetHourlyStats(year: value, month: month);
            HourlyMessageCounts = MapHourlyMessageCounts();

            Series = SetSeries();
        }


        [ObservableProperty] 
        private ComboBoxItem? _selectedMonthCombobox;

        partial void OnSelectedMonthComboboxChanged(ComboBoxItem? value)
        {
            int? month = ConvertMonthToNumeric(value?.Content?.ToString()!);

            _hourlyStats = GetHourlyStats(SelectedYearCombobox, month);
            HourlyMessageCounts = MapHourlyMessageCounts();

            Series = SetSeries();
        }


        public HourlyStatsPageViewModel()
        {
            _hourlyStats = GetHourlyStats(SelectedYearCombobox);
            HourlyMessageCounts = MapHourlyMessageCounts();

            ChatActiveYears = ChatModel.GetChatActiveYears();
            SelectedYearCombobox = ChatActiveYears.FirstOrDefault();

            XAxes = SetXAxes();
            YAxes = SetYAxes();
            Series = SetSeries();
        }


        private static int? ConvertMonthToNumeric(string month)
        {
            if (string.IsNullOrEmpty(month))
            {
                return null;
            }

            bool canParse = DateTime.TryParseExact(month, "MMMM", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate);

            if (!canParse)
            {
                return null;
            }

            int convertedMonth = int.Parse(parsedDate.ToString("MM"));

            return convertedMonth;
        }


        private static Dictionary<int, int> GetHourlyStats(int? year, int? month = null)
        {
            return ChatModel.ChatStats.GetMessageCountPerHour(ChatModel.Chat, year, month);
        }


        private ISeries[] SetSeries()
        {
            var series = new ISeries[]
            {
                new ColumnSeries<int>
                {
                    Name = "Message count",
                    Values = HourlyMessageCounts.Select(x => x.MessageCount),
                    Padding = 3,
                    MaxBarWidth = 20,
                    Fill = new SolidColorPaint(new SKColor(146, 135, 121)),
                    //Fill = new SolidColorPaint(new SKColor(212, 186, 175)),
                }
            };

            return series;
        }


        private List<Axis> SetXAxes()
        {
            return new List<Axis>
            {
                new() 
                {
                    Labels = HourlyMessageCounts.Select(x => x.Hour.ToString()).ToList(),
                    TextSize = CommonChartOptions.FontSize,
                    Padding = new Padding(8,15,8,0),
                    LabelsRotation = -45
                }
            };
        }


        private static List<Axis> SetYAxes()
        {
            return new List<Axis>
            {
                new()
                {
                    MinLimit = 0,
                    TextSize = CommonChartOptions.FontSize,
                    Padding = new Padding(0,0,15,0),
                }
            };
        }


        private List<MessageCountPerHour> MapHourlyMessageCounts()
        {
            List<MessageCountPerHour> statistics = new();

            for (int hour = 0; hour < 24; hour++)
            {
                int messageCountForHour = _hourlyStats.TryGetValue(hour, out int value) ? value : 0;
                statistics.Add(new MessageCountPerHour { Hour = hour, MessageCount = messageCountForHour });
            }

            return statistics;
        }
    }


    public class MessageCountPerHour
    {
        public int MessageCount { get; set; }
        public int Hour { get; set; }
    }
}
