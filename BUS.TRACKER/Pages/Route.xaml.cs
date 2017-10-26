using BUS.TRACKER.Model;
using BUS.TRACKER.ViewModel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.UI;
using Windows.UI.Popups;
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
    public sealed partial class Route : Page
    {
        StatusBar statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
        DataManager shareDataManager; 
        PassedData param;
        public Route()
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
                    switch (param.tag)
                    {
                        case "sitting":
                            statusBar.ProgressIndicator.Text = "What bus number are you in?"; 
                            await statusBar.ProgressIndicator.ShowAsync();
                            break;
                        case "waiting":
                            statusBar.ProgressIndicator.Text = "What bus number are you waiting for?";
                            await statusBar.ProgressIndicator.ShowAsync();
                            break;
                    }
                    shareDataManager.RouteDataSource = new RouteDataSource("all");
                    shareDataManager.RouteDataSource.LoadData(0);
                    shareDataManager.RouteDataSource.PropertyChanged += RouteDataSource_PropertyChanged;

                }
                HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            }
            catch (Exception ex) { }
        }
        private void RouteDataSource_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            try
            {
                if (e.PropertyName == "IsLoading")
                {
                    if (shareDataManager.RouteDataSource.IsLoading == false)
                    {
                        statusBar.ProgressIndicator.ProgressValue = 0;
                        if (shareDataManager.RouteDataSource.hasItem)
                        {
                            var result = from t in shareDataManager.RouteDataSource.Items
                                         group t by t.route_name into g
                                         select new { Key = g.Key, Items = g };
                            groupRoute.Source = result;
                        }
                    }
                }
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
        private  void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
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

        private void lvGroupRoute_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ListView listview = (ListView)sender;
                if (listview.SelectedItem == null)
                    return;
                TransitModel entry = (TransitModel)listview.SelectedItem;
                Frame.Navigate(typeof(Map), new PassedData
                {
                    tag = param.tag,
                    tracker = entry
                });
                listview.SelectedItem = null;
            }
            catch (Exception ex) { }
        }
    }
}
