using System;
using System.Collections.Generic;

namespace EventShadow.API.Models
{
    public partial class Devices
    {
        public long Id { get; set; }
        public string BluetoothAddress { get; set; }
        public DateTime? TimeStamp { get; set; }
        public string Advertisement { get; set; }
        public string Rssi { get; set; }
        public string LocalName { get; set; }
        public string ManufacturerDataString { get; set; }
        public long? SonarDeviceId { get; set; }
        public long? EventId { get; set; }
        public string TimeStampDate { get; set; }
        public string TimeStampHour { get; set; }

        public virtual Events Event { get; set; }
        public virtual EventShadowDevices SonarDevice { get; set; }
    }
}
