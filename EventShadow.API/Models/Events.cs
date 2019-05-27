using System;
using System.Collections.Generic;

namespace EventShadow.API.Models
{
    public partial class Events
    {
        public Events()
        {
            EventDevices = new HashSet<EventDevices>();
        }

        public long Id { get; set; }
        public string EventName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Market { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }

        public ICollection<EventDevices> EventDevices { get; set; }
    }
}
