using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// 空白頁項目範本已記錄在 http://go.microsoft.com/fwlink/?LinkId=234238

namespace PrintTest
{
    /// <summary>
    /// 可以在本身使用或巡覽至框架內的空白頁面。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private Printer printer = new Printer();

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await printer.Print();
        }
    }
}