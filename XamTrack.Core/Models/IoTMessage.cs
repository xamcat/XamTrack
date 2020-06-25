using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace XamTrack.Core.Models
{
    public class IoTMessage: Location
    {
        public IoTMessage(Location location, string statusText, string deviceId, string deviceName, string deviceModel):base(location)
        {
            Id = Guid.NewGuid().ToString();
            StatusText = statusText;
            DeviceId = deviceId;
            DeviceName= deviceName;
            DeviceModel = deviceModel;
            CreatedDateTime = DateTime.UtcNow;
        }

        public string Id { get; set; }
        public string DeviceId { get; set; }
        public string DeviceModel { get; set; }
        public string DeviceName { get; set; }
        public string StatusText { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}
