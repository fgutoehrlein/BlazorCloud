using BlazorCloud.Areas.Identity.Data;
using BlazorCloud.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorCloud.Areas.Authorization
{
    /// <summary>
    /// Class that holds logic to handle basic authentication requests.
    /// Primarily used as a injectable service. It holds its own UserManager instance.
    /// </summary>
    public class BasicAuthorization : IBasicAuthorization
    {
        private readonly UserManager<BlazorCloudUser> BasicAuthUserManager;
        public BasicAuthorization()
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
            BasicAuthUserManager = new UserManager<BlazorCloudUser>(store, null, new PasswordHasher<BlazorCloudUser>(), null, null, null, null, null, null);
        }
        /// <summary>
        /// Checks if the the given Basic Auth in the HttpContext is valid
        /// </summary>
        /// <returns>bool</returns>
        public async Task<bool> BasicAuthIsValid(HttpContext httpContext)
        {
            string authHeader = httpContext.Request.Headers["Authorization"];
            if (authHeader == null && !authHeader.StartsWith("Basic"))
            {
                return false;
            }

            string encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
            Encoding encoding = Encoding.GetEncoding("iso-8859-1");
            string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));
            int seperatorIndex = usernamePassword.IndexOf(':');

            var username = usernamePassword.Substring(0, seperatorIndex);
            var password = usernamePassword.Substring(seperatorIndex + 1);


            var blazorCloudUser = await BasicAuthUserManager.Users.SingleOrDefaultAsync(user => user.UserName == username);
            if (blazorCloudUser == null)
            {
                return false;
            }
            var passwordVerificationResult = BasicAuthUserManager.PasswordHasher.VerifyHashedPassword(blazorCloudUser, blazorCloudUser.PasswordHash, password);
            if (passwordVerificationResult == PasswordVerificationResult.Failed)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
