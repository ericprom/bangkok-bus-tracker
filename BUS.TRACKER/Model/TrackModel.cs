using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS.TRACKER.Model
{
    public class TrackModel : INotifyPropertyChanged
    {
        private string _bus_number;
        public string bus_number
        {
            get
            {
                return _bus_number;
            }
            set
            {
                if (value != _bus_number)
                {
                    _bus_number = value;
                    NotifyPropertyChanged("bus_number");

                }
            }

        }
        private double _latitude;
        public double latitude
        {
            get
            {
                return _latitude;
            }
            set
            {
                if (value != _latitude)
                {
                    _latitude = value;
                    NotifyPropertyChanged("latitude");

                }
            }

        }
        private double _longitude;
        public double longitude
        {

            get
            {
                return _longitude;
            }
            set
            {
                if (value != _longitude)
                {
                    _longitude = value;
                    NotifyPropertyChanged("longitude");

                }
            }
        }
       
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}