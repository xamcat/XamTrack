using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace XamTrack.Core.Services
{
    public interface IGeolocationService
    {
        Task<Location> GetLastKnownLocationAsync();
    }
}
