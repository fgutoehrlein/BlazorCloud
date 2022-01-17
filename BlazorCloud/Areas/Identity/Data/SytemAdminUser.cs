using BlazorCloud.Areas.Identity.Data;
using BlazorCloud.Data;
using BlazorCloudCore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BlazorCloud.Areas.Identity.Data
{
    public class SytemAdminUser
    {
        public SytemAdminUser()
        {
            var configUser = JsonConvert.DeserializeObject<UserBase>(File.ReadAllText(Path.Combine(Startup.WwwRootPath, "identitySeed.json")));
            Email = configUser.Email;
            Username = configUser.Username;
            Password = configUser.Password;
        }

        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public async Task SeedData()
        {
            var userManager = GetUserManager();            

            if(await userManager.FindByEmailAsync(Email) != null)
            {
                return;
            }

            var user = new BlazorCloudUser()
            {
                Email = Email,
                NormalizedEmail = Email.ToUpper(),
                UserName = Username,
                NormalizedUserName = Username.ToUpper(),
                EmailConfirmed = true,
                LockoutEnabled = false
            };

            if(!(await userManager.CreateAsync(user, Password)).Succeeded)
            {
                return;
            }

            var createdUser = await userManager.FindByEmailAsync(user.Email);
            await userManager.AddToRoleAsync(createdUser, IdentityRoles.Admin);

        }
        public UserManager<BlazorCloudUser> GetUserManager()
        {
            IConfigurationRoot configurationRoot;

            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder().SetBasePath(Environment.CurrentDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            configurationRoot = configurationBuilder.Build();

            string connectionString = ConfigurationExtensions.GetConnectionString(configurationRoot, "BlazorCloudContextConnection");

            var optionsBuilder = new DbContextOptionsBuilder<BlazorCloudContext>();
            optionsBuilder.UseSqlite(connectionString);
            var dbContext = new BlazorCloudContext(optionsBuilder.Options);

            IUserStore<BlazorCloudUser> store = new UserStore<BlazorCloudUser>(dbContext);
            var userManager = new UserManager<BlazorCloudUser>(store, null, new PasswordHasher<BlazorCloudUser>(), null, null, null, null, null, null);

            return userManager;
        }
    }
}
