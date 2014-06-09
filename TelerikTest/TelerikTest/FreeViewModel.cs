using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelerikTest
{
    public class FreeViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private List<Node> nodes;
        private int from;
        private int to;

        public List<Node> Nodes
        {
            get
            {
                return this.nodes;
            }

            set
            {
                this.nodes = value;
                this.OnPropertyChanged("Nodes");
            }
        }

        public int From
        {
            get
            {
                return this.from;
            }

            set
            {
                this.from = value;
                this.OnPropertyChanged("From");
            }
        }

        public int To
        {
            get
            {
                return this.to;
            }

            set
            {
                this.to = value;
                this.OnPropertyChanged("To");
            }
        }

        private void OnPropertyChanged(string name)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
