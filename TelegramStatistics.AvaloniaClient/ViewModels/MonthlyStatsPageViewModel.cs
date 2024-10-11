using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using TelegramStatistics.AvaloniaClient.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Diagnostics;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using LiveChartsCore.Drawing;

namespace TelegramStatistics.AvaloniaClient.ViewModels
{
    public partial class MonthlyStatsPageViewModel : ViewModelBase
    {
        private Dictionary<int, int> _messageCountPerMonthStats;

        public int[] ChatActiveYears { get; set; }

        [ObservableProperty] private ISeries[] _series;

        [ObservableProperty] private List<Axis> _xAxes;

        [ObservableProperty] private List<Axis> _yAxes;

        [ObservableProperty] private int _selectedYearCombobox;

        partial void OnSelectedYearComboboxChanged(int value)
        {
            _messageCountPerMonthStats = GetMonthlyStats(year: value);

            XAxes = SetXAxes();
            Series = SetSeries();
        }


        public MonthlyStatsPageViewModel()
        {
            _messageCountPerMonthStats = GetMonthlyStats(SelectedYearCombobox);

            ChatActiveYears = ChatModel.ChatStats.GetChatActiveYears();
            SelectedYearCombobox = ChatActiveYears.FirstOrDefault();

            XAxes = SetXAxes();
            YAxes = SetYAxes();
            Series = SetSeries();
        }


        private static Dictionary<int, int> GetMonthlyStats(int year)
        {
            return ChatModel.ChatStats.GetMessageCountPerMonth(year);
        }


        private ISeries[] SetSeries()
        {
            return new ISeries[]
            {
                new LineSeries<int>
                {
                    Name = "Message count",
                    Values = GetSeries(),
                    Fill = new LinearGradientPaint(
                        new [] { 
                            SKColors.Coral.WithAlpha(100), 
                            SKColors.Coral.WithAlpha(60), 
                            SKColors.Coral.WithAlpha(20), 
                            SKColors.Coral.WithAlpha(5) 
                        },
                        new SKPoint(0.5f, 0),
                        new SKPoint(0.5f, 1)),
                    Stroke = new SolidColorPaint(SKColors.Coral) { StrokeThickness = 4 },
                    GeometryStroke = new SolidColorPaint(SKColors.Coral) { StrokeThickness = 4 },
                    LineSmoothness = 0.65,
                }
            };
        }


        private List<Axis> SetXAxes()
        {
            return new List<Axis>
            {
                new() {
                    Labels = GetXAxesLabels(),
                    LabelsRotation = 125,
                    TextSize = CommonChartOptions.FontSize,
                }
            };
        }


        private static List<Axis> SetYAxes()
        {
            return new List<Axis>
            {
                new() {
                    MinLimit = 0,
                    Padding = new Padding(0, 0, 15, 0),
                    MinStep = 1,
                    TextSize = CommonChartOptions.FontSize,
                }
            };
        }


        private int[] GetSeries()
        {
            return _messageCountPerMonthStats.Values.ToArray();
        }


        private string[] GetXAxesLabels()
        {
            var xAxesLabels = _messageCountPerMonthStats.Keys
                .Select(monthNumeric => monthNumeric.ToString(CultureInfo.InvariantCulture.DateTimeFormat.GetMonthName(monthNumeric)))
                .ToArray();

            return xAxesLabels;
        }
    }
}
