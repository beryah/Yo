using System;
using System.Collections.Generic;
using TelerikTest.BLL;
using TelerikTest.Common;
using TelerikTest.DAL;
using TelerikTest.Entity.Basic;
using TelerikTest.Entity.Total;
using TelerikTest.Enum;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// 基本頁面項目範本已記錄在 http://go.microsoft.com/fwlink/?LinkId=234237

namespace TelerikTest
{
    /// <summary>
    /// 提供大部分應用程式共通特性的基本頁面。
    /// </summary>
    public sealed partial class Apparel_Total : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private ApparelTotal monthApparelTotal;
        private ApparelTotal seasonApparelTotal;
        private ApparelTotal yearApparelTotal;

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

        public ViewModel ViewModel
        {
            get
            {
                return this.Resources["ViewModel"] as ViewModel;
            }
        }

        private ApparelTotal MonthApparelTotal
        {
            get
            {
                if (this.monthApparelTotal == null)
                {
                    var rowDao = new MonthRowDao();
                    this.monthApparelTotal = new ApparelTotal(rowDao);
                }

                return this.monthApparelTotal;
            }
        }

        private ApparelTotal SeasonApparelTotal
        {
            get
            {
                if (this.seasonApparelTotal == null)
                {
                    var rowDao = new SeasonRowDao();
                    this.seasonApparelTotal = new ApparelTotal(rowDao);
                }

                return this.seasonApparelTotal;
            }
        }

        private ApparelTotal YearApparelTotal
        {
            get
            {
                if (this.yearApparelTotal == null)
                {
                    var rowDao = new YearRowDao();
                    this.yearApparelTotal = new ApparelTotal(rowDao);
                }

                return this.yearApparelTotal;
            }
        }

        private ApparelTotal ApparelTotal
        {
            get
            {
                var app = App.Current as App;

                switch (app.StatisticalInterval)
                {
                    case StatisticalInterval.Month:
                        return this.MonthApparelTotal;
                    case StatisticalInterval.Season:
                        return this.SeasonApparelTotal;
                    case StatisticalInterval.Year:
                        return this.YearApparelTotal;
                    default:
                        return this.MonthApparelTotal;
                }
            }
        }

        public Apparel_Total()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;

            this.MonthChecked.IsChecked = true;
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
        /// 
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
        ///
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

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            var app = App.Current as App;

            if (this.MonthChecked.IsChecked.Value)
            {
                app.StatisticalInterval = StatisticalInterval.Month;
            }
            else if (this.SeasonChecked.IsChecked.Value)
            {
                app.StatisticalInterval = StatisticalInterval.Season;
            }
            else if (this.YearChecked.IsChecked.Value)
            {
                app.StatisticalInterval = StatisticalInterval.Year;
            }

            this.SetBarChart();
        }

        private void SetBarChart()
        {
            this.ViewModel.NorthBarChart = this.ApparelTotal.GetSalesBySubLocation(SubLocation.North);

            this.ViewModel.CenterBarChart = this.ApparelTotal.GetSalesBySubLocation(SubLocation.Center);

            this.ViewModel.SouthBarChart = this.ApparelTotal.GetSalesBySubLocation(SubLocation.South);

            this.ViewModel.TotalBarChart = this.ApparelTotal.GetSalesAllLocation();
        }

        private void pageRoot_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize != e.PreviousSize)
            {
                var ratioX = e.NewSize.Width / 1366;
                var ratioY = e.NewSize.Height / 768;

                var defaultLocationStackSize = new LocationStackSize() { Height = 200, Width = 400, };
                var defaultTotalLocationMargin = new TransformMargin() { Left = 100, Top = 100, RatioX = ratioX, RatioY = ratioY };
                var defaultNorthLocationMargin = new TransformMargin() { Left = 900, Top = 30, RatioX = ratioX, RatioY = ratioY };
                var defaultCenterLocationMargin = new TransformMargin() { Left = 600, Top = 70, RatioX = ratioX, RatioY = ratioY };
                var defaultSouthLocationMargin = new TransformMargin() { Left = 350, Top = 170, RatioX = ratioX, RatioY = ratioY };

                this.ViewModel.LocationStackSize = defaultLocationStackSize;

                this.ViewModel.TotalLocationMargin = defaultTotalLocationMargin.Thickness;
                this.ViewModel.NorthLocationMargin = defaultNorthLocationMargin.Thickness;
                this.ViewModel.CenterLocationMargin = defaultCenterLocationMargin.Thickness;
                this.ViewModel.SouthLocationMargin = defaultSouthLocationMargin.Thickness;
            }
        }

        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            if (this.Frame != null)
            {
                var app = App.Current as App;
                app.selectedFlipViewIndex = 0;
                this.Frame.Navigate(typeof(Apparel_Location));
            }
        }

        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            if (this.Frame != null)
            {
                var app = App.Current as App;
                app.selectedFlipViewIndex = 1;
                this.Frame.Navigate(typeof(Apparel_Location));
            }
        }

        private void Button_Click3(object sender, RoutedEventArgs e)
        {
            if (this.Frame != null)
            {
                var app = App.Current as App;
                app.selectedFlipViewIndex = 2;
                this.Frame.Navigate(typeof(Apparel_Location));
            }
        }
    }
}
