using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System;
using System.Text.RegularExpressions;
using Avalonia.Media;
using Avalonia;
using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;

namespace TelegramStatistics.AvaloniaClient.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly IServiceProvider _serviceProvider;

        public MainWindowViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _currentPage = serviceProvider.GetRequiredService<HomePageViewModel>();
        }

        [ObservableProperty]
        private bool _isPaneOpen = true;

        [ObservableProperty]
        private ViewModelBase _currentPage;

        [ObservableProperty]
        private ListItemTemplate? _selectedListItem;

        partial void OnSelectedListItemChanged(ListItemTemplate? value)
        {
            if (value is null) return;
            var instance = _serviceProvider.GetService(value.ModelType);
            if (instance is null) return;
            CurrentPage = (ViewModelBase)instance;
        }

        public ObservableCollection<ListItemTemplate> Items { get; } = new()
        {
            new ListItemTemplate(typeof(HomePageViewModel), "home_regular"),
            new ListItemTemplate(typeof(GeneralInfoViewModel), "board_regular"),
            new ListItemTemplate(typeof(MonthlyStatsPageViewModel), "calendar_month_regular"),
            new ListItemTemplate(typeof(HourlyStatsPageViewModel), "clock_regular"),
            new ListItemTemplate(typeof(WordFrequencyViewModel), "text_word_count_regular"),
        };

        [RelayCommand]
        private void TriggerPane()
        {
            IsPaneOpen = !IsPaneOpen;
        }
    }


    public class ListItemTemplate
    {
        public ListItemTemplate(Type type, string iconKey)
        {
            ModelType = type;

            string sideBarTabName = type.Name.Replace("PageViewModel", "");
            string[] split = Regex.Split(sideBarTabName, @"(?<!^)(?=[A-Z])");

            if (split.Length > 1)
            {
                Label = split[0] + " " + split[1];
            }
            else 
            { 
                Label = sideBarTabName; 
            }

            Application.Current!.TryFindResource(iconKey, out var resource);
            ListItemIcon = (StreamGeometry)resource!;
        }

        public string Label { get; }
        public Type ModelType  { get; }
        public StreamGeometry ListItemIcon { get; }
    }
}
