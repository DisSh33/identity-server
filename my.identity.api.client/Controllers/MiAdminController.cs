using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;
using Newtonsoft.Json;

namespace mi.identity.api.client.Controllers
{
    [Route("mi")]
    [ApiController]
    public class MiAdminController : ControllerBase
    {
        public async Task<IActionResult> Index()
        {

            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;

            IdentityModelEventSource.ShowPII = true;

            using var serverClient = new HttpClient(clientHandler);

            string error           = "";
            string innerError      = "";
            string tokenError      = "";
            string responseContent = "";

            var tokenUrl = $"{AppSettings.Common.IdentityServer.Authority}/token/ClientCredentials";                     

            var model = new
            {
                ClientId = "my.identity.api.client",
                ClientSecret = "secret_my.identity.api.client_secret"
            };

            var stringContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var responseToken = await serverClient.PostAsync(tokenUrl, stringContent);

            var accessToken = await responseToken.Content.ReadAsStringAsync();

            try
            {
                using var apiClient = new HttpClient();

                apiClient.SetBearerToken(accessToken);
 
                var responseApi = await apiClient.GetAsync($"http://{AppSettings.Common.Api.IpAddress}:{AppSettings.Common.Api.HttpPort}/secret");

                responseContent = responseApi.StatusCode.ToString() + 
                    " --- ReasonPhrase: " + responseApi.ReasonPhrase + 
                    " --- Content: " + await responseApi.Content.ReadAsStringAsync() + 
                    " --- RequestMessage: " + responseApi.RequestMessage;
            }
            catch (Exception e)
            {                
                error = " --- ErrorMessage: " + e.Message + 
                        " --- StackTrase: " + e.StackTrace;

                innerError = " --- InnerErrorMessage: " + e.InnerException?.Message + 
                             " --- InnerErrorInfo" + e.InnerException.ToString();
            }

            return Ok(
                $"AccessToken: {accessToken}, {Environment.NewLine}" +
                $"TokenError: {tokenError}, {Environment.NewLine}" +
                $"Error: {error}, {Environment.NewLine}" +
                $"InnerErroor: {innerError}, {Environment.NewLine}" +
                $"ResponseFromApi: {responseContent}");
        }

        [Route("test")]
        [HttpGet]
        public IActionResult Test()
        {
            return Ok("OK");
        }
    }
}
