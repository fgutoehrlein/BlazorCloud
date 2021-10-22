using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BlazorCloudCore.Models
{
    public class DirectoryBase
    {
        public string DirectoryName { get; set; }
        public string DirectoryPath { get; set; }
        public List<DirectoryBase> Subdirectories { get; set; }
        public bool ShowSubdirectories { get; set; }
    }
}
