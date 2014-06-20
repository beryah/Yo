using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Telerik.Charting;
using Telerik.UI.Xaml.Controls.Chart;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using System.Linq;

// 空白頁項目範本已記錄在 http://go.microsoft.com/fwlink/?LinkId=234238

namespace App1
{
    /// <summary>
    /// 可以在本身使用或巡覽至框架內的空白頁面。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            var data = new PieData(30);
            this.pie.DataContext = data;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            double amt;

            if (double.TryParse(this.text.Text, out amt))
            {
                await Task.Delay(300);

                this.pie.DataContext = new PieData(amt);
            }
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            var renderTargetBitmap = new RenderTargetBitmap();
            await renderTargetBitmap.RenderAsync(this.cvs);
            var bytes = (await renderTargetBitmap.GetPixelsAsync()).ToArray();

            var picker = new FileSavePicker();
            picker.FileTypeChoices.Add("PNG Image", new string[] { ".png" });

            var file = await picker.PickSaveFileAsync();
            if (file != null)
            {
                using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.ReadWrite))
                {
                    var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream);
                    encoder.SetPixelData(BitmapPixelFormat.Bgra8,
                                         BitmapAlphaMode.Premultiplied,
                                         (uint)this.cvs.ActualWidth, (uint)this.cvs.ActualHeight,
                                         96, 96, bytes);

                    await encoder.FlushAsync();

                    var bitMapImage = new BitmapImage();
                    stream.Seek(0);
                    bitMapImage.SetSource(stream);
                    this.img.Source = bitMapImage;
                }
            }
        }

        private async void SetImg_Click(object sender, RoutedEventArgs e)
        {
            var picker = new FileOpenPicker();
            picker.FileTypeFilter.Add(".png");
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".bmp");
            picker.FileTypeFilter.Add(".gif");

            var file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                //var image = new BitmapImage(new Uri(file.Path, UriKind.Absolute));
                //this.img.Source = image;

                using (var stream = await file.OpenReadAsync())
                {
                    using (var reader = new DataReader(stream))
                    {
                        var bytes = new byte[stream.Size];
                        await reader.LoadAsync((uint)stream.Size);
                        reader.ReadBytes(bytes);

                        var a = Convert.ToBase64String(bytes);
                        var l = a.Length;
                        var b = Convert.FromBase64String(a);

                        await this.SetImageFromByteArray(b, this.img);
                    }
                }
            }
        }

        private async Task SetImageFromByteArray(byte[] data, Image image)
        {
            using (var raStream = new InMemoryRandomAccessStream())
            {
                using (var writer = new DataWriter(raStream))
                {
                    // Write the bytes to the stream
                    writer.WriteBytes(data);

                    // Store the bytes to the MemoryStream
                    await writer.StoreAsync();

                    // Not necessary, but do it anyway
                    await writer.FlushAsync();

                    // Detach from the Memory stream so we don't close it
                    writer.DetachStream();
                }

                raStream.Seek(0);

                var bitMapImage = new BitmapImage();
                bitMapImage.SetSource(raStream);

                image.Source = bitMapImage;
            }
        }
    }

    public class PieData
    {
        public PieData(double amt)
        {
            this.ItemsSource = new List<Node>()
            {
                new Node() { Value = amt, IsSelected = true, },
                new Node() { Value = 100 - amt, IsSelected = false, },
            };

            var startAngle = 180 - amt / 2 / 100 * 360;

            this.AngleRange = new AngleRange(startAngle, 360);
        }

        public List<Node> ItemsSource { get; private set; }

        public AngleRange AngleRange { get; private set; }
    }

    public class Node
    {
        public double Value { get; set; }

        public bool IsSelected { get; set; }
    }
}
