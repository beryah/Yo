﻿using System;
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

namespace PieChartSample
{
    /// <summary>
    /// 可以在本身使用或巡覽至框架內的空白頁面。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            var data = new List<Node>()
            {
                new Node() { Value = 20, Label = "Label_1", },
                new Node() { Value = 22, Label = "Label_2", },
                new Node() { Value = 17, Label = "Label_3", },
            };

            this.MyPieChart.Series[0].ItemsSource = data;
        }
    }

    public class Node
    {
        public double Value { get; set; }

        public string Label { get; set; } // 可選擇
    }
}
