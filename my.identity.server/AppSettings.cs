using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using mi.identity.server.Data.DataAccess;
using mi.identity.server.ViewModels;
using Microsoft.Extensions.Configuration;

namespace mi.identity.server
{
    public class AppSettings
    {
        public readonly Common Common = new Common();
        public readonly DataAccessConstants DataAccessConstants = new DataAccessConstants();

        public IConfiguration Configuration { get; }

#if DEBUG
        private const string DefaultEnvironmentName = "debug";
#elif RELEASE
        private const string DefaultEnvironmentName = "release";
#endif

        public AppSettings()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                .AddJsonFile($"appsettings.{DefaultEnvironmentName}.json", optional: true, reloadOnChange: false);

            Configuration = builder.Build();
            Configuration.GetSection("Common").Bind(Common);

            Configuration.GetSection("DataAccessConstants").Bind(DataAccessConstants);
        }
        

        public IEnumerable<ApiScope> GetApisScope()
        {
            return new List<ApiScope>
            {
                new ApiScope { Name = "shared.scope" },
                new ApiScope { Name = "my.admin.scope" },
                new ApiScope { Name = "my.dashboard.scope" }
            };
        }

        public IEnumerable<ApiResource> GetApis()
        {
            return new List<ApiResource>
            {
                new ApiResource
                {
                    Name = "my.identity.api",
                    Scopes = { "shared.scope", "my.admin.scope" },
                    UserClaims = { "role", "full_access" }
                },
                new ApiResource
                {
                    Name = "my.dashboard.api",
                    Scopes = { "shared.scope", "my.dashboard.scope" },
                    UserClaims = { "role", "full_access" }
                }
            };
        }

        public IEnumerable<Client> GetClients() =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "my.identity.client.mvc",
                    ClientSecrets = {new Secret("secret_my.identity.client.mvc_secret".ToSha256())},

                    AllowedGrantTypes = GrantTypes.Code,
                    RequireConsent = false,
                    RequirePkce = true,

                    RedirectUris = {$"{Common.IdentityServer.Authority}/signin-oidc"},
                    PostLogoutRedirectUris = { $"{Common.IdentityServer.Authority}/signout-callback-oidc"},

                    AlwaysIncludeUserClaimsInIdToken = true,
                    AccessTokenLifetime = TimeSpan.FromDays(1).Seconds,
                    AllowOfflineAccess = true,

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "offline_access",
                        "shared.scope",
                        "my.admin.scope"
                    }
                },

                new Client
                {
                    ClientId = "my.identity.api.client",
                    ClientSecrets = {new Secret("secret_my.identity.api.client_secret".ToSha256())},

                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    AccessTokenLifetime = TimeSpan.FromDays(1).Seconds,
                    AllowOfflineAccess = true,

                     AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "offline_access",
                        "shared.scope",
                        "my.admin.scope"
                    }
                },

            };
               
        public IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResource
                {
                    Name = "my.identity_resourse",
                    UserClaims ={ "role", "full_access" },
                }
            };
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
