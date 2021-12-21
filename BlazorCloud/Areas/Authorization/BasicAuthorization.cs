using BlazorCloud.Areas.Identity.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorCloud.Areas.Authorization
{
    public class BasicAuthorization : IBasicAuthorization
    {
        public UserManager<BlazorCloudUser> _userManager;
        public BasicAuthorization(UserManager<BlazorCloudUser> userManager)
        {
            _userManager = userManager;
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


            var blazorCloudUser = await _userManager.Users.SingleOrDefaultAsync(user => user.UserName == username);
            if (blazorCloudUser == null)
            {
                return false;
            }
            var passwordVerificationResult = _userManager.PasswordHasher.VerifyHashedPassword(blazorCloudUser, blazorCloudUser.PasswordHash, password);
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
