using BlazorCloudCore.Logic.String;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BlazorCloudCore.Logic.Services
{
    public class UserConfigurationService
    {
        public string ConfigFilePath { get; set; }
        public string ConfigFileDirectory { get; set; }
        public Dictionary<string,string> UserPathConfig { get; set; }
        public UserConfigurationService(string configFilePath)
        {
            ConfigFilePath = configFilePath;
            ConfigFileDirectory = Path.GetDirectoryName(configFilePath);
            UserPathConfig = new Dictionary<string, string>();
            LoadConfiguration();
        }
        public void LoadConfiguration()
        {
            UserPathConfig = JsonConvert.DeserializeObject<Dictionary<string,string>>(File.ReadAllText(ConfigFilePath));
        }
        public string GetUserPath(string uid)
        {
            string path;
            UserPathConfig.TryGetValue(uid,out path);
            return path;
        }
        public async Task AddUserToConfigAsync(string uid)
        {
            if(UserPathConfig.TryGetValue(uid, out _))
            {
                return;
            }
            else
            {
                var directoryPath = Path.Combine(ConfigFileDirectory, uid);
                Directory.CreateDirectory(directoryPath);

                var stringFormater = new StringFormater();
                UserPathConfig.Add(uid, stringFormater.ConvertWindowsPathToUnix(Path.Combine(ConfigFileDirectory, uid)));
                var dictionaryJson = JsonConvert.SerializeObject(UserPathConfig);

                await File.WriteAllTextAsync(ConfigFilePath, dictionaryJson);
            }
        }
    }
}
