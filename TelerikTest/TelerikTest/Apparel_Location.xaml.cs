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
using TelerikTest.Entity.Location;
using TelerikTest.Entity.Basic;
using TelerikTest.BLL;
using TelerikTest.Enum;
using Telerik.UI.Xaml.Controls.Chart;
using TelerikTest.DAL;
using Telerik.Charting;
using Windows.UI;
using Windows.UI.Xaml.Shapes;
using Telerik.Core;
using MyPieDataPoint = TelerikTest.Entity.Basic.PieDataPoint;
using Windows.UI.Popups;

// 基本頁面項目範本已記錄在 http://go.microsoft.com/fwlink/?LinkId=234237

namespace TelerikTest
{
    /// <summary>
    /// 提供大部分應用程式共通特性的基本頁面。
    /// </summary>
    public sealed partial class Apparel_Location : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private Random random = new Random();
        private ApparelLocation apparelLocation;

        #region properties

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

        private ViewModel ViewModel
        {
            get
            {
                return this.Resources["ViewModel"] as ViewModel;
            }
        }

        private ApparelLocation ApparelLocation
        {
            get
            {
                if (this.apparelLocation == null)
                {
                    var app = App.Current as App;

                    switch (app.StatisticalInterval)
                    {
                        case StatisticalInterval.Month:
                            this.apparelLocation = new ApparelLocation(new MonthRowDao());
                            break;
                        case StatisticalInterval.Season:
                            this.apparelLocation = new ApparelLocation(new SeasonRowDao());
                            break;
                        case StatisticalInterval.Year:
                            this.apparelLocation = new ApparelLocation(new YearRowDao());
                            break;
                        default:
                            this.apparelLocation = new ApparelLocation(new MonthRowDao());
                            break;
                    }
                }

                return this.apparelLocation;
            }
        }

        private App App
        {
            get
            {
                return App.Current as App;
            }
        }

        #endregion

        public Apparel_Location()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;

            this.FlipView.SelectedIndex = this.App.selectedFlipViewIndex;

            this.ViewModel.North = this.CreateFlipModel(SubLocation.North);

            this.ViewModel.Center = this.CreateFlipModel(SubLocation.Center);

            this.ViewModel.South = this.CreateFlipModel(SubLocation.South);
        }

        #region NavigationHelper 註冊

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

        #region control event

        private void BarChart_LayoutUpdated(object sender, object e)
        {
            var barLabelStrategy = this.Resources["Strategy"] as BarLabelStrategy;

            foreach (var item in barLabelStrategy.BarButtons)
            {
                item.Click -= this.BarButton_Click;
                item.Click += this.BarButton_Click;
            }
        }

        private void myItemControl_ItemClick(object sender, ItemClickEventArgs e)
        {
            var pieDataPoint = e.ClickedItem as MyPieDataPoint;

            var currentFlipModel = this.GetCurrentFlipModel();

            this.DrawAssignedStoreBar(pieDataPoint.Label, currentFlipModel);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(MainPage));
            }
        }

        private void BarButton_Click(object sender, RoutedEventArgs e)
        {
            var barButton = sender as BarButton;
            var flipModel = this.GetCurrentFlipModel();

            if (flipModel.BarInFlipModel == BarInFlipModel.Brand)
            {
                flipModel.BarInFlipModel = BarInFlipModel.Category;
                flipModel.AssignedBrand = barButton.Category;

                var salesBar = this.ApparelLocation.GetSalesAmountByCategory(flipModel.SubLocation, flipModel.AssignedBrand);

                flipModel.Bar1 = salesBar.Select(x => this.GetDefaultCartesianDataPoint(x)).ToList();
                flipModel.Bar2 = salesBar;
            }
            else if (flipModel.BarInFlipModel == BarInFlipModel.Category)
            {
                flipModel.BarInFlipModel = BarInFlipModel.Product;
                flipModel.AssignedCategory = barButton.Category;

                var salesBar = this.ApparelLocation.GetSalesAmountByProduct(flipModel.SubLocation, flipModel.AssignedBrand, flipModel.AssignedCategory);

                flipModel.Bar1 = salesBar.Select(x => this.GetDefaultCartesianDataPoint(x)).ToList();
                flipModel.Bar2 = salesBar;
            }
        }

        private void BarChartSwitchToBrand_Tapped(object sender, RoutedEventArgs e)
        {
            FlipModel flipModel = this.GetCurrentFlipModel();

            if (!string.IsNullOrEmpty(flipModel.AssignedBrand))
            {
                flipModel.BarInFlipModel = BarInFlipModel.Brand;
                flipModel.AssignedBrand = string.Empty;
                flipModel.AssignedCategory = string.Empty;

                var sales = this.ApparelLocation.GetSalesAmountByBrand(flipModel.SubLocation);
                flipModel.Bar1 = sales.Select(x => this.GetDefaultCartesianDataPoint(x)).ToList();
                flipModel.Bar2 = sales;
            }
        }

        private void BarChartSwitchToCategory_Tapped(object sender, RoutedEventArgs e)
        {
            FlipModel flipModel = this.GetCurrentFlipModel();

            if (!string.IsNullOrEmpty(flipModel.AssignedCategory))
            {
                flipModel.BarInFlipModel = BarInFlipModel.Category;
                flipModel.AssignedCategory = string.Empty;

                var sales = this.ApparelLocation.GetSalesAmountByCategory(flipModel.SubLocation, flipModel.AssignedBrand);
                flipModel.Bar1 = sales.Select(x => this.GetDefaultCartesianDataPoint(x)).ToList();
                flipModel.Bar2 = sales;
            }
        }

        #endregion

        #region private

        private FlipModel CreateFlipModel(SubLocation subLocation)
        {
            var salesByStore = this.ApparelLocation.GetSalesAmountByStore(subLocation);

            for (int i = 0; i < salesByStore.Count; i++)
            {
                salesByStore[i].Brush = ChartPalettes.DefaultLight.FillEntries.Brushes[i];
            }

            var salesByBrand = this.ApparelLocation.GetSalesAmountByBrand(subLocation);

            return new FlipModel()
            {
                SubLocation = subLocation,
                PieChart = salesByStore,
                Bar1 = salesByBrand.Select(x => this.GetDefaultCartesianDataPoint(x)).ToList(),
                Bar2 = salesByBrand,
                IsClickEnabled = false,
                AssignedBrand = string.Empty,
                BarInFlipModel = BarInFlipModel.Brand,
            };
        }

        private CartesianDataPoint GetDefaultCartesianDataPoint(CartesianDataPoint dataPoint)
        {
            return new CartesianDataPoint()
            {
                Category = dataPoint.Category,
                Value = 0,
                SubLocation = dataPoint.SubLocation,
            };
        }

        private void DrawAssignedStoreBar(string store, FlipModel flipModel)
        {
            if (flipModel.BarInFlipModel == BarInFlipModel.Brand)
            {
                this.DrawAssignedStoreBarByBrand(store, flipModel);
            }
            else if (flipModel.BarInFlipModel == BarInFlipModel.Category)
            {
                this.DrawAssignedStoreBarByCategory(store, flipModel);
            }
            else if (flipModel.BarInFlipModel == BarInFlipModel.Product)
            {
                this.DrawAssignedStoreBarByProduct(store, flipModel);
            }
        }

        private void DrawAssignedStoreBarByBrand(string store, FlipModel flipModel)
        {
            var totalSales = this.ApparelLocation.GetSalesAmountByBrand(flipModel.SubLocation);
            var storeSales = this.ApparelLocation.GetStoreSalesAmountByBrand(store, flipModel.SubLocation);

            var restRatio = new List<CartesianDataPoint>();

            if (totalSales.Count == storeSales.Count)
            {
                for (int i = 0; i < totalSales.Count; i++)
                {
                    var cartesianDataPoint = new CartesianDataPoint()
                    {
                        Category = totalSales[i].Category,
                        Value = totalSales[i].Value - storeSales[i].Value,
                        SubLocation = totalSales[i].SubLocation,
                    };

                    restRatio.Add(cartesianDataPoint);
                }
            }

            var isSameBar = this.IsSameBar(flipModel.Bar1, storeSales);

            flipModel.Bar1 = isSameBar ? storeSales.Select(x => this.GetDefaultCartesianDataPoint(x)).ToList() : storeSales;
            flipModel.Bar2 = isSameBar ? totalSales : restRatio;
        }

        private void DrawAssignedStoreBarByCategory(string store, FlipModel flipModel)
        {
            var totalSales = this.ApparelLocation.GetSalesAmountByCategory(flipModel.SubLocation, flipModel.AssignedBrand);
            var storeSales = this.ApparelLocation.GetStoreSalesAmountByCategory(store, flipModel.SubLocation, flipModel.AssignedBrand);

            var restRatio = new List<CartesianDataPoint>();

            if (totalSales.Count == storeSales.Count)
            {
                for (int i = 0; i < totalSales.Count; i++)
                {
                    var cartesianDataPoint = new CartesianDataPoint()
                    {
                        Category = totalSales[i].Category,
                        Value = totalSales[i].Value - storeSales[i].Value,
                        SubLocation = totalSales[i].SubLocation,
                    };

                    restRatio.Add(cartesianDataPoint);
                }
            }

            var isSameBar = this.IsSameBar(flipModel.Bar1, storeSales);

            flipModel.Bar1 = isSameBar ? storeSales.Select(x => this.GetDefaultCartesianDataPoint(x)).ToList() : storeSales;
            flipModel.Bar2 = isSameBar ? totalSales : restRatio;
        }

        private void DrawAssignedStoreBarByProduct(string store, FlipModel flipModel)
        {
            var totalSales = this.ApparelLocation.GetSalesAmountByProduct(flipModel.SubLocation, flipModel.AssignedBrand, flipModel.AssignedCategory);
            var storeSales = this.ApparelLocation.GetStoreSalesAmountByProduct(store, flipModel.SubLocation, flipModel.AssignedBrand, flipModel.AssignedCategory);

            var restRatio = new List<CartesianDataPoint>();

            if (totalSales.Count == storeSales.Count)
            {
                for (int i = 0; i < totalSales.Count; i++)
                {
                    var cartesianDataPoint = new CartesianDataPoint()
                    {
                        Category = totalSales[i].Category,
                        Value = totalSales[i].Value - storeSales[i].Value,
                        SubLocation = totalSales[i].SubLocation,
                    };

                    restRatio.Add(cartesianDataPoint);
                }
            }

            var isSameBar = this.IsSameBar(flipModel.Bar1, storeSales);

            flipModel.Bar1 = isSameBar ? storeSales.Select(x => this.GetDefaultCartesianDataPoint(x)).ToList() : storeSales;
            flipModel.Bar2 = isSameBar ? totalSales : restRatio;
        }

        private bool IsSameBar(List<CartesianDataPoint> bar1, List<CartesianDataPoint> bar2)
        {
            var result = false;

            if (bar1.Count == bar2.Count)
            {
                result = true;

                for (int i = 0; i < bar1.Count; i++)
                {
                    if (bar1[i].Category != bar2[i].Category ||
                        bar1[i].Value != bar2[i].Value)
                    {
                        result = false;
                    }
                }
            }

            return result;
        }

        private FlipModel GetCurrentFlipModel()
        {
            var subLocation = (SubLocation)(this.FlipView.SelectedIndex + 1);

            FlipModel flipModel;
            switch (subLocation)
            {
                case SubLocation.North:
                    flipModel = this.ViewModel.North;
                    break;
                case SubLocation.Center:
                    flipModel = this.ViewModel.Center;
                    break;
                case SubLocation.South:
                    flipModel = this.ViewModel.South;
                    break;
                default:
                    flipModel = this.ViewModel.North;
                    break;
            }

            return flipModel;
        }

        #endregion
    }
}
