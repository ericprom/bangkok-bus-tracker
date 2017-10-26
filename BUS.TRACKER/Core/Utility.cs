using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace BUS.TRACKER.Core
{
    public class Utility
    {
        public static async Task<string> PostData(string url, string postData)
        {
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, new Uri(url))
                {
                    Content = new StringContent(postData, Encoding.UTF8, "application/x-www-form-urlencoded")
                };
                var response = await client.SendAsync(request);
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }
        public static DependencyObject FindChildControl<T>(DependencyObject control, string ctrlName)
        {
            int childNumber = VisualTreeHelper.GetChildrenCount(control);
            for (int i = 0; i < childNumber; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(control, i);
                FrameworkElement fe = child as FrameworkElement;
                if (fe == null) return null;

                if (child is T && fe.Name == ctrlName)
                {
                    return child;
                }
                else
                {
                    DependencyObject nextLevel = FindChildControl<T>(child, ctrlName);
                    if (nextLevel != null)
                        return nextLevel;
                }
            }
            return null;
        }

    }
}
