using System;
using BlazorCloud.Areas.Identity.Data;
using BlazorCloud.Data;
using BlazorCloudCore.Logic.SQLite;
using BlazorCloud.Areas.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

[assembly: HostingStartup(typeof(BlazorCloud.Areas.Identity.IdentityHostingStartup))]
namespace BlazorCloud.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                using (ISqliteSetup sqliteSetup = new SqliteSetup())
                {
                    var dbName = sqliteSetup.GetDatabaseNameFromConnectionString(context.Configuration.GetConnectionString("BlazorCloudContextConnection"));
                    sqliteSetup.CreateDatabaseIfNonExistant(dbName);
                }
                services.AddDbContext<BlazorCloudContext>(options =>
                    options.UseSqlite(
                        context.Configuration.GetConnectionString("BlazorCloudContextConnection")));
                services.AddAuthorization(options =>
                {
                    options.FallbackPolicy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .Build();
                });
                services.AddDefaultIdentity<BlazorCloudUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<BlazorCloudContext>();

                services.TryAddSingleton<IBasicAuthorization, BasicAuthorization>();
            });
        }
    }
}