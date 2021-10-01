using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorCloudCore.Models
{
    public class UserSessionModel
    {
        public bool UserActive { get; set; }
        public string CurrentPath { get; set; }
    }
}
