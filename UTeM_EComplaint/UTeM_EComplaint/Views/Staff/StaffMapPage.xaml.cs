using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using UTeM_EComplaint.Tools;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;

namespace UTeM_EComplaint.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StaffMapPage : ContentPage, IQueryAttributable
    {
        string longitude;
        string latitude;
        public StaffMapPage()
        {
            InitializeComponent();
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            if (query.ContainsKey("longitude"))
            {
                longitude = HttpUtility.UrlDecode(query["longitude"]).Trim();
                latitude = HttpUtility.UrlDecode(query["latitude"]).Trim();

                maps.MoveToRegion(MapHandler.moveToLocationGoogle(Convert.ToDouble(latitude), Convert.ToDouble(longitude)));
            }
            else
                getLocation();
        }

        public async void getLocation()
         {
             try
             {
                 var location = await Geolocation.GetLastKnownLocationAsync();

                 maps.MoveToRegion(MapHandler.moveToLocationGoogle(location.Latitude, location.Longitude));

                 if (location != null)
                 {
                     Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                     Latitude.Text = location.Latitude.ToString();
                     Longitude.Text = location.Longitude.ToString();
                 }
             }
             catch (FeatureNotSupportedException fnsEx)
             {
                 // Handle not supported on device exception
             }
             catch (FeatureNotEnabledException fneEx)
             {
                 // Handle not enabled on device exception
             }
             catch (PermissionException pEx)
             {
                 // Handle permission exception
             }
             catch (Exception ex)
             {
                 // Unable to get location
             }
         }

        bool firstTime = true;

        [Obsolete]
        private void maps_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var map = sender as Xamarin.Forms.GoogleMaps.Map;
            if(map.VisibleRegion != null)
            {
                if(firstTime)
                {
                    firstTime = false;
                    return;
                }
                Latitude.Text = map.VisibleRegion.Center.Latitude.ToString();
                Longitude.Text = map.VisibleRegion.Center.Longitude.ToString();
            }
        }

        private async void ToolbarItem_ClickedAsync(object sender, EventArgs e)
        {
            var answer = await Application.Current.MainPage.DisplayAlert("Position", "Are you confirm the position?", "YES", "NO");
            if (answer)
            {
                await Shell.Current.GoToAsync(string.Format("../?longitude={0}&latitude={1}", Longitude.Text, Latitude.Text));
            }
        }
    }
}