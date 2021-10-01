using BlazorCloudCore.Models.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;

namespace BlazorCloudCore.Logic.Services
{
    public class UserActivityChannelService
    {
        public Channel<UserActivityEvent> ActivitChannel { get; set; }
        public UserActivityChannelService()
        {
            ActivitChannel = Channel.CreateUnbounded<UserActivityEvent>();
        }
    }
}
