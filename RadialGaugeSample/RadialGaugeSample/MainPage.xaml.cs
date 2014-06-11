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

namespace RadialGaugeSample
{
    /// <summary>
    /// 可以在本身使用或巡覽至框架內的空白頁面。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        AA MyRadiaGauge = new AA();


        public MainPage()
        {
            this.InitializeComponent();

            // 目前：{ ArrowValue = 20, MaxValue = 50 }
            // 改成：{ ArrowValue = 70, MaxValue = 90 }

            // 1. 先將 ArrowValue 歸零
            this.MyRadiaGauge.ArrowValue = 0;

            // 2. 設定 MaxValue
            this.MyRadiaGauge.MaxValue = 90;

            // 3. 設定ArrowValue
            this.MyRadiaGauge.ArrowValue = 70;
        }
    }

    public class AA
    {
        private double arrowValue;

        public double ArrowValue
        {
            get
            {
                return this.arrowValue;
            }

            set
            {
                this.arrowValue = value > this.MaxValue ? this.MaxValue : value;
                // Notify property changed...
            }
        }

        public double MaxValue { get; set; }
    }
}
