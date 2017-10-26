using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS.TRACKER.Model
{
    public class TransitModel : INotifyPropertyChanged
    {
        //bus model
        private int _bus_id;
        public int bus_id
        {
            get
            {
                return _bus_id;
            }
            set
            {
                if (value != _bus_id)
                {
                    _bus_id = value;
                    NotifyPropertyChanged("bus_id");

                }
            }

        }
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
        private string _route_name;
        public string route_name
        {

            get
            {
                return _route_name;
            }
            set
            {
                if (value != _route_name)
                {
                    _route_name = value;
                    NotifyPropertyChanged("route_name");

                }
            }
        }
        //bts model
        private int _station_id;
        public int station_id
        {
            get
            {
                return _station_id;
            }
            set
            {
                if (value != _station_id)
                {
                    _station_id = value;
                    NotifyPropertyChanged("station_id");

                }
            }

        }
        private string _station_name;
        public string station_name
        {
            get
            {
                return _station_name;
            }
            set
            {
                if (value != _station_name)
                {
                    _station_name = value;
                    NotifyPropertyChanged("station_name");

                }
            }

        }

        private int _stop_id;
        public int stop_id
        {
            get
            {
                return _stop_id;
            }
            set
            {
                if (value != _stop_id)
                {
                    _stop_id = value;
                    NotifyPropertyChanged("stop_id");

                }
            }

        }
        private string _stop_name;
        public string stop_name
        {
            get
            {
                return _stop_name;
            }
            set
            {
                if (value != _stop_name)
                {
                    _stop_name = value;
                    NotifyPropertyChanged("stop_name");

                }
            }
        }
        private string _tran_group;
        public string tran_group
        {

            get
            {
                return _tran_group;
            }
            set
            {
                if (value != _tran_group)
                {
                    _tran_group = value;
                    NotifyPropertyChanged("tran_group");

                }
            }
        }
        private string _trans_icon;
        public string trans_icon
        {

            get
            {
                return _trans_icon;
            }
            set
            {
                if (value != _trans_icon)
                {
                    _trans_icon = value;
                    NotifyPropertyChanged("trans_icon");

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