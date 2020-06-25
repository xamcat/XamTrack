using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace XamTrack.Core.Models
{
    public class IoTMessage
    {
        public IoTMessage(Location location, string statusText, string deviceId, string deviceName, string deviceModel)
        {
            Id = Guid.NewGuid().ToString();
            Latitude = location.Latitude;
            Longitude = location.Longitude;
            Altitude = location.Altitude;
            Accuracy = location.Accuracy;
            Speed = location.Speed;

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
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double? Altitude { get; set; }
        public double? Accuracy { get; set; }
        public double? Speed { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}
