using System;
using System.Collections.Generic;
using System.Text;

namespace XamTrack.Core.Services
{
    public interface IDeviceInfoService
    {
        string GetDeviceId();
        string GetDeviceModel();
        string GetDeviceName();
    }
}
