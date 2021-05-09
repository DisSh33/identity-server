using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace mi.identity.server.Controllers
{
    public class HomeController : Controller
    {
        private IIdentityServerInteractionService _identityServerInteractionService;

        public HomeController(IIdentityServerInteractionService identityServerInteractionService)
        {
            _identityServerInteractionService = identityServerInteractionService;
        }

        [Route("error")]
        public async Task<object> Error(string errorId)
        {
            return await _identityServerInteractionService.GetErrorContextAsync(errorId);
        }
    }
}
