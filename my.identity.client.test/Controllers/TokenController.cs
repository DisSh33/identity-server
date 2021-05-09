using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace mi.identity.client.test.Controllers
{
    public class TokenController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public TokenController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var idToken = await HttpContext.GetTokenAsync("id_token");

            if (accessToken != null && idToken != null)
            {
                var _claims = User.Claims.ToList();
                var _accessToken = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
                var _idToken = new JwtSecurityTokenHandler().ReadJwtToken(idToken);
            }

            var apiClient = _httpClientFactory.CreateClient();

            apiClient.SetBearerToken(accessToken);

            var response = await apiClient.GetAsync($"https://{AppSettings.Common.Api.IpAddress}:{AppSettings.Common.Api.HttpsPort}/secret");

            var content = await response.Content.ReadAsStringAsync();

            var output = new OutputViewModel
            {
                Content = content
            };

            return View(output);
        }
    }
}
