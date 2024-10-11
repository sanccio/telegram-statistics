using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.IO;
using System;
using System.Threading.Tasks;
using System.Threading;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using TelegramStatistics.Interfaces;

namespace TelegramStatistics.AvaloniaClient.ViewModels
{
    public partial class HomePageViewModel : ViewModelBase
    {
        private readonly IDeserializer _deserializer;
        private readonly IChatStatistics _chatStatistics;

        public HomePageViewModel(IDeserializer deserializer, IChatStatistics chatStatistics)
        {
            _deserializer = deserializer;
            _chatStatistics = chatStatistics;
        }

        [ObservableProperty] private static string? _fileChoosingOperationStatus;

        [ObservableProperty] private static string? _fileStatusIconColor;

        [ObservableProperty] private static StreamGeometry? _fileStatusIcon;

        [RelayCommand]
        private async Task OpenFile(CancellationToken token)
        {
            var file = await DoOpenFilePickerAsync();
            if (file is null) return;

            if (!Path.GetExtension(file.Path.LocalPath.ToString()).Equals(".json", StringComparison.OrdinalIgnoreCase))
            {
                SetFileChoosingOperationResult(
                    iconKey: "error_circle_regular",
                    iconColor: "Red",
                    message: "Wrong file format! Please choose a .json file.");

                return;
            }

            var chat = _deserializer.DeserializeFile(file.Path.LocalPath.ToString());
            _chatStatistics.SetChat(chat);

            SetFileChoosingOperationResult(
                    iconKey: "checkmark_regular",
                    iconColor: "Green",
                    message: file.Path.LocalPath);
        }


        private void SetFileChoosingOperationResult(string iconKey, string iconColor, string message)
        {
            Application.Current!.TryFindResource(iconKey, out var resource);
            FileStatusIcon = (StreamGeometry)resource!;
            FileStatusIconColor = iconColor;
            FileChoosingOperationStatus = message;
        }


        private async Task<IStorageFile?> DoOpenFilePickerAsync()
        {
            if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop ||
                desktop.MainWindow?.StorageProvider is not { } provider)
                throw new NullReferenceException("Missing StorageProvider instance.");

            var files = await provider.OpenFilePickerAsync(new FilePickerOpenOptions()
            {
                Title = "Open Json File",
                AllowMultiple = false
            });

            return files?.Count >= 1 ? files[0] : null;
        }
    }
}
