using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorCloudCore.Models.Events
{
    public class UserActivityEvent
    {
        string UserId { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
