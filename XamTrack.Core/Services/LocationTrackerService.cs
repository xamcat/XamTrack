using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace XamTrack.Core.Services
{
    public class LocationTrackerService : ILocationTrackerService
    {
        IIoTDeviceClientService _deviceClientService;
        IGeolocationService _geolocationService;
        LocationTrackerService(IIoTDeviceClientService ioTDeviceClientService, IGeolocationService geolocationService)
        {
            _deviceClientService = ioTDeviceClientService;
            _geolocationService = geolocationService;
        }

        public Task<bool> StartTrackingAsync()
        {
            return Task.FromResult(true);
        }

        public Task<bool> StopTrackingAsync()
        {
            return Task.FromResult(true);
        }
    }
}
