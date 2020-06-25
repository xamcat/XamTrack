using System;
using System.Collections.Generic;
using System.Text;

namespace XamTrack.Core.Services
{
    public class DeviceInfoService : IDeviceInfoService
    {
        string _deviceId;
        public DeviceInfoService()
        {
            _deviceId = Guid.NewGuid().ToString().Substring(0, 6);            
        }

        public string GetDeviceId()
        {           
            return _deviceId;
        }

        public string GetDeviceModel()
        {
            return Xamarin.Essentials.DeviceInfo.Model;
        }
    }
}
