using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace XamTrack.Core.Services
{
    public interface ILocationTrackerService
    {
        Task<bool> StartTrackingAsync();
        Task<bool> StopTrackingAsync();
    }
}
