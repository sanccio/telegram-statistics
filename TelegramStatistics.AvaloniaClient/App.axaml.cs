using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using SkiaSharp;
using System.Drawing;
using TelegramStatistics.AvaloniaClient.ViewModels;
using TelegramStatistics.AvaloniaClient.Views;

namespace TelegramStatistics.AvaloniaClient
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);

            //Application.Current!.TryFindResource("PoppinsFont", out var resource);

            string fontFilePath = "C:\\Users\\sanch\\Desktop\\Mulish-Medium.ttf";

            SKTypeface customTypeface = SKTypeface.FromFile(fontFilePath);

            LiveCharts.Configure(config =>
                config
                    // Register your custom typeface
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