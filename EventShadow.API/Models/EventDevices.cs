using System;
using System.Collections.Generic;

namespace EventShadow.API.Models
{
    public partial class EventDevices
    {
        public long Id { get; set; }
        public long EventId { get; set; }
        public long EventShadowId { get; set; }

        public Events Event { get; set; }
        public EventShadowDevices EventShadow { get; set; }
    }
}
