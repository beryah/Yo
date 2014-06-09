using System;
using System.Collections.Generic;
using System.ComponentModel;
using Telerik.UI.Xaml.Controls.Chart;
using TelerikTest.Common;
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
    public sealed partial class PieChart : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private PieChartSource pieChartSource = new PieChartSource();

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

        public PieChart()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;

            this.pieChartSource.Data = SampleData.Data1;
            this.myPieChart.Series[0].DataContext = this.pieChartSource;
            this.myItemControl.DataContext = this.pieChartSource;
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.pieChartSource.Data = SampleData.Data1;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.pieChartSource.Data = SampleData.Data2;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            this.pieChartSource.Data = SampleData.Data3;
        }
    }

    public class PieEntity
    {
        public string Label { get; set; }

        public double Value { get; set; }

        public Brush Brush { get; set; }
    }

    public class PieChartSource : INotifyPropertyChanged
    {
        private List<PieEntity> data;

        public event PropertyChangedEventHandler PropertyChanged;

        public List<PieEntity> Data
        {
            get
            {
                return this.data; 
            }

            set
            {
                this.data = value;
                for (int i = 0; i < this.data.Count; i++)
                {
                    this.data[i].Brush = ChartPalettes.DefaultLight.FillEntries.Brushes[i];
                }

                this.OnPropertyChanged("Data");
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            if(this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public static class SampleData
    {
        public static List<PieEntity> Data1 = new List<PieEntity>()
        {
            new PieEntity(){ Label = "Label 1", Value = 22 },
            new PieEntity(){ Label = "Label 2", Value = 39 },
            new PieEntity(){ Label = "Label 3", Value = 12 },
        };

        public static List<PieEntity> Data2 = new List<PieEntity>()
        {
            new PieEntity(){ Label = "Label 1", Value = 22 },
            new PieEntity(){ Label = "Label 2", Value = 29 },
            new PieEntity(){ Label = "Label 3", Value = 77 },
            new PieEntity(){ Label = "Label 4", Value = 40 },
        };

        public static List<PieEntity> Data3 = new List<PieEntity>()
        {
            new PieEntity(){ Label = "Label 1", Value = 82 },
            new PieEntity(){ Label = "Label 2", Value = 33 },
            new PieEntity(){ Label = "Label 3", Value = 34 },
            new PieEntity(){ Label = "Label 4", Value = 40 },
            new PieEntity(){ Label = "Label 5", Value = 91 },
        };
    }
}
