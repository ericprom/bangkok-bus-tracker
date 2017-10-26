using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.Storage;
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
    public sealed partial class Setting : Page
    {

        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        public Setting()
        {
            this.InitializeComponent();
            foreach (ComboBoxItem d in ComboBoxMenu.Items)
            {
                if (localSettings.Values["tracktime"] != null)
                {
                    if (d.Tag.ToString() == localSettings.Values["tracktime"].ToString())
                    {
                        ComboBoxMenu.SelectedItem = d;
                    }
                }
            }
            foreach (ComboBoxItem d in ComboBoxLoop.Items)
            {
                if (localSettings.Values["reloadtime"] != null)
                {
                    if (d.Tag.ToString() == localSettings.Values["reloadtime"].ToString())
                    {
                        ComboBoxLoop.SelectedItem = d;
                    }
                }
            }
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected  override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
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

        private void ComboBoxMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var tracktime = ((ComboBoxItem)ComboBoxMenu.SelectedItem).Tag.ToString();
                if (tracktime != null)
                    localSettings.Values["tracktime"] = tracktime;
            }
            catch (Exception ex) { }
        }
        private void ComboBoxLoop_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var reloadtime = ((ComboBoxItem)ComboBoxLoop.SelectedItem).Tag.ToString();
                if (reloadtime != null)
                    localSettings.Values["reloadtime"] = reloadtime;
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
