using System.Collections.Generic;
using System.Linq;
using Telerik.Charting;
using Telerik.Core;
using Telerik.UI.Xaml.Controls.Chart;
using TelerikTest.Entity.Basic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace TelerikTest.Entity.Location
{
    public class BarLabelStrategy : ChartSeriesLabelStrategy
    {
        public LabelStrategyOptions options = LabelStrategyOptions.DefaultVisual | LabelStrategyOptions.Measure;

        public override LabelStrategyOptions Options
        {
            get
            {
                return this.options;
            }
        }

        public BarLabelStrategy()
        {
            this.BarButtons = new List<BarButton>();
        }

        public List<BarButton> BarButtons { get; set; }

        public override FrameworkElement CreateDefaultVisual(DataPoint point, int labelIndex)
        {
            ChartSeries series = point.Presenter as ChartSeries;

            var b = new BarButton();
            b.Key = this.BarButtons.Count;

            this.BarButtons.Add(b);
            return b;
        }

        public override RadSize GetLabelDesiredSize(DataPoint point, FrameworkElement visual, int labelIndex)
        {
            var cartesianDataPoint = point.DataItem as CartesianDataPoint;

            var barButton = visual as BarButton;

            this.BarButtons[barButton.Key].Category = cartesianDataPoint.Category;

            return new RadSize(point.LayoutSlot.Width + 5, point.LayoutSlot.Height + 5);
        }
    }
}
