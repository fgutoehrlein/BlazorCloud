using BlazorCloudCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorCloudCore.Logic.Services
{
    public class UserSessionService
    {
        public PathChangedEventArgs currentEventArgs = new PathChangedEventArgs();
        public event EventHandler<PathChangedEventArgs> changeEvent;
        public UserSessionService()
        {
        }

        public class PathChangedEventArgs : EventArgs
        {
            public string DirectoryPath { get; set; }
        }
        public void Invoke()
        {
            if (currentEventArgs != null)
            {
                changeEvent?.Invoke(this, currentEventArgs);
            }
        }
        public string GetCurrentPath()
        {
            return currentEventArgs.DirectoryPath;
        }
    }
}
