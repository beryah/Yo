using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
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

            var paletteEntryCollection = new PaletteEntryCollection();
            paletteEntryCollection.Brushes.AddRange(
                new List<SolidColorBrush>()
                {
                    new SolidColorBrush(Color.FromArgb(255, 198, 214, 165)),
                    new SolidColorBrush(Color.FromArgb(255, 206, 206, 198)),
                });

            this.pie.Palette = new ChartPalette()
            {
                Name = "CustomPalette",
                FillEntries = paletteEntryCollection,
                StrokeEntries = paletteEntryCollection,
            };

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
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            var renderTargetBitmap = new RenderTargetBitmap();
            await renderTargetBitmap.RenderAsync(this.cvs);

            var a = this.pie.MinHeight;
            var b = this.pie.MinWidth;

            var picker = new FileSavePicker();
            picker.FileTypeChoices.Add("PNG Image", new string[] { ".png" });
            var file = await picker.PickSaveFileAsync();
            if (file != null)
            {
                var pixels = await renderTargetBitmap.GetPixelsAsync();

                using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.ReadWrite))
                {
                    var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream);
                    var bytes = pixels.ToArray();
                    encoder.SetPixelData(BitmapPixelFormat.Bgra8,
                                         BitmapAlphaMode.Premultiplied,
                                         (uint)this.cvs.ActualWidth, (uint)this.cvs.ActualHeight,
                                         96, 96, bytes);

                    //var bitmapImage = new BitmapImage();
                    //await bitmapImage.SetSourceAsync(stream);
                    //this.img.Source = bitmapImage;

                    await encoder.FlushAsync();
                }
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
