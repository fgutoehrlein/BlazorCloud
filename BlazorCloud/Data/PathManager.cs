using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BlazorCloudCore.Models;
using System.Text;

namespace BlazorCloud.Data
{
    public sealed class PathManager
    {
        private static PathManager instance = null;
        private static readonly object padlock = new object();
        private PathContainerBase Paths { get; set; }

        PathManager()
        {
            Paths = JsonConvert.DeserializeObject<PathContainerBase>(File.ReadAllText(Path.Combine(Startup.WwwRootPath, "BasicConfig.json")));
            if (Paths.UseBasePath)
            {
                Paths.BasePath = Startup.WwwRootPath;
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
                return Path.Combine(Instance.Paths.CustomBasePath);
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
        public async Task SaveChangesToJson(PathContainerBase pathContainer)
        {
            var pathContainerJson = JsonConvert.SerializeObject(pathContainer);

            await File.WriteAllTextAsync(Path.Combine(Startup.WwwRootPath, Paths.ConfigFileName),pathContainerJson);
        }
        public PathContainerBase GetPathsInstance()
        {
            return Paths;
        }
    }
}