using Windows.UI.Xaml;

namespace TelerikTest.Entity.Basic
{
    public class TransformMargin
    {
        public double Left { get; set; }

        public double Top { get; set; }

        public double RatioX { get; set; }

        public double RatioY { get; set; }

        public Thickness Thickness
        {
            get
            {
                return new Thickness()
                {
                    Left = this.Left * this.RatioX,
                    Top = this.Top * this.RatioY,
                };
            }
        }
    }
}
