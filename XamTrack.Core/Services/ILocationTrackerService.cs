using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace XamTrack.Core.Services
{
    public interface ILocationTrackerService
    {
        bool IsTracking { get; }
        Task<bool> StartTrackingAsync();
        Task<bool> StopTrackingAsync();
    }
}
