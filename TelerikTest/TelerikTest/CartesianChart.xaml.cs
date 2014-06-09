using System;
using System.Collections.Generic;
using System.ComponentModel;
using Telerik.Charting;
using Telerik.UI.Xaml.Controls.Chart;
using TelerikTest.Common;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 基本頁面項目範本已記錄在 http://go.microsoft.com/fwlink/?LinkId=234237

namespace TelerikTest
{
    /// <summary>
    /// 提供大部分應用程式共通特性的基本頁面。
    /// </summary>
    public sealed partial class CartesianChart : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private string[] months = { "January", "Feburary", "March", "April", "May", "June", "July", "August", "September", "Octorber", "November", "December" };
        private Random random = new Random();
        private LineSeries line1;
        private LineSeries line2;
        private CartesianDataSource barData1;
        private CartesianDataSource barData2;
        private ShowWhichBar showWhichBar;

        /// <summary>
        /// 此項可能變更為強類型檢視模型。
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// NavigationHelper 是用在每個頁面上協助巡覽及
        /// 處理程序生命週期管理
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        private LineSeries Line1
        {
            get
            {
                if (this.line1 == null)
                {
                    var source = this.CreateCartesianDataSource(ChartPalettes.DefaultLight.FillEntries.Brushes[9]);
                    this.line1 = this.CreateLine(source);
                }

                return this.line1;
            }
        }

        private LineSeries Line2
        {
            get
            {
                if (this.line2 == null)
                {
                    var source = this.CreateCartesianDataSource(ChartPalettes.DefaultLight.FillEntries.Brushes[8]);
                    this.line2 = this.CreateLine(source);
                }

                return this.line2;
            }
        }

        private CartesianDataSource BarData1
        {
            get
            {
                if (this.barData1 == null)
                {
                    this.barData1 = this.CreateCartesianDataSource(ChartPalettes.DefaultLight.FillEntries.Brushes[5]);
                }

                return this.barData1;
            }
        }

        private CartesianDataSource BarData2
        {
            get
            {
                if (this.barData2 == null)
                {
                    this.barData2 = this.CreateCartesianDataSource(ChartPalettes.DefaultLight.FillEntries.Brushes[7]);
                }

                return this.barData2;
            }
        }

        private BarSeries Bar1 { get; set; }

        private BarSeries Bar2 { get; set; }

        public CartesianChart()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;

            this.DrawLineCartesian();
            this.DrawBarCartesian();
        }

        private void DrawLineCartesian()
        {
            var brush = this.Resources["Brush1"] as Brush;

            var baseLineSource = new CartesianDataSource() { Data = new List<DataPointEntity>(), Stroke = brush };

            foreach (var item in this.months)
            {
                baseLineSource.Data.Add(new DataPointEntity() { Category = item, Value = 0 });
            }

            var baseLine = new LineSeries()
            {
                ItemsSource = baseLineSource.Data,
                Stroke = baseLineSource.Stroke,
                CategoryBinding = new PropertyNameDataPointBinding("Category"),
                ValueBinding = new PropertyNameDataPointBinding("Value"),
            };

            this.myLineCartesian.Series.Add(baseLine);

            this.myLineCartesian.Series.Add(this.Line1);
            this.myLineCartesian.Series.Add(this.Line2);

            this.stack1.DataContext = this.Line1;
            this.stack2.DataContext = this.Line2;
        }

        private void DrawBarCartesian()
        {
            this.showWhichBar = ShowWhichBar.None;

            var bar1 = new CartesianDataSource() { Data = new List<DataPointEntity>() };

            foreach (var item in this.months)
            {
                bar1.Data.Add(new DataPointEntity() { Category = item, Value = 0 });
            }

            this.Bar1 = this.CreateBar(bar1);

            var bar2 = new CartesianDataSource() { Data = new List<DataPointEntity>() };

            for (int i = 0; i < months.Length; i++)
            {
                bar2.Data.Add(new DataPointEntity() { Category = months[i], Value = this.BarData1.Data[i].Value + this.BarData2.Data[i].Value });
            }

            this.Bar2 = this.CreateBar(bar2);

            this.myBarCartesian.Series.Add(this.Bar1);
            this.myBarCartesian.Series.Add(this.Bar2);
        }

        private CartesianDataSource CreateCartesianDataSource(Brush stroke)
        {
            var source = new CartesianDataSource() { Data = new List<DataPointEntity>(), Stroke = stroke };

            foreach (var item in this.months)
            {
                source.Data.Add(new DataPointEntity() { Category = item, Value = this.random.Next(20, 50) });
            }

            return source;
        }

        private LineSeries CreateLine(CartesianDataSource source)
        {
            return new LineSeries()
            {
                ItemsSource = source.Data,
                Stroke = source.Stroke,
                CategoryBinding = new PropertyNameDataPointBinding("Category"),
                ValueBinding = new PropertyNameDataPointBinding("Value"),
            };
        }

        private BarSeries CreateBar(CartesianDataSource source)
        {
            return new BarSeries()
            {
                ItemsSource = source.Data,
                CombineMode = ChartSeriesCombineMode.Stack,
                CategoryBinding = new PropertyNameDataPointBinding("Category"),
                ValueBinding = new PropertyNameDataPointBinding("Value"),
            };
        }

        /// <summary>
        /// 巡覽期間以傳遞的內容填入頁面。從之前的工作階段
        /// 重新建立頁面時，也會提供儲存的狀態。
        /// </summary>
        /// <param name="sender">
        /// 事件之來源；通常是<see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">提供傳遞出去之巡覽參數之事件資料
        /// <see cref="Frame.Navigate(Type, Object)"/> 初始要求本頁面時及
        /// 這個頁面在先前的工作階段期間保留的狀態字典
        /// 工作階段。第一次瀏覽頁面時，狀態是 null。</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// 在應用程式暫停或從巡覽快取中捨棄頁面時，
        /// 保留與這個頁面關聯的狀態。值必須符合
        /// <see cref="SuspensionManager.SessionState"/> 的序列化需求。
        /// </summary>
        /// <param name="sender">事件之來源；通常是<see cref="NavigationHelper"/></param>
        /// <param name="e">事件資料，此資料提供即將以可序列化狀態填入的空白字典
        ///。</param>
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper 註冊

        /// 本區段中提供的方法只用來允許
        /// NavigationHelper 可回應頁面的巡覽方法。
        /// 
        /// 頁面專屬邏輯應該放在事件處理常式中
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// 和 <see cref="GridCS.Common.NavigationHelper.SaveState"/>。
        /// 巡覽參數可用於 LoadState 方法
        /// 除了先前的工作階段期間保留的頁面狀態。

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void CheckBox1_Checked(object sender, RoutedEventArgs e)
        {
            if (this.myLineCartesian != null)
            {
                this.myLineCartesian.Series.Add(this.Line1);
            }
        }

        private void CheckBox1_Unchecked(object sender, RoutedEventArgs e)
        {
            if (this.myLineCartesian != null)
            {
                this.myLineCartesian.Series.Remove(this.Line1);
            }
        }

        private void CheckBox2_Checked(object sender, RoutedEventArgs e)
        {
            if (this.myLineCartesian != null)
            {
                this.myLineCartesian.Series.Add(this.Line2);
            }
        }

        private void CheckBox2_Unchecked(object sender, RoutedEventArgs e)
        {
            if (this.myLineCartesian != null)
            {
                this.myLineCartesian.Series.Remove(this.Line2);
            }
        }

        private void Bar1_Click(object sender, RoutedEventArgs e)
        {
            this.myBarCartesian.Series.Remove(this.Bar1);
            this.myBarCartesian.Series.Remove(this.Bar2);

            if (this.showWhichBar == ShowWhichBar.Bar1)
            {
                this.DrawBarCartesian();
            }
            else
            {
                this.Bar1 = this.CreateBar(this.BarData1);
                this.Bar2 = this.CreateBar(this.BarData2);

                this.myBarCartesian.Series.Add(this.Bar1);
                this.myBarCartesian.Series.Add(this.Bar2);

                this.showWhichBar = ShowWhichBar.Bar1;
            }
        }

        private void Bar2_Click(object sender, RoutedEventArgs e)
        {
            this.myBarCartesian.Series.Remove(this.Bar1);
            this.myBarCartesian.Series.Remove(this.Bar2);

            if (this.showWhichBar == ShowWhichBar.Bar2)
            {
                this.DrawBarCartesian();
            }
            else
            {
                this.Bar1 = this.CreateBar(this.BarData2);
                this.Bar2 = this.CreateBar(this.BarData1);

                this.myBarCartesian.Series.Add(this.Bar1);
                this.myBarCartesian.Series.Add(this.Bar2);

                this.showWhichBar = ShowWhichBar.Bar2;
            }

        }
    }

    public class CartesianDataSource : INotifyPropertyChanged
    {
        private List<DataPointEntity> data;

        public event PropertyChangedEventHandler PropertyChanged;

        public List<DataPointEntity> Data
        {
            get
            {
                return this.data;
            }

            set
            {
                this.data = value;

                this.OnPropertyChanged("Data");
            }
        }

        public Brush Stroke { get; set; }

        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public class DataPointEntity
    {
        public string Category { get; set; }

        public double Value { get; set; }
    }

    public enum ShowWhichBar
    {
        None = 0,
        Bar1 = 1,
        Bar2 = 2,
    }
}
