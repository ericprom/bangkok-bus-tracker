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
    public class BtsDataSource : INotifyPropertyChanged
    {
        public ObservableCollection<TransitModel> Items { get; private set; }
        public BtsDataSource()
        {
            this.Items = new ObservableCollection<TransitModel>();

        }
        public string url { get; set; }
        public BtsDataSource(string path)
        {

            this.url = "http://bus.promrat.com/bts/station/" + path;
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
                        var bts_line_icon = "";
                        if ((string)dataItem["bts_line"] == "Sukhumvit")
                        {
                            bts_line_icon = "/Assets/bts/sukhumvit.png";
                        }
                        else
                        {
                            bts_line_icon = "/Assets/bts/silom.png";
                        }
                        TransitModel model = new TransitModel()
                        {
                            station_id = (int)dataItem["station_id"],
                            station_name = (string)dataItem["station_name"],
                            tran_group = (string)dataItem["bts_line"],
                            trans_icon = bts_line_icon,
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