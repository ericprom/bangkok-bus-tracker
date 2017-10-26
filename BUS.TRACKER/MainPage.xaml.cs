using BUS.TRACKER.Model;
using BUS.TRACKER.Pages;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace BUS.TRACKER
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        StatusBar statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
        DataManager shareDataManager;
        CommandBar mainBar;
        AppBarButton searchBtn, settingBtn;
        public MainPage()
        {
            this.InitializeComponent();
            PrepareAppBars();
            BottomAppBar = mainBar;
            shareDataManager = DataManager.shareInstance;
            statusBar.BackgroundColor = Color.FromArgb(255, 44, 69, 102);
            statusBar.BackgroundOpacity = 1;
            this.NavigationCacheMode = NavigationCacheMode.Required;
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                await statusBar.ProgressIndicator.HideAsync();
                HardwareButtons.BackPressed += HardwareButtons_BackPressed;
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
        #region prepare appbar
        private void PrepareAppBars()
        {
            try
            {
                mainBar = new CommandBar();
                mainBar.Background = new SolidColorBrush(Color.FromArgb(255, 44, 69, 102));
                mainBar.IsOpen = true;
                
                searchBtn = new AppBarButton() { Icon = new BitmapIcon() { UriSource = new Uri("ms-appx:///Assets/icons/search.png") } };
                searchBtn.Label = "Search";
                searchBtn.Tag = "Search";
                searchBtn.Click += appbarBtn_Clicked;
                mainBar.PrimaryCommands.Add(searchBtn);

                settingBtn = new AppBarButton();
                //settingBtn = new AppBarButton() { Icon = new BitmapIcon() { UriSource = new Uri("ms-appx:///Assets/icons/settings.png") } };
                settingBtn.Label = "Settings";
                settingBtn.Tag = "Setting";
                settingBtn.Click += appbarBtn_Clicked;
                mainBar.SecondaryCommands.Add(settingBtn);
                //mainBar.PrimaryCommands.Add(settingBtn);
            }
            catch (Exception ex) { }
        }

        private void appbarBtn_Clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                var Button = sender as Button;
                if (Button == null) return;
                switch ((string)Button.Tag)
                {
                    case "Setting":
                        Frame.Navigate(typeof(Setting));
                        break;
                    case "Search":
                        Frame.Navigate(typeof(Search));
                        break;
                }
            }
            catch (Exception ex) { }
        }

        #endregion
        private void BusClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                var Button = sender as Button;
                if (Button == null) return;
                Frame.Navigate(typeof(Route), new PassedData
                {
                    tag = (string)Button.Tag
                });
            }
            catch (Exception ex) { }
        }

        private async void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            try
            {
                e.Handled = true;
                var frame = Window.Current.Content as Frame;
                if (frame != null && frame.CanGoBack)
                {
                    frame.GoBack();
                }
                else
                {
                    var msg = new MessageDialog("คุณต้องการออกจากแอพใช่หรือไม่?", "BUS TRACKER");
                    var okBtn = new UICommand("OK");
                    var cancelBtn = new UICommand("Cancel");
                    msg.Commands.Add(okBtn);
                    msg.Commands.Add(cancelBtn);
                    IUICommand result = await msg.ShowAsync();

                    if (result != null && result.Label == "OK")
                    {
                        Application.Current.Exit();
                    }
                }
            }
            catch (Exception ex) { }
        }
    }
}
