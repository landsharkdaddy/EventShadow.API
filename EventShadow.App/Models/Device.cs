using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventShadow.App.Models
{
    public class Device
    {
        public string BluetoothAddress { get; set; }
        public DateTimeOffset timestamp { get; set; }
        public string AdvertisementType { get; set; }
        public short RSSI { get; set; }
        public string localName { get; set; }
        public string manufacturerDataString { get; set; }
        public int DeviceID { get; set; }
        public int EventID { get; set; }
    }
}
