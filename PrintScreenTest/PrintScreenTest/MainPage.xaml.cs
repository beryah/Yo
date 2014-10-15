using System;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

// 空白頁項目範本已記錄在 http://go.microsoft.com/fwlink/?LinkId=234238

namespace PrintScreenTest
{
    /// <summary>
    /// 可以在本身使用或巡覽至框架內的空白頁面。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var renderTargetBitmap = new RenderTargetBitmap();
            await renderTargetBitmap.RenderAsync(this.PrintScreenArea);
            var pixels = (await renderTargetBitmap.GetPixelsAsync()).ToArray();

            var picker = new FileSavePicker();
            picker.FileTypeChoices.Add("PNG Image", new string[] { ".png" });

            var file = await picker.PickSaveFileAsync();
            if (file != null)
            {
                using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
                {
                    var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream);
                    encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied, (uint)renderTargetBitmap.PixelWidth, (uint)renderTargetBitmap.PixelHeight, 96, 96, pixels);
                    await encoder.FlushAsync();
                }
            }
        }
    }
}