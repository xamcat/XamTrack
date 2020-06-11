using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace XamTrack.Services
{
    public interface IGeolocationService
    {
        Task<Location> GetLastKnownLocationAsync();


        Exception Error { get; }

    }
}
