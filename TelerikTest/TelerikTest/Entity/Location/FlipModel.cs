using System.Collections.Generic;
using System.ComponentModel;
using TelerikTest.Entity.Basic;
using TelerikTest.Enum;
using Windows.UI.Xaml.Controls;

namespace TelerikTest.Entity.Location
{
    public class FlipModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private List<CartesianDataPoint> bar1;

        private List<CartesianDataPoint> bar2;

        private string assignedBrand;

        private string assignedCategory;

        public SubLocation SubLocation { get; set; }

        public string Title
        {
            get
            {
                switch (this.SubLocation)
                {
                    case SubLocation.North:
                        return "北區";
                    case SubLocation.Center:
                        return "中區";
                    case SubLocation.South:
                        return "南區";
                    default:
                        return string.Empty;
                }
            }
        }

        public List<PieDataPoint> PieChart { get; set; }

        public List<CartesianDataPoint> Bar1
        {
            get
            {
                return this.bar1;
            }

            set
            {
                this.bar1 = value;
                this.OnPropertyChanged("Bar1");
            }
        }

        public List<CartesianDataPoint> Bar2
        {
            get
            {
                return this.bar2;
            }

            set
            {
                this.bar2 = value;
                this.OnPropertyChanged("Bar2");
            }
        }

        public bool IsClickEnabled { get; set; }

        public string AssignedBrand
        {
            get
            {
                return this.assignedBrand;
            }

            set
            {
                this.assignedBrand = value;
                this.OnPropertyChanged("BarChartStratumBrand");
            }
        }

        public string BarChartStratumBrand
        {
            get
            {
                if (!string.IsNullOrEmpty(this.AssignedBrand))
                {
                    return string.Format(" > {0}", this.AssignedBrand);
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public string AssignedCategory
        {
            get
            {
                return this.assignedCategory;
            }

            set
            {
                this.assignedCategory = value;
                this.OnPropertyChanged("BarChartStratumCategory");
            }
        }

        public string BarChartStratumCategory
        {
            get
            {
                if (!string.IsNullOrEmpty(this.AssignedCategory))
                {
                    return string.Format(" > {0}", this.AssignedCategory);
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public BarInFlipModel BarInFlipModel { get; set; }

        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
