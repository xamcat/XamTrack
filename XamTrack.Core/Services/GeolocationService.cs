using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace XamTrack.Core.Services
{
    public class GeolocationService: IGeolocationService
    {
        public event EventHandler<Location> LocationUpatedHandler;

        public async Task<string> GetCityName(Location location)
        {
            var placemarks = await Geocoding.GetPlacemarksAsync(location);

            var placemark = placemarks?.FirstOrDefault();
            if (placemark != null)
            {
                return placemark.SubAdminArea;
            }
            return "Unknown location";
        }

        public async Task<Location> GetLastKnownLocationAsync()
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();

                if (location != null)
                {
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                    LocationUpatedHandler?.Invoke(this, location);
                }

                return location;
            }
            catch (Exception ex)
            {
                // Unable to get location
                Debug.WriteLine(ex);
            }
            return null;
        }

        public async Task<Location> GetLocationAsync()
        {
            try
            {                
                var location = await Geolocation.GetLocationAsync();

                if (location != null)
                {
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                    LocationUpatedHandler?.Invoke(this, location);
                }

                return location;
            }
            catch (Exception ex)
            {
                // Unable to get location
                Debug.WriteLine(ex);
            }
            return null;
        }
    }
}
