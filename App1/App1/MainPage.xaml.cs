using System;
using System.Collections.Generic;
using System.ComponentModel;
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

                    await encoder.FlushAsync();
                }
            }
        }
    }

    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Text
        {
            get
            {
                return "Hello!!";
            }
        }

        public Model Model
        {
            get
            {
                return new Model() { ModelText = "YYYYY" };
            }
        }

        public List<Model> Models
        {
            get
            {
                return new List<Model>()
                {
                    new Model()
                    {
                        ModelText="Model_1",
                        ModelA = new ModelInModel() { TextA = "1AA", TextB = "1AB", },
                        ModelB = new ModelInModel() { TextA = "1BA", TextB = "1BB", },
                        ModelC = new ModelInModel() { TextA = "1CA", TextB = "1CB", },
                    },
                    new Model()
                    {
                        ModelText="Model_2",
                        ModelA = new ModelInModel() { TextA = "2AA", TextB = "2AB", },
                        ModelB = new ModelInModel() { TextA = "2BA", TextB = "2BB", },
                        ModelC = new ModelInModel() { TextA = "3CA", TextB = "3CB", },
                    },
                    new Model()
                    {
                        ModelText="Model_3",
                        ModelA = new ModelInModel() { TextA = "3AA", TextB = "3AB", },
                        ModelB = new ModelInModel() { TextA = "2BA", TextB = "2BB", },
                        ModelC = new ModelInModel() { TextA = "3CA", TextB = "3CB", },
                    },
                };
            }
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public class Model
    {
        public string ModelText { get; set; }

        public ModelInModel ModelA { get; set; }

        public ModelInModel ModelB { get; set; }

        public ModelInModel ModelC { get; set; }
    }

    public class ModelInModel
    {
        public string TextA { get; set; }

        public string TextB { get; set; }
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
