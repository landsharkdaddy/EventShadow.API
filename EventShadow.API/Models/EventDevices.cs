using System;
using System.Collections.Generic;

namespace EventShadow.API.Models
{
    public partial class EventDevices
    {
        public long Id { get; set; }
        public long EventId { get; set; }
        public long EventShadowId { get; set; }

        public virtual Events Event { get; set; }
        public virtual EventShadowDevices EventShadow { get; set; }
    }
}
