using BlazorCloud.Areas.Identity;
using BlazorCloud.Data;
using BlazorCloudCore.Logic.SQLite;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MudBlazor.Services;
using BlazorCloudCore.Models.Events;
using BlazorCloudCore.Logic.Services;
using BlazorCloud.Areas.Authorization;
using Microsoft.Extensions.DependencyInjection.Extensions;
using BlazorCloud.Areas.Identity.Data;

namespace BlazorCloud
{
    public class Startup
    {
        public static string WwwRootPath;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMudServices();
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.TryAddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<Areas.Identity.Data.BlazorCloudUser>>();

            services.TryAddSingleton<UserActivityChannelService>();
            services.AddSingleton<FileAndDirectoryService>();

            services.TryAddScoped<UserSessionService>();
            services.AddSwaggerGen();
            SeedRoles(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            WwwRootPath = env.WebRootPath;
            _=PathManager.Instance;
            _=SuffixContainer.Instance;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
            SeedAdminUser();
        }
        private void SeedRoles(IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            //initializing custom roles 
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roleNames = { IdentityRoles.Admin, IdentityRoles.Manager, IdentityRoles.Member };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = RoleManager.RoleExistsAsync(roleName).Result;
                if (!roleExist)
                {
                    //create the roles and seed them to the database: Question 1
                    roleResult = RoleManager.CreateAsync(new IdentityRole(roleName)).Result;
                }
            }
        }
        private void SeedAdminUser()
        {
            _ = new SytemAdminUser().SeedData();
        }
    }
}
