using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using mi.identity.server.Data;
using mi.identity.server.Data.DataAccess.Commands;
using mi.identity.server.Data.DataAccess.Queries;
using mi.identity.server.Models;
using mi.identity.server.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace mi.identity.server.Controllers
{
    [Route("api/role")]
    [ApiController]
    [Authorize]
    public class RoleController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RoleController( IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("")]
        public async Task<List<IdentityRole>> GetAllRolesAsync()
        {
            return await _mediator.Send(new RoleGetAllQuery());
        }

        //[HttpGet]
        //[Route("userRoles")]
        //public async Task<List<IdentityRole>> GetUserRoles(string userId)
        //{
        //    var user = await _userManager.FindByIdAsync(userId);
        //    var pairList = _context.UserRoles.Where(x => x.UserId == user.Id).ToList();

        //    var output = new List<IdentityRole>();
        //    foreach (var pair in pairList)
        //    {
        //        var role = await _roleManager.FindByIdAsync(pair.RoleId);
        //        output.Add(role);
        //    }

        //    return output;
        //}

        //[HttpGet]
        //[Route("usersWithRole")]
        //public async Task<List<ApplicationUser>> GetUsersWithRole(string roleName)
        //{
        //    var role = await _roleManager.FindByNameAsync(roleName);
        //    var pairList = _context.UserRoles.Where(x => x.RoleId == role.Id).ToList();

        //    var output = new List<ApplicationUser>();
        //    foreach (var pair in pairList)
        //    {
        //        var user = await _userManager.FindByIdAsync(pair.UserId);
        //        output.Add(user);
        //    }

        //    return output;
        //}

        [HttpPost]
        [Route("")]
        public async Task<IdentityRole> Create([FromBody]RoleCreateCommand command)
        {
            return await _mediator.Send(command);
        }
        
        [HttpDelete]
        [Route("")]
        public async Task<IdentityRole> Delete([FromBody]RoleDeleteCommand command)
        {
            return await _mediator.Send(command);
        }
    }
}
