using BUS.TRACKER.Core;
using BUS.TRACKER.Model;
using BUS.TRACKER.ViewModel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Devices.Sensors;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace BUS.TRACKER.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Map : Page
    {
        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        StatusBar statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
        DataManager shareDataManager;
        PassedData param;
        Geolocator geolocator = null;
        private DispatcherTimer dispatcherTimer;
        double myLat,myLong;
        bool tracking = false;
        string bus_number;
        public Map()
        {
            this.InitializeComponent();

            geolocator = new Geolocator();
            geolocator.DesiredAccuracy = PositionAccuracy.High;
            geolocator.MovementThreshold = 50; 

            shareDataManager = DataManager.shareInstance;
            statusBar.BackgroundColor = Color.FromArgb(255, 44, 69, 102);
            statusBar.BackgroundOpacity = 1;

            dispatcherTimer = new DispatcherTimer();
            if (localSettings.Values["reloadtime"] != null)
            {
                dispatcherTimer.Interval = new TimeSpan(0, 0, int.Parse(localSettings.Values["reloadtime"].ToString()));
            }
            else
            {
                dispatcherTimer.Interval = new TimeSpan(0, 0, 30);
            }

            if (localSettings.Values["tracktime"] != null)
            {
                localSettings.Values["tracktime"] = localSettings.Values["tracktime"];
            }
            else
            {
                localSettings.Values["tracktime"] = 30;
            }
            
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                if (e.Parameter.GetType() == typeof(PassedData))
                {
                    param = (PassedData)e.Parameter;

                    TransitModel entry = (TransitModel)param.tracker;
                    bus_number = entry.bus_number.ToString();
                    statusBar.ProgressIndicator.Text = "BUSTRACKER - " + bus_number + " " + entry.route_name;
                    statusBar.ProgressIndicator.ProgressValue = 0;
                    await statusBar.ProgressIndicator.ShowAsync();
                    checkLocationConsent();
                }
                HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            }
            catch (Exception ex) { }
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            try
            {

                dispatcherTimer.Tick -= dispatcherTimer_Tick;
                dispatcherTimer.Stop();
                HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
            }
            catch (Exception ex) { }
        }
        private async void checkLocationConsent()
        {
            try
            {
                if (localSettings.Values["LocationConsent"] != null)
                {
                    return;
                }
                else
                {
                    var msg = new MessageDialog("This app accesses your phone's location. Is that ok", "BUS TRACKER");
                    var okBtn = new UICommand("OK");
                    var cancelBtn = new UICommand("Cancel");
                    msg.Commands.Add(okBtn);
                    msg.Commands.Add(cancelBtn);
                    IUICommand result = await msg.ShowAsync();

                    if (result != null && result.Label == "OK")
                    {
                        localSettings.Values["LocationConsent"] = true;
                        TrackMe();
                    }
                    else
                    {
                        localSettings.Values["LocationConsent"] = false;
                    }
                }

            }
            catch (Exception ex) { }
        }
        private void MyMap_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                TrackMe();
                dispatcherTimer.Tick += dispatcherTimer_Tick;
                dispatcherTimer.Start();
            }
            catch (Exception ex) { }
        }

        private void dispatcherTimer_Tick(object sender, object e)
        {
            if (param.tag == "waiting")
            {
                findTheBus(bus_number);
            }
        }
        private void TrackMe()
        {
            try
            {
                if (!tracking)
                {
                    geolocator.StatusChanged += geolocator_StatusChanged;
                    geolocator.PositionChanged += geolocator_PositionChanged;

                    tracking = true;
                }
                else
                {
                    geolocator.PositionChanged -= geolocator_PositionChanged;
                    geolocator.StatusChanged -= geolocator_StatusChanged;

                    tracking = false;
                    StatusTextBlock.Text = "stopped";
                }
            }
            catch (Exception ex) { }
        }

        private async void geolocator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            try
            {
                await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                {
                    myLat = args.Position.Coordinate.Point.Position.Latitude;
                    myLong = args.Position.Coordinate.Point.Position.Longitude;
                    if (param.tag == "sitting")
                    {
                        save_location(bus_number, args.Position.Coordinate.Point);
                    }
                    if (param.tag == "waiting")
                    {
                        findTheBus(bus_number);
                    }
                    iAmHere(args.Position.Coordinate.Point);
                });
            }
            catch (Exception ex) { }
        }
        private async void iAmHere(Geopoint location)
        {
            try
            {
                MyMap.Children.Clear();
                AddPushpin(location, "You");
                await MyMap.TrySetViewAsync(location, 15, 0, 0, MapAnimationKind.Bow);
            }
            catch (Exception ex) { }
        }
        private void findTheBus(string route_id)
        {
            try
            {
                if (route_id == "") { return; }
                StatusTextBlock.Visibility = Windows.UI.Xaml.Visibility.Visible;
                StatusTextBlock.Text = "Tracking...";
                shareDataManager.TrackDataSource = new TrackDataSource(route_id,localSettings.Values["tracktime"].ToString());
                shareDataManager.TrackDataSource.LoadData(0);
                shareDataManager.TrackDataSource.PropertyChanged += TrackDataSource_PropertyChanged;
            }
            catch (Exception ex) { }
        }

        private void TrackDataSource_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            try
            {
                if (e.PropertyName == "IsLoading")
                {
                    if (shareDataManager.TrackDataSource.IsLoading == false)
                    {
                        if (shareDataManager.TrackDataSource.hasItem)
                        {
                            iAmHere(new Geopoint(new BasicGeoposition()
                            {
                                Latitude = myLat,
                                Longitude = myLong
                            }));
                            var result = shareDataManager.TrackDataSource.Items;
                            for (int i = 0; i < result.Count(); i++)
                            {
                                AddPushpin(new Geopoint(new BasicGeoposition() {
                                    Latitude = result[i].latitude,
                                    Longitude = result[i].longitude 
                                }), result[i].bus_number);

                            }
                        }
                        if (StatusTextBlock.Visibility == Windows.UI.Xaml.Visibility.Visible)
                        {
                            StatusTextBlock.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                        }
                    }
                }
            }
            catch (Exception ex) { }
        }

        private async void geolocator_StatusChanged(Geolocator sender, StatusChangedEventArgs args)
        {
            try
            {
                string status = "";

                switch (args.Status)
                {
                    case PositionStatus.Disabled:
                        status = "location is disabled in phone settings";
                        break;
                    case PositionStatus.Initializing:
                        status = "initializing map...";
                        break;
                    case PositionStatus.NoData:
                        status = "no data";
                        break;
                    case PositionStatus.Ready:
                        status = "ready";
                        break;
                    case PositionStatus.NotAvailable:
                        status = "not available";
                        break;
                    case PositionStatus.NotInitialized:
                        break;
                }
                await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                {
                    StatusTextBlock.Text = status;
                    if (status == "ready")
                        StatusTextBlock.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    else
                        StatusTextBlock.Visibility = Windows.UI.Xaml.Visibility.Visible;
                });
            }
            catch (Exception ex) { }
        }
        public void AddPushpin(Geopoint location, string text)
        {
            try
            {
                var pin = new Grid()
                {
                    Width = 30,
                    Height = 30,
                    Margin = new Windows.UI.Xaml.Thickness(-12)
                };

                var color = Colors.DodgerBlue;
                if (text == "You")
                {
                    color = Colors.DodgerBlue;
                }
                else
                {
                    color = Colors.DarkOrange;
                }

                pin.Children.Add(new Ellipse()
                {
                    Fill = new SolidColorBrush(color),
                    Stroke = new SolidColorBrush(Colors.White),
                    StrokeThickness = 3,
                    Width = 30,
                    Height = 30
                });

                pin.Children.Add(new TextBlock()
                {
                    Text = text,
                    FontSize = 12,
                    Foreground = new SolidColorBrush(Colors.White),
                    HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center,
                    VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center
                });
                MapControl.SetLocation(pin, location);
                MyMap.Children.Add(pin);

            }
            catch (Exception ex) { }

        }
        private async void save_location(string route, Geopoint location)
        {
            try
            {
                var body = new StringBuilder();
                body.Append("&route=");
                body.Append(Uri.EscapeDataString(route));
                body.Append("&lat=");
                body.Append(location.Position.Latitude.ToString("0.000000"));
                body.Append("&lng=");
                body.Append(location.Position.Longitude.ToString("0.000000"));
                body.Append("&time=");
                var timestamp = Utility.GetTimeStamp();
                body.Append(Uri.EscapeDataString(timestamp));
                var content = await Utility.PostData("http://bus.promrat.com/bus/tracker", body.ToString());
                if (!string.IsNullOrEmpty(content))
                {
                    JObject json = JObject.Parse(content);
                    if ((bool)json["status"]){ }
                }

            }
            catch (Exception ex) { }
        }
        private void Traffic_Checked(object sender, RoutedEventArgs e)
        {
            MyMap.TrafficFlowVisible = true; 
        }

        private void Traffic_Unchecked(object sender, RoutedEventArgs e)
        {
            MyMap.TrafficFlowVisible = false;
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
