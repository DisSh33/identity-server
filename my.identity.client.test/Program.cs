using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace mi.identity.client.test
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var host = CreateHostBuilder(args)
                .ConfigureLogging((hostContext, loggingBuilder) =>
                {
                    var logger = new LoggerConfiguration()
                    .Enrich.FromLogContext()
                    .MinimumLevel.Debug()
                    .WriteTo.File("LogFile.txt")
                    .CreateLogger();

                    loggingBuilder
                    .AddSerilog(logger)
                    .AddConsole()
                    .AddDebug();
                }).Build();

            host.Run();
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
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder

                        .UseStartup<Startup>()
                        .UseUrls(urls.ToArray());
                });
        }
    }
}
