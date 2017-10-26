using BUS.TRACKER.Model;
using BUS.TRACKER.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace BUS.TRACKER.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Link : Page
    {
        StatusBar statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
        DataManager shareDataManager;
        PassedData param;
        TransitModel entry;
        public Link()
        {
            this.InitializeComponent();
            shareDataManager = DataManager.shareInstance;
            statusBar.BackgroundColor = Color.FromArgb(255, 44, 69, 102);
            statusBar.BackgroundOpacity = 1;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                if (e.Parameter.GetType() == typeof(PassedData))
                {
                    param = (PassedData)e.Parameter;
                    entry = (TransitModel)param.tracker;
                    statusBar.ProgressIndicator.Text = "Loading bus links...";
                    await statusBar.ProgressIndicator.ShowAsync();
                    var path = "";
                    if(param.tag == "stop"){
                        path = param.tag + "/" + entry.stop_name;
                    }
                    else
                    {
                       path = param.tag + "/" + entry.station_id;
                    }
                    shareDataManager.TransitDataSource = new TransitDataSource(path);
                    shareDataManager.TransitDataSource.LoadData(0);
                    shareDataManager.TransitDataSource.PropertyChanged += TransitDataSource_PropertyChanged;

                }
                HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            }
            catch (Exception ex) { }
        }
        private async void TransitDataSource_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            try
            {
                if (e.PropertyName == "IsLoading")
                {
                    if (shareDataManager.TransitDataSource.IsLoading == false)
                    {
                        if (shareDataManager.TransitDataSource.hasItem)
                        {
                            var result = from t in shareDataManager.TransitDataSource.Items
                                         group t by t.tran_group into g
                                         select new { Key = g.Key, Items = g };
                            groupLink.Source = result;
                        }
                        await statusBar.ProgressIndicator.HideAsync();
                    }
                }
            }
            catch (Exception ex) { }
        }
        private void RoutePivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ListView listview = (ListView)sender;
                if (listview.SelectedItem == null)
                    return;
                TransitModel entry = (TransitModel)listview.SelectedItem;
                Frame.Navigate(typeof(Stop), new PassedData
                {
                    tracker = entry
                });
                listview.SelectedItem = null;
            }
            catch (Exception ex) { }
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            try
            {
                HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
            }
            catch (Exception ex) { }
        }
        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            try
            {
                e.Handled = true;
                var frame = Window.Current.Content as Frame;
                if (frame != null && frame.CanGoBack)
                {
                    frame.GoBack();
                }
            }
            catch (Exception ex) { }
        }

    }
}
