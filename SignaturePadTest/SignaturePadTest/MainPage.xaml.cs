using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using System.Linq;
using System;
using System.Text;

// 空白頁項目範本已記錄在 http://go.microsoft.com/fwlink/?LinkId=234238

namespace SignaturePadTest
{
    /// <summary>
    /// 可以在本身使用或巡覽至框架內的空白頁面。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private uint? pointerId;
        private Point? previousPoint;
        private SolidColorBrush strokeBrush = new SolidColorBrush(Colors.Black);
        private int count = 0;

        public MainPage()
        {
            this.InitializeComponent();
            this.ViewModel.Signature.Width = this.MyCanvas.Width;
            this.ViewModel.Signature.Height = this.MyCanvas.Height;
        }

        private ViewModel ViewModel
        {
            get
            {
                return this.Resources["ViewModel"] as ViewModel;
            }
        }

        private void Canvas_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var currentPoint = e.GetCurrentPoint(this.MyCanvas);
            this.pointerId = currentPoint.PointerId;
            this.previousPoint = currentPoint.Position;
        }

        private void Canvas_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            this.pointerId = null;
            this.previousPoint = null;
        }

        private void Canvas_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            var currentPoint = e.GetCurrentPoint(this.MyCanvas);

            if (currentPoint.PointerId == this.pointerId)
            {
                this.count++;

                if (this.count == 2)
                {
                    this.count = 0;

                    var line = new Line()
                    {
                        X1 = this.previousPoint.Value.X,
                        Y1 = this.previousPoint.Value.Y,
                        X2 = currentPoint.Position.X,
                        Y2 = currentPoint.Position.Y,
                        Stroke = this.strokeBrush,
                    };

                    this.previousPoint = currentPoint.Position;
                    this.MyCanvas.Children.Add(line);
                }
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            this.MyCanvas.Children.Clear();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Func<UIElement, SignatureLine> getSignatureLine = x =>
            {
                var line = x as Line;
                return new SignatureLine()
                {
                    X1 = Math.Round(line.X1, 2),
                    Y1 = Math.Round(line.Y1, 2),
                    X2 = Math.Round(line.X2, 2),
                    Y2 = Math.Round(line.Y2, 2),
                };
            };

            this.ViewModel.Signature.Lines.Clear();

            this.ViewModel.Signature.Lines = this.MyCanvas.Children.Where(x => x is Line).Select(x => getSignatureLine(x)).ToList();

            var a = new DataContractJsonSerializer(typeof(Signature));
            using (var ms = new MemoryStream())
            {
                a.WriteObject(ms, this.ViewModel.Signature);
                ms.Position = 0;
                using (var tr = new StreamReader(ms, Encoding.UTF8) as TextReader)
                {
                    var jsonText = tr.ReadToEnd();
                }
            }
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            Func<SignatureLine, Canvas, Line> getZoomedLine = (signatureLine, canvas) =>
                {
                    var xRatio = canvas.Width / this.ViewModel.Signature.Width;
                    var yRatio = canvas.Height / this.ViewModel.Signature.Height;

                    return new Line()
                    {
                        X1 = signatureLine.X1 * xRatio,
                        Y1 = signatureLine.Y1 * yRatio,
                        X2 = signatureLine.X2 * xRatio,
                        Y2 = signatureLine.Y2 * yRatio,
                        Stroke = this.strokeBrush,
                    };
                };

            this.MyCanvas.Children.Clear();

            foreach (var item in this.ViewModel.Signature.Lines)
            {
                this.MyCanvas.Children.Add(getZoomedLine(item, this.MyCanvas));
                this.Canvas_1.Children.Add(getZoomedLine(item, this.Canvas_1));
                this.Canvas_2.Children.Add(getZoomedLine(item, this.Canvas_2));
            }
        }
    }
}
