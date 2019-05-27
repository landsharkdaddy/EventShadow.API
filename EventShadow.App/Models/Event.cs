using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventShadow.App.Models
{
    public class Event
    {
        public int id { get; set; }
        public string eventName { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public string market { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public List<Device> eventDevices { get; set; }
    }
}
