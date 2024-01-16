using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using System.Text.Json;
using TelegramStatistics.AvaloniaClient.Models;
using TelegramStatistics.Models;

namespace TelegramStatistics.AvaloniaClient.Views
{
    public partial class HomePageView : UserControl
    {
        public HomePageView()
        {
            InitializeComponent();
        }

        //public async void UploadFile(object source, RoutedEventArgs args)
        //{
        //    var topLevel = TopLevel.GetTopLevel(this);

        //    var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        //    {
        //        Title = "Open result.json file",
        //        AllowMultiple = false
        //    });

        //    if (files.Count >= 1)
        //    {
        //        await using var stream = await files[0].OpenReadAsync();

        //        ChatModel.Chat = await JsonSerializer.DeserializeAsync<Chat>(stream);
        //    }
        //}
    }
}
