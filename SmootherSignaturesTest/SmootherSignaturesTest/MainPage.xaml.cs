using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// 空白頁項目範本已記錄在 http://go.microsoft.com/fwlink/?LinkId=234238

namespace SmootherSignaturesTest
{
    /// <summary>
    /// 可以在本身使用或巡覽至框架內的空白頁面。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly double velocityFilterWeight = 0.7;
        private readonly double maximumWidth = 10;
        private readonly double minimumWidth = 2;
        private readonly SolidColorBrush penColor = new SolidColorBrush(Colors.Black);

        public MainPage()
        {
            this.InitializeComponent();
            this.Points = new List<BezierCurvePoint>();
        }

        private uint? PointerId { get; set; }

        private List<BezierCurvePoint> Points { get; set; }

        private double LastVelocity { get; set; }

        private double LastWidth { get; set; }

        private void MyCanvas_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var p = e.GetCurrentPoint(this.MyCanvas);
            this.PointerId = p.PointerId;
            this.StrokeBegin(p);
        }

        private void MyCanvas_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            var p = e.GetCurrentPoint(this.MyCanvas);
            if (this.PointerId == p.PointerId)
            {
                this.StrokeUpdate(p);
            }
        }

        private void MyCanvas_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            this.PointerId = null;
            this.StrokeEnd();
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            this.MyCanvas.Children.Clear();
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            var renderTargetBitmap = new RenderTargetBitmap();
            await renderTargetBitmap.RenderAsync(this.MyCanvas);

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
                    //var a = Convert.ToBase64String(bytes);
                    //var b = Convert.FromBase64String(a);
                    encoder.SetPixelData(BitmapPixelFormat.Bgra8,
                                         BitmapAlphaMode.Premultiplied,
                                         (uint)this.MyCanvas.ActualWidth, (uint)this.MyCanvas.ActualHeight,
                                         96, 96, bytes);

                    await encoder.FlushAsync();
                }
            }
        }

        #region Stroke Control

        private void StrokeUpdate(PointerPoint pointerPoint)
        {
            var point = this.CreatePoint(pointerPoint);
            this.AddPoint(point);
        }

        private void StrokeBegin(PointerPoint pointerPoint)
        {
            this.Reset();
            this.StrokeUpdate(pointerPoint);
        }

        private void StrokeDraw(BezierCurvePoint point)
        {
            this.DrawPoint(point, this.LastWidth);
        }

        private void StrokeEnd()
        {
            if (this.Points.Count <= 2 && this.Points.FirstOrDefault() != null)
            {
                this.StrokeDraw(this.Points[0]);
            }
        }

        private void Reset()
        {
            this.Points.Clear();
            this.LastVelocity = 0;
            this.LastWidth = (this.maximumWidth + this.minimumWidth) / 2;
        }

        private BezierCurvePoint CreatePoint(PointerPoint pointerPoint)
        {
            return new BezierCurvePoint(pointerPoint.Position.X, pointerPoint.Position.Y);
        }

        private void AddPoint(BezierCurvePoint point)
        {
            this.Points.Add(point);

            if (this.Points.Count > 2)
            {
                if (this.Points.Count == 3)
                {
                    this.Points.Insert(0, this.Points[0]);
                }

                var startPoint = this.Points[1];
                var controlPoint1 = this.CalculateCurveControlPoints(new List<BezierCurvePoint>() { this.Points[0], this.Points[1], this.Points[2], })[1];
                var controlPoint2 = this.CalculateCurveControlPoints(new List<BezierCurvePoint>() { this.Points[1], this.Points[2], this.Points[3], })[0];
                var endPoint = this.Points[2];

                var curve = new BezierCurve(startPoint, controlPoint1, controlPoint2, endPoint);

                this.AddCurve(curve);

                this.Points.RemoveAt(0);
            }
        }

        private List<BezierCurvePoint> CalculateCurveControlPoints(List<BezierCurvePoint> points)
        {
            var dx1 = points[0].X - points[1].X;
            var dy1 = points[0].Y - points[1].Y;
            var dx2 = points[1].X - points[2].X;
            var dy2 = points[1].Y - points[2].Y;

            var m1 = new BezierCurvePoint((points[0].X + points[1].X) / 2, (points[0].Y + points[1].Y) / 2);
            var m2 = new BezierCurvePoint((points[1].X + points[2].X) / 2, (points[1].Y + points[2].Y) / 2);

            var l1 = Math.Sqrt(dx1 * dx1 + dy1 * dy1);
            var l2 = Math.Sqrt(dx2 * dx2 + dy2 * dy2);

            var dxm = m1.X - m2.X;
            var dym = m1.Y - m2.Y;

            var k = l2 / (l1 + l2);
            var cm = new BezierCurvePoint(m2.X + dxm * k, m2.Y + dym * k);

            var tx = points[1].X - cm.X;
            var ty = points[1].Y - cm.Y;

            return new List<BezierCurvePoint>()
            {
                new BezierCurvePoint(m1.X + tx, m1.Y + ty),
                new BezierCurvePoint(m2.X + tx, m2.Y + ty),
            };
        }

        private void AddCurve(BezierCurve curve)
        {
            var velocity = curve.EndPoint.VelocityFrom(curve.StartPoint);
            velocity = this.velocityFilterWeight * velocity + (1 - this.velocityFilterWeight) * this.LastVelocity;

            var newWidth = Math.Max(this.maximumWidth / (velocity + 1), this.minimumWidth);

            this.DrawCurve(curve, this.LastWidth, newWidth);

            this.LastVelocity = velocity;
            this.LastWidth = newWidth;
        }

        private void DrawCurve(BezierCurve curve, double startWidth, double endWidth)
        {
            var widthDelta = endWidth - startWidth;
            var drawSteps = Math.Floor(curve.Length);

            for (int i = 0; i < drawSteps; i++)
            {
                var t = i / drawSteps;
                var tt = t * t;
                var ttt = tt * t;

                var u = 1 - t;
                var uu = u * u;
                var uuu = uu * u;

                var x = curve.StartPoint.X * uuu + 3 * curve.ControlPoint1.X * uu * t + 3 * curve.ControlPoint2.X * u * tt + curve.EndPoint.X * ttt;
                var y = curve.StartPoint.Y * uuu + 3 * curve.ControlPoint1.Y * uu * t + 3 * curve.ControlPoint2.Y * u * tt + curve.EndPoint.Y * ttt;
                var point = new BezierCurvePoint(x, y);

                var width = startWidth + ttt * widthDelta;

                this.DrawPoint(point, width);
            }
        }

        private void DrawPoint(BezierCurvePoint point, double width)
        {
            this.MyCanvas.Children.Add(
                new Ellipse()
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Width = width,
                    Height = width,
                    Fill = this.penColor,
                    Stroke = this.penColor,
                    Margin = new Thickness(point.X, point.Y, 0, 0),
                });
        }

        #endregion
    }
}
