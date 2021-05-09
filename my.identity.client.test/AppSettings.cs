using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace mi.identity.client.test
{
    public static class AppSettings
    {
        public static readonly Common Common = new Common();

        public static IConfiguration Configuration { get; }

#if DEBUG
        private const string DefaultEnvironmentName = "debug";
#elif RELEASE
        private const string DefaultEnvironmentName = "release";
#endif

        static AppSettings()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                .AddJsonFile($"appsettings.{DefaultEnvironmentName}.json", optional: true, reloadOnChange: false);

            Configuration = builder.Build();

            Configuration.GetSection("Common").Bind(Common);
        }
    }

    public class Common
    {
        public HostSettings Host { get; set; }
        public HostSettings Api { get; set; }
        public IdentitySettings IdentityServer { get; set; }
    }

    public class HostSettings
    {
        public string IpAddress { get; set; }
        public int HttpPort { get; set; }
        public int HttpsPort { get; set; }
        public string Root { get; set; }
    }

    public class IdentitySettings
    {
        public string Authority { get; set; }
    }
}