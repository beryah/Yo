using System.Collections.Generic;
using Windows.UI.Xaml.Shapes;

namespace SignaturePadTest
{
    public class ViewModel
    {
        public ViewModel()
        {
            this.Signature = new Signature()
            {
                Lines = new List<SignatureLine>(),
            };
        }

        public Signature Signature { get; set; }
    }

    public class Signature
    {
        public double Width { get; set; }

        public double Height { get; set; }

        public List<SignatureLine> Lines { get; set; }
    }

    public class SignatureLine
    {
        public double X1 { get; set; }

        public double Y1 { get; set; }

        public double X2 { get; set; }

        public double Y2 { get; set; }
    }
}
