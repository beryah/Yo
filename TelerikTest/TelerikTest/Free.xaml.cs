using TelerikTest.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Telerik.Charting;
using Telerik.UI.Xaml.Controls.Chart;
using System.Threading.Tasks;

// 基本頁面項目範本已記錄在 http://go.microsoft.com/fwlink/?LinkId=234237

namespace TelerikTest
{
    /// <summary>
    /// 提供大部分應用程式共通特性的基本頁面。
    /// </summary>
    public sealed partial class Free : Page
    {
        private string[] months = { "1月", "2月", "3月", "4月", "5月", "6月", "7月", "8月", "9月", "10月", "11月", "12月", };

        private Random random = new Random();

        public Free()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;

            TheirThings_1();
        }

        private FreeViewModel ViewModel
        {
            get
            {
                return this.Resources["FreeViewModel"] as FreeViewModel;
            }
        }

        private void TheirThings_1()
        {
            var chart = new BarChart();

            chart.Line1 = new List<Node>();
            chart.Line2 = new List<Node>();

            var myYears = new List<string>();

            for (int i = 0; i < 15; i++)
            {
                myYears.Add((2000 + i).ToString());
            }

            for (int i = 0; i < myYears.Count; i++)
            {
                foreach (var month in this.months)
                {
                    chart.Line1.Add(new Node() { Category = myYears[i] + "-" + month, Value = this.random.Next(15, 20) });
                    chart.Line2.Add(new Node() { Category = myYears[i] + "-" + month, Value = this.random.Next(15, 20) });
                }
            }

            chart.Nodes = chart.Line1.Count * 2;
            chart.To = chart.Nodes;
            chart.From = chart.Nodes - 6;
            chart.Zoom = new Size(chart.Line1.Count / 15, 1);
            chart.ScrollOffset = new Point(1 - chart.Zoom.Width, 0);

            myChart.DataContext = chart;
        }

        #region NavigationHelper 註冊

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

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

        private void ChartSelectionBehavior_SelectionChanged(object sender, EventArgs e)
        {
            var aa = sender as CategoricalDataPoint;
            var point = (myChart.Behaviors[0] as ChartSelectionBehavior).SelectedPoints.First().DataItem as Node;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var nodes = new List<Node>()
            {
                new Node(){ Category = "Date_1", Value = 5, },
                new Node(){ Category = "Date_2", Value = 6, },
                new Node(){ Category = "Date_3", Value = 4, },
                new Node(){ Category = "Date_4", Value = 2, },
                new Node(){ Category = "Date_5", Value = 6, },
            };

            this.ViewModel.Nodes = nodes;
            this.ViewModel.From = 0;
            this.ViewModel.To = 2;

            //this.Ring.IsActive = !this.Ring.IsActive;

            this.Ring.IsActive = true;
            this.AAA();
        }

        private async void AAA()
        {
            //var a = DateTime.Now;
            //while (DateTime.Now.Second - a.Second < 5) ;
            var client = new ApiClient();
            var asd = await client.PostAsync<string, string>("api/Values", "Yoo");
            
            this.ApiTest.Text = asd;
            this.ApiTest.Text = asd;
            this.ApiTest.Text = asd;
            this.ApiTest.Text = asd;

            this.Ring.IsActive = false;
        }
    }

    public class BarChart
    {
        public List<Node> Bar { get; set; }

        public List<Node> Line1 { get; set; }

        public List<Node> Line2 { get; set; }

        public int Nodes { get; set; }

        public Size Zoom { get; set; }

        public Point ScrollOffset { get; set; }

        public int From { get; set; }

        public int To { get; set; }
    }

    public class Node
    {
        public string Category { get; set; }
        public double Value { get; set; }
    }
}
