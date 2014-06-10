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

// 空白頁項目範本已記錄在 http://go.microsoft.com/fwlink/?LinkId=234238

namespace BulletGraphSample
{
    /// <summary>
    /// 可以在本身使用或巡覽至框架內的空白頁面。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private BulletGraph MyBulletGraph = new BulletGraph();

        public MainPage()
        {
            this.InitializeComponent();

            // 目前：{ FeatureMeasure = 20, EndValue = 50 }
            // 改成：{ FeatureMeasure = 70, EndValue = 90 }

            // 1. 先將 FeatureMeasure 歸零
            this.MyBulletGraph.FeatureMeasure = 0;

            // 2. 設定 EndValue
            this.MyBulletGraph.EndValue = 90;

            // 3. 設定 FeatureMeasure
            this.MyBulletGraph.FeatureMeasure = 70;
        }
    }

    public class BulletGraph
    {
        private double featureMeasure;

        public double EndValue { get; set; }

        public double FeatureMeasure
        {
            get
            {
                return this.featureMeasure;
            }

            set
            {
                this.featureMeasure = value > this.EndValue ? this.EndValue : value;
                // Notify property changed...
            }
        }
    }
}
