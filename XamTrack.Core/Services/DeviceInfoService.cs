using System;
using System.Collections.Generic;
using System.Text;

namespace XamTrack.Core.Services
{
    public class DeviceInfoService : IDeviceInfoService
    {
        public DeviceInfoService()
        {
        }

        public string GetDeviceId()
        {
            return "DeviceInfo";
        }
    }
}
