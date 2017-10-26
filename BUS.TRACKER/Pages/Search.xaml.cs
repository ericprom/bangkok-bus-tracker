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
    public sealed partial class Search : Page
    {
        StatusBar statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
        DataManager shareDataManager; 
        public Search()
        {
            this.InitializeComponent();
            shareDataManager = DataManager.shareInstance;
            statusBar.BackgroundColor = Color.FromArgb(255, 44, 69, 102);
            statusBar.BackgroundOpacity = 1;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            }
            catch (Exception ex) { }
        }

        private async void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (((Pivot)sender).SelectedIndex)
            {
                case 0:
                    statusBar.ProgressIndicator.Text = "Loading bus routes..."; 
                    await statusBar.ProgressIndicator.ShowAsync();
                    shareDataManager.RouteDataSource = new RouteDataSource("all");
                    shareDataManager.RouteDataSource.LoadData(0);
                    shareDataManager.RouteDataSource.PropertyChanged += RouteDataSource_PropertyChanged;
                    break;
                case 1:
                    statusBar.ProgressIndicator.Text = "Loading BTS stations..."; 
                    await statusBar.ProgressIndicator.ShowAsync();
                    shareDataManager.BtsDataSource = new BtsDataSource("all");
                    shareDataManager.BtsDataSource.LoadData(0);
                    shareDataManager.BtsDataSource.PropertyChanged += BtsDataSource_PropertyChanged;
                    break;
                case 2:
                    statusBar.ProgressIndicator.Text = "Loading MRT stations...";
                    await statusBar.ProgressIndicator.ShowAsync();
                    shareDataManager.MrtDataSource = new MrtDataSource("all");
                    shareDataManager.MrtDataSource.LoadData(0);
                    shareDataManager.MrtDataSource.PropertyChanged += MrtDataSource_PropertyChanged;
                    break;
                case 3:
                    statusBar.ProgressIndicator.Text = "Loading BRT stations...";
                    await statusBar.ProgressIndicator.ShowAsync();
                    shareDataManager.BrtDataSource = new BrtDataSource("all");
                    shareDataManager.BrtDataSource.LoadData(0);
                    shareDataManager.BrtDataSource.PropertyChanged += BrtDataSource_PropertyChanged;
                    break;
                case 4:
                    statusBar.ProgressIndicator.Text = "Loading Airport Real Link stations...";
                    await statusBar.ProgressIndicator.ShowAsync();
                    shareDataManager.ArlDataSource = new ArlDataSource("all");
                    shareDataManager.ArlDataSource.LoadData(0);
                    shareDataManager.ArlDataSource.PropertyChanged += ArlDataSource_PropertyChanged;
                    break;
                case 5:
                    statusBar.ProgressIndicator.Text = "Loading bus stop...";
                    await statusBar.ProgressIndicator.ShowAsync();
                    shareDataManager.StopDataSource = new StopDataSource("all");
                    shareDataManager.StopDataSource.LoadData(0);
                    shareDataManager.StopDataSource.PropertyChanged += StopDataSource_PropertyChanged;
                    break;    
            }
        }
        private async void RouteDataSource_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            try
            {
                if (e.PropertyName == "IsLoading")
                {
                    if (shareDataManager.RouteDataSource.IsLoading == false)
                    {
                        if (shareDataManager.RouteDataSource.hasItem)
                        {
                            var result = from t in shareDataManager.RouteDataSource.Items
                                         group t by t.tran_group into g
                                         select new { Key = g.Key, Items = g };
                            groupBus.Source = result;
                        }
                        await statusBar.ProgressIndicator.HideAsync();
                    }
                }
            }
            catch (Exception ex) { }
        }
        private async void BtsDataSource_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            try
            {
                if (e.PropertyName == "IsLoading")
                {
                    if (shareDataManager.BtsDataSource.IsLoading == false)
                    {
                        if (shareDataManager.BtsDataSource.hasItem)
                        {
                            var result = from t in shareDataManager.BtsDataSource.Items
                                         group t by t.tran_group into g
                                         select new { Key = g.Key, Items = g };
                            groupBts.Source = result;
                        }
                        await statusBar.ProgressIndicator.HideAsync();
                    }
                }
            }
            catch (Exception ex) { }
        }
        private async void MrtDataSource_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            try
            {
                if (e.PropertyName == "IsLoading")
                {
                    if (shareDataManager.MrtDataSource.IsLoading == false)
                    {
                        if (shareDataManager.MrtDataSource.hasItem)
                        {
                            var result = from t in shareDataManager.MrtDataSource.Items
                                         group t by t.tran_group into g
                                         select new { Key = g.Key, Items = g };
                            groupMrt.Source = result;
                        }
                        await statusBar.ProgressIndicator.HideAsync();
                    }
                }
            }
            catch (Exception ex) { }
        }
        private async void BrtDataSource_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            try
            {
                if (e.PropertyName == "IsLoading")
                {
                    if (shareDataManager.BrtDataSource.IsLoading == false)
                    {
                        if (shareDataManager.BrtDataSource.hasItem)
                        {
                            var result = from t in shareDataManager.BrtDataSource.Items
                                         group t by t.tran_group into g
                                         select new { Key = g.Key, Items = g };
                            groupBrt.Source = result;
                        }
                        await statusBar.ProgressIndicator.HideAsync();
                    }
                }
            }
            catch (Exception ex) { }
        }
        private async void ArlDataSource_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            try
            {
                if (e.PropertyName == "IsLoading")
                {
                    if (shareDataManager.ArlDataSource.IsLoading == false)
                    {
                        if (shareDataManager.ArlDataSource.hasItem)
                        {
                            var result = from t in shareDataManager.ArlDataSource.Items
                                         group t by t.tran_group into g
                                         select new { Key = g.Key, Items = g };
                            groupArl.Source = result;
                        }
                        await statusBar.ProgressIndicator.HideAsync();
                    }
                }
            }
            catch (Exception ex) { }
        }
        private async void StopDataSource_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            try
            {
                if (e.PropertyName == "IsLoading")
                {
                    if (shareDataManager.StopDataSource.IsLoading == false)
                    {
                        if (shareDataManager.StopDataSource.hasItem)
                        {
                            var result = from t in shareDataManager.StopDataSource.Items
                                         group t by t.tran_group into g
                                         select new { Key = g.Key, Items = g };
                            groupStop.Source = result;
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
        private void BtsPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ListView listview = (ListView)sender;
                if (listview.SelectedItem == null)
                    return;
                TransitModel entry = (TransitModel)listview.SelectedItem;
                Frame.Navigate(typeof(Link), new PassedData
                {
                    tag = "bts",
                    tracker = entry
                });
                listview.SelectedItem = null;
            }
            catch (Exception ex) { }
        }
        private void MrtPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ListView listview = (ListView)sender;
                if (listview.SelectedItem == null)
                    return;
                TransitModel entry = (TransitModel)listview.SelectedItem;
                Frame.Navigate(typeof(Link), new PassedData
                {
                    tag = "mrt",
                    tracker = entry
                });
                listview.SelectedItem = null;
            }
            catch (Exception ex) { }
        }
        private void BrtPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ListView listview = (ListView)sender;
                if (listview.SelectedItem == null)
                    return;
                TransitModel entry = (TransitModel)listview.SelectedItem;
                Frame.Navigate(typeof(Link), new PassedData
                {
                    tag = "brt",
                    tracker = entry
                });
                listview.SelectedItem = null;
            }
            catch (Exception ex) { }
        }
        private void ArlPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ListView listview = (ListView)sender;
                if (listview.SelectedItem == null)
                    return;
                TransitModel entry = (TransitModel)listview.SelectedItem;
                Frame.Navigate(typeof(Link), new PassedData
                {
                    tag = "arl",
                    tracker = entry
                });
                listview.SelectedItem = null;
            }
            catch (Exception ex) { }
        }
        private void StopPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ListView listview = (ListView)sender;
                if (listview.SelectedItem == null)
                    return;
                TransitModel entry = (TransitModel)listview.SelectedItem;
                Frame.Navigate(typeof(Link), new PassedData
                {
                    tag = "stop",
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
