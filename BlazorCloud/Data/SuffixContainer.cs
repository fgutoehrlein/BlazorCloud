using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorCloud.Data
{
    public sealed class SuffixContainer
    {
        private static SuffixContainer instance = null;
        private static readonly object padlock = new object();
        private List<string> SuffixList { get; set; }

        SuffixContainer()
        {
            var config = PathManager.Instance.GetConfigBase();
            var suffixFilePath = Path.Combine(Startup.WwwRootPath, config.ConfigPath, config.ReadableDataResourcesFile);
            SuffixList = JsonConvert.DeserializeObject<BlazorCloudCore.Models.DataSuffixBase>(File.ReadAllText(suffixFilePath)).DataSuffix;
        }

        public static SuffixContainer Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new SuffixContainer();
                    }
                    return instance;
                }
            }
        }
        public List<string> GetSuffixList()
        {
            return SuffixList;
        }
    }
}