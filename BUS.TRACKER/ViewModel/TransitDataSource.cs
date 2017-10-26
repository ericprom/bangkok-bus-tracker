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
   public class TransitDataSource : INotifyPropertyChanged
    {
        public ObservableCollection<TransitModel> Items { get; private set; }
        public TransitDataSource()
        {
            this.Items = new ObservableCollection<TransitModel>();

        }
        public string url { get; set; }
        public TransitDataSource(string path)
        {
            this.url = "http://bus.promrat.com/bus/goto/" + path;
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
                int total = (int)json["feed"]["total"];
                foreach (var dataItem in DataItems)
                {
                    try
                    {
                        var type = "";
                        if ((string)dataItem["transit_type"] == "inbound" || (string)dataItem["transit_type"] == "outbound")
                        {
                            type = "";
                        }
                        else
                        {
                            type = (string)dataItem["transit_type"];
                        }
                        TransitModel model = new TransitModel()
                        {
                            bus_number = (string)dataItem["bus_number"],
                            tran_group = "Go to " + type + " " + (string)dataItem["station_name"],
                            trans_icon = "/Assets/bus/bus.png"
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

