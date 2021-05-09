using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using mi.identity.server.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace mi.identity.server.Controllers
{
    [Route("token")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        [HttpPost]
        [Route("ClientCredentials")]
        public async Task<string> GetClientCredentialsToken(ClientCredentialsViewModel clientCredentials)
        {
            using var serverClient = new HttpClient();
            var appSettings = new AppSettings();
                        
            var authorityUrl = appSettings.Common.IdentityServer.Authority;

            var discoveryDocument = await serverClient.GetDiscoveryDocumentAsync(authorityUrl);

            var tokenResponse = await serverClient.RequestClientCredentialsTokenAsync(
                new ClientCredentialsTokenRequest
                {
                    RequestUri = new Uri(discoveryDocument.TokenEndpoint),
                    GrantType = "client_credentials",

                    ClientId = clientCredentials.ClientId,
                    ClientSecret = clientCredentials.ClientSecret,

                    Scope = clientCredentials.Scope ?? "my.admin.scope",                   
                });

            return tokenResponse.AccessToken;
        }
    }
}
