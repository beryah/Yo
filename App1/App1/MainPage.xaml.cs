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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            double amt;

            if (double.TryParse(this.text.Text, out amt))
            {
                this.pie.DataContext = new PieData(amt);
            }

            var a = this.grid;
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

                    var bitmapImage = new BitmapImage(new Uri(@"C:\Users\Ejun\Pictures\XXX.png"));
                    //bitmapImage.SetSource(stream);

                    this.img.Source = bitmapImage;
                }
            }

            //await this.SetImageFromByteArray(bytes, this.img);
        }

        public async Task SetImageFromByteArray(byte[] data, Windows.UI.Xaml.Controls.Image image)
        {
            using (InMemoryRandomAccessStream raStream = new InMemoryRandomAccessStream())
            {
                using (DataWriter writer = new DataWriter(raStream))
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

                BitmapImage bitMapImage = new BitmapImage();
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
