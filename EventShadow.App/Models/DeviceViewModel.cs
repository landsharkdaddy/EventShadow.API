using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventShadow.App.Models
{
    public class DeviceViewModel
    {
        public Event eventInfo { get; set; }
        public List<Device> devicesFound { get; set; }
    }
}