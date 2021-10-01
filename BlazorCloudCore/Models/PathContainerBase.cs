using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BlazorCloudCore.Models
{
    public class PathContainerBase
    {
        public string ConfigPath { get; set; }
        public string FilePath { get; set; }
        public bool UseBasePath { get; set; }
        public string CustomBasePath { get; set; }
        public string ReadableDataResourcesFile { get; set; }
        public string IconResources { get; set; }
        [JsonIgnore]
        public string BasePath { get; set; }
    }
}
