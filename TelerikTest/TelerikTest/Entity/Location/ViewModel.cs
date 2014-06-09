using System.ComponentModel;

namespace TelerikTest.Entity.Location
{
    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region Fields

        private FlipModel north { get; set; }

        private FlipModel center { get; set; }

        private FlipModel south { get; set; }

        #endregion

        #region Properties

        public FlipModel North
        {
            get
            {
                return this.north;
            }

            set
            {
                this.north = value;
                this.OnPropertyChanged("North");
            }
        }

        public FlipModel Center
        {
            get
            {
                return this.center;
            }

            set
            {
                this.center = value;
                this.OnPropertyChanged("Center");
            }
        }

        public FlipModel South
        {
            get
            {
                return this.south;
            }

            set
            {
                this.south = value;
                this.OnPropertyChanged("South");
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
