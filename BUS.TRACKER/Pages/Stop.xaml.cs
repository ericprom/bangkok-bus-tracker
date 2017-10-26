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
    public sealed partial class Stop : Page
    {
        StatusBar statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
        DataManager shareDataManager;
        PassedData param;
        TransitModel entry;
        public Stop()
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
                if (e.Parameter.GetType() == typeof(PassedData))
                {
                    param = (PassedData)e.Parameter;
                    entry = (TransitModel)param.tracker;

                }
                HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            }
            catch (Exception ex) { }
        }
        private async void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (((Pivot)sender).SelectedIndex)
            {
                case 0:
                    statusBar.ProgressIndicator.Text = "Loading inbound routes..."; 
                    await statusBar.ProgressIndicator.ShowAsync();
                    shareDataManager.StopDataSource = new StopDataSource(entry.bus_number + "/inbound");
                    shareDataManager.StopDataSource.LoadData(0);
                    shareDataManager.StopDataSource.PropertyChanged += StopDataSource_PropertyChanged;
                    break;
                case 1: 
                    statusBar.ProgressIndicator.Text = "Loading outbound routes...";
                    await statusBar.ProgressIndicator.ShowAsync();
                    shareDataManager.StopDataSource = new StopDataSource(entry.bus_number+"/outbound");
                    shareDataManager.StopDataSource.LoadData(0);
                    shareDataManager.StopDataSource.PropertyChanged += StopDataSource_PropertyChanged;
                    break;
            }
        }
        private async void StopDataSource_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            try
            {
                if (e.PropertyName == "IsLoading")
                {
                    if (shareDataManager.RouteDataSource.IsLoading == false)
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
