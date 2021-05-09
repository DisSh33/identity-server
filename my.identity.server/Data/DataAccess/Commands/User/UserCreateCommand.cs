using MediatR;
using mi.identity.server.Data.DataAccess.Responses;
using mi.identity.server.Models;
using mi.identity.server.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace mi.identity.server.Data.DataAccess.Commands.User
{
    public class UserCreateCommand : IRequest<UserResponse>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public ProfileViewModel UserProfile { get; set; }
        public string RoleName { get; set; }
    }

    public class UserCreateHandler : IRequestHandler<UserCreateCommand, UserResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMediator _mediator;
        public UserCreateHandler(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IMediator mediator)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mediator = mediator;
        }

        public async Task<UserResponse> Handle(UserCreateCommand request, CancellationToken cancellationToken)
        {
            var user = new ApplicationUser
            {
                UserName = request.UserName,
                FirstName = request.UserProfile.FirstName,
                LastName = request.UserProfile.LastName,
                Email = request.UserProfile.Email
            };

            var createResult = await _userManager.CreateAsync(user, request.Password);

            string roleName = request.RoleName ?? "Basic"; ///// DEFAULT ROLE 

            if (createResult.Succeeded)
            {
                await _mediator.Send(new AssignRoleCommand(user.Id, roleName));

                await _mediator.Send( new AddClaimCommand(user.Id, new ClaimViewModel { Type = "user_name", Value = user.UserName }) );
                await _mediator.Send( new AddClaimCommand(user.Id, new ClaimViewModel { Type = "first_name", Value = user.FirstName }) );
                await _mediator.Send( new AddClaimCommand(user.Id, new ClaimViewModel { Type = "last_name", Value = user.LastName }) );
                await _mediator.Send( new AddClaimCommand(user.Id, new ClaimViewModel { Type = "email", Value = user.Email }) );
                await _mediator.Send( new AddClaimCommand(user.Id, new ClaimViewModel { Type = "role", Value = roleName }) );

                return new UserResponse
                {
                    User = new ProfileViewModel(user),
                    UserRole = await _roleManager.FindByNameAsync(roleName)
                };
            }

            return new UserResponse();
        }
    }
}
