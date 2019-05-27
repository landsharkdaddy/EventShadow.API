using System;
using System.Collections.Generic;

namespace EventShadow.API.Models
{
    public partial class Devices
    {
        public long Id { get; set; }
        public string BluetoothAddress { get; set; }
        public DateTime? Timestamp { get; set; }
        public string AdvertisementType { get; set; }
        public string Rssi { get; set; }
        public string LocalName { get; set; }
        public string ManufacturerDataString { get; set; }
        public int SonarDeviceId { get; set; }
        public int EventId { get; set; }

        public EventShadowDevices SonarDevice { get; set; }
    }
}
