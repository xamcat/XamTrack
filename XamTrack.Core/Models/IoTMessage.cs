using System;
using System.Collections.Generic;
using System.Text;

namespace XamTrack.Core.Models
{
    class IoTMessage
    {
        string Id { get; set; }
        string DeviceId { get; set; }
        string DeviceModel { get; set; }
        string DeviceVersion { get; set; }
        string MessageText { get; set; }
        
        string Lat { get; set; }
        string Lon { get; set; }
        DateTime CreatedDateTime { get; set; }
    }
}
