using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TelerikTest.Entity.Location
{
    public class BarButton : Button
    {
        public BarButton()
        {
            var app = App.Current as App;
            var textBlockButtonStyle = app.Resources["TextBlockButtonStyle"] as Style;

            this.Style = textBlockButtonStyle;
        }

        public string Category { get; set; }

        public int Key { get; set; }
    }
}
