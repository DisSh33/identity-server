using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace mi.identity.server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var appSettings = new AppSettings();

            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {

                #region ConfigurationDbSeedScript

                //var context = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();

                //if (!context.Clients.Any())
                //{
                //    foreach (var client in appSettings.GetClients())
                //    {
                //        context.Clients.Add(client.ToEntity());
                //    }
                //    context.SaveChanges();
                //}

                //if (!context.IdentityResources.Any())
                //{
                //    foreach (var resource in appSettings.GetIdentityResources())
                //    {
                //        context.IdentityResources.Add(resource.ToEntity());
                //    }
                //    context.SaveChanges();
                //}

                //if (!context.ApiScopes.Any())
                //{
                //    foreach (var resource in appSettings.GetApisScope())
                //    {
                //        context.ApiScopes.Add(resource.ToEntity());
                //    }
                //    context.SaveChanges();
                //}

                //if (!context.ApiResources.Any())
                //{
                //    foreach (var resource in appSettings.GetApis())
                //    {
                //        context.ApiResources.Add(resource.ToEntity());
                //    }
                //    context.SaveChanges();
                //}

                #endregion
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var appSettings = new AppSettings();
            var hostSettings = appSettings.Common.Host;

            var urls = new List<string>()
            {
                $"http://{hostSettings.IpAddress}:{hostSettings.HttpPort}"
            };

            if (hostSettings.HttpsPort > 0)
            {
                urls.Add($"https://{hostSettings.IpAddress}:{hostSettings.HttpsPort}");
            }

            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseStartup<Startup>()
                        .UseUrls(urls.ToArray());
                });
        }
    }
}
