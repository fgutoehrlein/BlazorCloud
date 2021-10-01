using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorCloud.Data
{
    public sealed class PathManager
    {
        private static PathManager instance = null;
        private static readonly object padlock = new object();
        private BlazorCloudCore.Models.PathContainerBase Paths { get; set; }

        PathManager()
        {
            Paths = JsonConvert.DeserializeObject<BlazorCloudCore.Models.PathContainerBase>(File.ReadAllText(Path.Combine(Startup.WwwRootPath,"BasicConfig.json")));
            if (Paths.UseBasePath)
            {
                Paths.BasePath = Startup.WwwRootPath;
            }
            else
            {
                Paths.BasePath = Paths.CustomBasePath;
            }
        }

        public static PathManager Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new PathManager();
                    }
                    return instance;
                }
            }
        }
        public BlazorCloudCore.Models.PathContainerBase GetConfigBase()
        {
            return this.Paths;
        }
        public string GetFullFilePath()
        {
            if (Instance.Paths.UseBasePath)
            {
                return Path.Combine(Instance.Paths.BasePath, Instance.Paths.FilePath);
            }
            else
            {
                return Path.Combine(Instance.Paths.CustomBasePath, Instance.Paths.FilePath);
            }
        }
        public string GetFullConfigPath()
        {
            if (Instance.Paths.UseBasePath)
            {
                return Path.Combine(Instance.Paths.BasePath, Instance.Paths.ConfigPath);
            }
            else
            {
                return Path.Combine(Instance.Paths.CustomBasePath, Instance.Paths.ConfigPath);
            }
        }
    }
}