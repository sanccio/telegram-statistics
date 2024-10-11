using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using SkiaSharp;
using System;
using TelegramStatistics.AvaloniaClient.ViewModels;
using TelegramStatistics.AvaloniaClient.Views;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using TelegramStatistics.Interfaces;

namespace TelegramStatistics.AvaloniaClient
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);

            string fontFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Mulish-Medium.ttf");

            SKTypeface customTypeface = SKTypeface.FromFile(fontFilePath);

            LiveCharts.Configure(config =>
                config
                    .HasGlobalSKTypeface(customTypeface)
            );
        }

        public override void OnFrameworkInitializationCompleted()
        {
            var collection = new ServiceCollection();

            collection.AddSingleton<MainWindowViewModel>();
            collection.AddSingleton<HomePageViewModel>();
            collection.AddTransient<GeneralInfoViewModel>();
            collection.AddTransient<MonthlyStatsPageViewModel>();
            collection.AddTransient<HourlyStatsPageViewModel>();
            collection.AddTransient<WordFrequencyViewModel>();
            collection.AddSingleton<IDeserializer, JsonDeserializer>();
            collection.AddSingleton<IChatStatistics, ChatStatistics>();
            collection.AddSingleton<IChatService, ChatService>();
            collection.AddSingleton<ITextAnalyzer, TextAnalyzer>();

            var services = collection.BuildServiceProvider();
            var mainWindowViewModel = services.GetRequiredService<MainWindowViewModel>();

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = mainWindowViewModel,
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}