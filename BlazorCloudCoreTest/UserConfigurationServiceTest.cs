using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using BlazorCloudCore.Logic.Services;
using System.Threading.Tasks;
using BlazorCloudCore.Logic.String;

namespace BlazorCloudCoreTest
{
    [TestClass]
    public class UserConfigurationServiceTest
    {
        public void PrepareTest()
        {
            var jsonDictionary = new Dictionary<string, string>();

            var stringFormater = new StringFormater();

            jsonDictionary.Add("TestUser",
                stringFormater.ConvertWindowsPathToUnix(Path.Combine(Directory.GetCurrentDirectory(),"TestUserDirectory")));
            jsonDictionary.Add("AnotherTestUser", stringFormater.ConvertWindowsPathToUnix(
                Path.Combine(Directory.GetCurrentDirectory(), "AnotherTestUserDirectory")));
            jsonDictionary.Add("SomeOtherTestUser",
                stringFormater.ConvertWindowsPathToUnix(Path.Combine(Directory.GetCurrentDirectory(), "SomeOtherTestUserDirectory")));
            jsonDictionary.Add("DifferentUser",
                stringFormater.ConvertWindowsPathToUnix(Path.Combine(Directory.GetCurrentDirectory(), "DifferentUserDirectory")));

            var serializedJsonDictionary = JsonConvert.SerializeObject(jsonDictionary);
            File.WriteAllText(Path.GetFullPath("./UserConfiguration.json"), serializedJsonDictionary);
        }
        [TestMethod]
        public void LoadConfigurationTest()
        {
            PrepareTest();
            var userService = new UserConfigurationService(Path.GetFullPath("./UserConfiguration.json"));

            Assert.IsTrue(userService.UserPathConfig.Count == 4 ? true : false);
        }
        [TestMethod]
        public async Task AddUserToConfigAsyncTest()
        {
            PrepareTest();

            var userService = new UserConfigurationService(Path.GetFullPath("./UserConfiguration.json"));
            await userService.AddUserToConfigAsync("TestUser2");

            var deserializedJson = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(Path.GetFullPath("./UserConfiguration.json")));
            Assert.IsTrue(deserializedJson.TryGetValue("TestUser2", out _));
            File.Delete(Path.GetFullPath("./UserConfiguration.json"));
        }
        [TestMethod]
        public void GetUserPathTest()
        {
            PrepareTest();

            var userService = new UserConfigurationService(Path.GetFullPath("./UserConfiguration.json"));
            var userPath = userService.GetUserPath("TestUser");

            var stringFormater = new StringFormater();


            Assert.IsTrue(userPath == stringFormater.ConvertWindowsPathToUnix(
                Path.Combine(Directory.GetCurrentDirectory(), "TestUserDirectory")) ? true : false);
            File.Delete(Path.GetFullPath("./UserConfiguration.json"));
        }
    }
}
