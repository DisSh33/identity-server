using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace mi.identity.api
{
    public class AppSettings
    {
        public readonly IConfigurationRoot Configuration;
        public readonly Common Common;

#if DEBUG
        private const string DefaultEnvironmentName = "debug";
#elif RELEASE
        private const string DefaultEnvironmentName = "release";
#endif

        public AppSettings()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .AddJsonFile($"appsettings.{DefaultEnvironmentName}.json", optional: true, reloadOnChange: false);

            Configuration = builder.Build();

            var сommon = new Common();
            Configuration.GetSection("Common").Bind(сommon);
            Common = сommon;
        }
    }
    public class Common
    {
        public HostSettings Host { get; set; }
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
