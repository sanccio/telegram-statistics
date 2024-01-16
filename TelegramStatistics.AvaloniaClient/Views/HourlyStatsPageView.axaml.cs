using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Diagnostics;

namespace TelegramStatistics.AvaloniaClient.Views
{
    public partial class HourlyStatsPageView : UserControl
    {
        public HourlyStatsPageView()
        {
            InitializeComponent();
        }

        private void OnYearComboBoxSelectionChanged(object source, SelectionChangedEventArgs e)
        {
            Debug.WriteLine($"The {e.AddedItems[0]} year is selected!");
        }
    }
}
