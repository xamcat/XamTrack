using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace XamTrack.Core.Services
{
    public interface IGeolocationService
    {
        event EventHandler<Location> LocationUpatedHandler;
        Task<Location> GetLastKnownLocationAsync();
        Task<Location> GetLocationAsync();
        Task<string> GetCityName(Location location);
    }
}
