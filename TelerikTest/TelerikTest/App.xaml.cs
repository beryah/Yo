using System;
using System.Collections.Generic;
using Telerik.UI.Xaml.Controls.Chart;
using TelerikTest.Entity.Basic;
using TelerikTest.Enum;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// 空白應用程式範本已記錄在 http://go.microsoft.com/fwlink/?LinkId=234227

namespace TelerikTest
{
    /// <summary>
    /// 提供應用程式專屬行為以補充預設的應用程式類別。
    /// </summary>
    sealed partial class App : Application
    {
        public int selectedFlipViewIndex = 0;

        private List<RowInfo> data { get; set; }

        private readonly string[] products = { "產品1", "產品2", "產品3", "產品4", "產品5", "產品6", "產品7", "產品8", "產品9", };

        private readonly string[] stores = { "分店1", "分店2", "分店3", "分店4", "分店5", };

        private readonly string[] brands = { "品牌1", "品牌2", "品牌3", };

        private readonly string[] categories = { "分類1", "分類2", "分類3", "分類4", "分類5" };

        private Random random = new Random();

        /// <summary>
        /// 初始化單一應用程式物件。這是第一行執行之撰寫程式碼，
        /// 而且其邏輯相當於 main() 或 WinMain()。
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            this.StatisticalInterval = StatisticalInterval.Month;
        }

        public List<RowInfo> Data
        {
            get
            {
                if (this.data == null)
                {
                    this.data = new List<RowInfo>();

                    //// Create base data first
                    foreach (var product in this.products)
                    {
                        foreach (var store in this.stores)
                        {
                            foreach (var brand in this.brands)
                            {
                                foreach (var category in this.categories)
                                {
                                    for (int subLocation = 1; subLocation <= 3; subLocation++)
                                    {
                                        for (int month = 1; month <= 12; month++)
                                        {
                                            for (int year = 0; year < 3; year++)
                                            {
                                                var row = new RowInfo()
                                                {
                                                    Product = product,
                                                    SalesAmount = this.random.Next(10, 200),
                                                    Store = store,
                                                    Brand = brand,
                                                    Category = category,
                                                    SubLocation = (SubLocation)subLocation,
                                                    Month = month,
                                                    Year = DateTime.Now.Year - year,
                                                };

                                                this.data.Add(row);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                return this.data;
            }
        }

        public StatisticalInterval StatisticalInterval { get; set; }

        /// <summary>
        /// 在應用程式由使用者正常啟動時叫用。其他進入點
        /// 將在例如啟動應用程式時使用以開啟特定檔案。
        /// </summary>
        /// <param name="e">關於啟動要求和處理序的詳細資料。</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {

#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;

            // 當視窗已經有內容時，不重複應用程式初始化，
            // 只確定視窗是作用中
            if (rootFrame == null)
            {
                // 建立框架做為巡覽內容，並巡覽至第一頁
                rootFrame = new Frame();
                // 設定預設語言
                rootFrame.Language = Windows.Globalization.ApplicationLanguages.Languages[0];

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO:  從之前暫停的應用程式載入狀態
                }

                // 將框架放在目前視窗中
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // 在巡覽堆疊未還原時，巡覽至第一頁，
                // 設定新的頁面，方式是透過傳遞必要資訊做為巡覽
                // 參數
                rootFrame.Navigate(typeof(MainPage), e.Arguments);
            }
            // 確定目前視窗是作用中
            Window.Current.Activate();
        }

        /// <summary>
        /// 在巡覽至某頁面失敗時叫用
        /// </summary>
        /// <param name="sender">使巡覽失敗的畫面格</param>
        /// <param name="e">有關巡覽失敗的詳細資料</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// 在應用程式暫停執行時叫用。應用程式狀態會儲存起來，
        /// 但不知道應用程式即將結束或繼續，而且仍將記憶體
        /// 的內容保持不變。
        /// </summary>
        /// <param name="sender">暫停之要求的來源。</param>
        /// <param name="e">有關暫停之要求的詳細資料。</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO:  儲存應用程式狀態，並停止任何背景活動
            deferral.Complete();
        }
    }
}
