using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reactive.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using DynamicData;
using LiveChartsCore;
using LiveChartsCore.Drawing;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using TelegramStatistics.AvaloniaClient.Models;
using TelegramStatistics.Interfaces;
using TelegramStatistics.Models;

namespace TelegramStatistics.AvaloniaClient.ViewModels
{
    public partial class HourlyStatsPageViewModel : ViewModelBase
    {
        private const string DefaultSelectedMonthValue = "All months";

        private readonly IChatStatistics _chatStatistics;
        private IReadOnlyList<HourlyMessageCount> _hourlyStats = new List<HourlyMessageCount>();

        [ObservableProperty] private IReadOnlyList<string> _availableChatMonthsWithinYear = new List<string>();
        [ObservableProperty] private IReadOnlyList<ISeries> _series = new List<ISeries>();
        [ObservableProperty] private int _selectedYearCombobox;
        [ObservableProperty] private string _selectedMonthCombobox;

        public int[] AvailableChatYears { get; set; }
        public IReadOnlyList<MessageCountPerHour> HourlyMessageCounts { get; set; } = new List<MessageCountPerHour>();
        public IReadOnlyList<Axis> XAxes { get; set; }
        public IReadOnlyList<Axis> YAxes { get; set; }


        public HourlyStatsPageViewModel(IChatStatistics chatStatistics)
        {
            _chatStatistics = chatStatistics;

            SelectedMonthCombobox = DefaultSelectedMonthValue;

            AvailableChatYears = _chatStatistics.GetChatActiveYears()
                .OrderByDescending(y => y)
                .ToArray();

            SelectedYearCombobox = AvailableChatYears.FirstOrDefault();

            XAxes = SetXAxes();
            YAxes = SetYAxes();
        }


        partial void OnSelectedYearComboboxChanged(int value)
        {
            UpdateMonthsCombobox(value);
            UpdateStats(value, SelectedMonthCombobox);
        }


        partial void OnSelectedMonthComboboxChanged(string value)
        {
            if (SelectedYearCombobox != 0 && value is not null)
            {
                UpdateStats(SelectedYearCombobox, SelectedMonthCombobox);
            }
        }


        private void UpdateStats(int year, string month)
        {
            int? numericMonth = ConvertMonthToNumeric(month);

            _hourlyStats = GetHourlyStats(year, numericMonth);
            HourlyMessageCounts = MapHourlyMessageCounts();

            Series = SetSeries();
        }


        private void UpdateMonthsCombobox(int year)
        {
            var months = _chatStatistics
                .GetAvailableMonthsWithinYear(year)
                .Select(y => new DateTimeFormatInfo().GetMonthName(y))
                .ToList();

            AvailableChatMonthsWithinYear = new List<string>() { DefaultSelectedMonthValue, months };

            SelectedMonthCombobox = SelectedMonthCombobox is null ? DefaultSelectedMonthValue : SelectedMonthCombobox;
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


        private List<HourlyMessageCount> GetHourlyStats(int? year, int? month = null)
        {
            return _chatStatistics.GetIndividualMessageCountPerHour(year, month);
        }


        private IReadOnlyList<ISeries> SetSeries()
        {
            string[] chatParticipants = HourlyMessageCounts
                .SelectMany(x => x.UserMessageCount.Keys)
                .Distinct()
                .ToArray();

            var series = new List<ISeries>();
            int colorIndexFromEnd = 1;

            foreach (string participant in chatParticipants)
            {
                IEnumerable<int> participantMessageCounts = HourlyMessageCounts
                    .Select(x => x.UserMessageCount.GetValueOrDefault(participant));

                var columnSeries = new ColumnSeries<int>()
                {
                    Name = participant,
                    Values = participantMessageCounts,
                    MaxBarWidth = 12,
                    Padding = 3,
                    Fill = new SolidColorPaint(PieChartColorPalette.Colors[^colorIndexFromEnd++]),
                };

                series.Add(columnSeries);
            }

            return series;
        }


        private IReadOnlyList<Axis> SetXAxes()
        {
            return new List<Axis>
            {
                new()
                {
                    Labels = HourlyMessageCounts.Select(x => x.Hour.ToString()).ToList(),
                    TextSize = CommonChartOptions.FontSize,
                    Padding = new Padding(8, 15, 8, 0),
                    LabelsRotation = -45,
                    TicksPaint = new SolidColorPaint(new SKColor(35, 35, 35)),
                    TicksAtCenter = true,
                    ShowSeparatorLines = false,
                }
            };
        }


        private static IReadOnlyList<Axis> SetYAxes()
        {
            return new List<Axis>
            {
                new()
                {
                    MinLimit = 0,
                    TextSize = CommonChartOptions.FontSize,
                    Padding = new Padding(0, 0, 15, 0),
                    MinStep = 1,
                }
            };
        }


        private IReadOnlyList<MessageCountPerHour> MapHourlyMessageCounts()
        {
            List<MessageCountPerHour> statistics = new(
                Enumerable.Range(0, 24).Select(hour => new MessageCountPerHour { Hour = hour })
            );

            foreach (HourlyMessageCount item in _hourlyStats)
            {
                statistics[item.Hour].UserMessageCount = item.UserMessageCount;
            }

            return statistics;
        }
    }


    public class MessageCountPerHour
    {
        public int Hour { get; set; }
        public Dictionary<string, int> UserMessageCount { get; set; } = new();
    }
}
