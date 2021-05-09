using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace mi.identity.api
{
    public class Program
    {
        private static readonly AppSettings AppSettings = new AppSettings();

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var hostSettings = AppSettings.Common.Host;
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
