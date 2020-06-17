using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace XamTrack.Core.Services
{
    class LocationTrackerService : ILocationTrackerService
    {
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
