using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using mi.identity.server.Data.DataAccess.Commands.User;
using mi.identity.server.Data.DataAccess.Queries;
using mi.identity.server.Data.DataAccess.Responses;
using mi.identity.server.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace mi.identity.server.Controllers
{
    [Route("api/user")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{userId}")]
        public async Task<ProfileViewModel> GetById([FromRoute]string userId)
        {
            return await _mediator.Send(new UserGetByIdQuery(userId));
        }

        [HttpGet]
        [Route("")]
        public async Task<List<ProfileViewModel>> GetAll()
        {
            return await _mediator.Send(new UserGetAllQuery());
        }

        [HttpPost]
        [Route("")]
        public async Task<UserResponse> Create([FromBody]UserCreateCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpDelete]
        [Route("{userId}")]
        public async Task<ProfileViewModel> Delete([FromRoute] string userId)
        {
            return await _mediator.Send(new UserDeleteCommand(userId));
        }

        [HttpPost]
        [Route("{userId}/role")]
        public async Task<UserRoleResponse> AddRole([FromRoute]string userId, string roleName)
        {
            return await _mediator.Send(new AssignRoleCommand(userId, roleName));
        }

        [HttpDelete]
        [Route("{userId}/role")]
        public async Task<UserRoleResponse> RemoveRole([FromRoute]string userId, string roleName)
        {
            return await _mediator.Send(new RemoveRoleCommand(userId, roleName));
        }

        [HttpPost]
        [Route("{userId}/claim")]
        public async Task<UserClaimResponse> AddClaim([FromRoute]string userId, [FromBody]ClaimViewModel claim)
        {
            return await _mediator.Send(new AddClaimCommand(userId, claim));
        }

        [HttpPut]
        [Route("{userId}/claim")]
        public async Task<UserClaimResponse> ChangeClaim([FromRoute]string userId,[FromBody] ClaimViewModel claim)
        {
            return await _mediator.Send(new ChangeClaimCommand(userId, claim));
        }

        [HttpDelete]
        [Route("{userId}/claim")]
        public async Task<UserClaimResponse> RemoveClaim([FromRoute]string userId, [FromBody]ClaimViewModel claim)
        {
            return await _mediator.Send(new RemoveClaimCommand(userId, claim));
        }
    }
}
