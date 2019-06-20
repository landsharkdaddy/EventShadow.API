using System;
using System.Collections.Generic;

namespace EventShadow.API.Models
{
    public partial class EventShadowDevices
    {
        public EventShadowDevices()
        {
            Devices = new HashSet<Devices>();
            EventDevices = new HashSet<EventDevices>();
        }

        public long Id { get; set; }
        public string DeviceName { get; set; }

        public ICollection<Devices> Devices { get; set; }
        public ICollection<EventDevices> EventDevices { get; set; }
    }
}
