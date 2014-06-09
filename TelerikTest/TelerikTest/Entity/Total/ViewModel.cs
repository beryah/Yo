using System.Collections.Generic;
using System.ComponentModel;
using TelerikTest.Entity.Basic;
using Windows.UI.Xaml;

namespace TelerikTest.Entity.Total
{
    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region Fields

        private List<CartesianDataPoint> totalBarChart;

        private List<CartesianDataPoint> northBarChart;

        private List<CartesianDataPoint> centerBarChart;

        private List<CartesianDataPoint> southBarChart;

        private LocationStackSize locationStackSize;

        private Thickness totalLocationMargin;

        private Thickness northLocationMargin;

        private Thickness centerLocationMargin;

        private Thickness southLocationMargin;

        #endregion

        #region Properties

        public List<CartesianDataPoint> TotalBarChart
        {
            get
            {
                return this.totalBarChart;
            }

            set
            {
                this.totalBarChart = value;
                this.OnPropertyChanged("TotalBarChart");
            }
        }

        public List<CartesianDataPoint> NorthBarChart
        {
            get
            {
                return this.northBarChart;
            }

            set
            {
                this.northBarChart = value;
                this.OnPropertyChanged("NorthBarChart");
            }
        }

        public List<CartesianDataPoint> CenterBarChart
        {
            get
            {
                return this.centerBarChart;
            }

            set
            {
                this.centerBarChart = value;
                this.OnPropertyChanged("CenterBarChart");
            }
        }

        public List<CartesianDataPoint> SouthBarChart
        {
            get
            {
                return this.southBarChart;
            }

            set
            {
                this.southBarChart = value;
                this.OnPropertyChanged("SouthBarChart");
            }
        }

        public LocationStackSize LocationStackSize
        {
            get
            {
                return this.locationStackSize;
            }

            set
            {
                this.locationStackSize = value;
                this.OnPropertyChanged("LocationStackSize");
            }
        }

        public Thickness TotalLocationMargin
        {
            get
            {
                return this.totalLocationMargin;
            }

            set
            {
                this.totalLocationMargin = value;
                this.OnPropertyChanged("TotalLocationMargin");
            }
        }

        public Thickness NorthLocationMargin
        {
            get
            {
                return this.northLocationMargin;
            }

            set
            {
                this.northLocationMargin = value;
                this.OnPropertyChanged("NorthLocationMargin");
            }
        }

        public Thickness CenterLocationMargin
        {
            get
            {
                return this.centerLocationMargin;
            }

            set
            {
                this.centerLocationMargin = value;
                this.OnPropertyChanged("CenterLocationMargin");
            }
        }

        public Thickness SouthLocationMargin
        {
            get
            {
                return this.southLocationMargin;
            }

            set
            {
                this.southLocationMargin = value;
                this.OnPropertyChanged("SouthLocationMargin");
            }
        }

        #endregion

        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
