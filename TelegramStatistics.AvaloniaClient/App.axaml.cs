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
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}