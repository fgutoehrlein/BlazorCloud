using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorCloud.Data
{
    public sealed class ConfigContainer
    {
        private static ConfigContainer instance = null;
        private static readonly object padlock = new object();
        private BlazorCloudCore.Models.ConfigBase Config { get; set; }

        ConfigContainer()
        {
            Config = JsonConvert.DeserializeObject<BlazorCloudCore.Models.ConfigBase>(File.ReadAllText(Path.Combine(Startup.WwwRootPath,"BasicConfig.json")));
            if (Config.UseBasePath)
            {
                Config.BasePath = Startup.WwwRootPath;
            }
            else
            {
                Config.BasePath = Config.CustomBasePath;
            }
        }

        public static ConfigContainer Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new ConfigContainer();
                    }
                    return instance;
                }
            }
        }
        public BlazorCloudCore.Models.ConfigBase GetConfigBase()
        {
            return this.Config;
        }
    }
}