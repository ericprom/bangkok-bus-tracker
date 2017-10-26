using BUS.TRACKER.Core;
using BUS.TRACKER.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS.TRACKER.ViewModel
{
    public class StopDataSource : INotifyPropertyChanged
    {
        public ObservableCollection<TransitModel> Items { get; private set; }
        public StopDataSource()
        {
            this.Items = new ObservableCollection<TransitModel>();

        }
        public string url { get; set; }
        public string path { get; set; }
        public StopDataSource(string path)
        {
            this.path = path;
            this.url = "http://bus.promrat.com/bus/stop/" + path;
            this.Items = new ObservableCollection<TransitModel>();

        }


        private bool _hasItem = false;
        public bool hasItem
        {
            get
            {
                return _hasItem;
            }
            set
            {
                _hasItem = value;
                NotifyPropertyChanged("hasItem");

            }
        }

        private bool _isLoading = false;
        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                _isLoading = value;
                NotifyPropertyChanged("IsLoading");

            }
        }

        public async void LoadData(int skip)
        {
            try
            {
                if (skip == 0)
                {
                    this.Items.Clear();
                }

                this.IsLoading = true;

                var content = await Utility.PostData(this.url, "");
                if (!string.IsNullOrEmpty(content))
                {
                    downloadCompleted(content);
                }
            }
            catch (Exception) { }
        }

        private void downloadCompleted(string jsonString)
        {
            try
            {
                JObject json = JObject.Parse(jsonString);
                List<JToken> DataItems = json["feed"]["entry"].ToList();
                foreach (var dataItem in DataItems)
                {
                    try
                    {
                        var path = "";
                        if (this.path == "all")
                            path = "Bus Stops";
                        else
                            path = "ROUTE " + (string)dataItem["bus_number"];
                        TransitModel model = new TransitModel()
                        {
                            stop_id = (int)dataItem["stop_id"],
                            stop_name = (string)dataItem["stop_name"],
                            tran_group = path,
                            trans_icon = "/Assets/bus/stop.png"
                        };
                        this.Items.Add(model);
                    }
                    catch (Exception) { }
                }
                this.hasItem = DataItems.Count() != 0;
            }
            catch (Exception) { }
            this.IsLoading = false;
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
