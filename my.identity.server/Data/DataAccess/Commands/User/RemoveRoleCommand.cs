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
    public class RemoveRoleCommand : IRequest<UserRoleResponse>
    {
        public string UserId { get; set; }
        public string RoleName { get; set; }

        public RemoveRoleCommand(string userId, string roleName)
        {
            UserId = userId;
            RoleName = roleName;
        }
    }

    public class RemoveRoleHandler : IRequestHandler<RemoveRoleCommand, UserRoleResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMediator _mediator;

        public DataAccessConstants DataAccessConstants { get; set; }

        public RemoveRoleHandler(
            UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole> roleManager,
            IMediator mediator)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mediator = mediator;

            var appSettings = new AppSettings();
            DataAccessConstants = appSettings.DataAccessConstants;
        }

        public async Task<UserRoleResponse> Handle(RemoveRoleCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            var role = await _roleManager.FindByNameAsync(request.RoleName);

            if (role != null)
            {
                await _userManager.RemoveFromRoleAsync(user, role.Name);

                await RemoveClaimsByRole(request);
            }

            return new UserRoleResponse
            {
                Role = role,
                User = new ProfileViewModel(user)
            };
        }

        private async Task RemoveClaimsByRole(RemoveRoleCommand request)
        {
            var permissions = DataAccessConstants.RolePermissions.FirstOrDefault(role => role.Key == request.RoleName);

            foreach (var permission in permissions.Value)
            {
                await _mediator.Send(new RemoveClaimCommand(request.UserId, new ClaimViewModel { Type = permission, Value = "true" }));
            }
        }
    }
}
