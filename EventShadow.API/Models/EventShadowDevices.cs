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

        public virtual ICollection<Devices> Devices { get; set; }
        public virtual ICollection<EventDevices> EventDevices { get; set; }
    }
}
