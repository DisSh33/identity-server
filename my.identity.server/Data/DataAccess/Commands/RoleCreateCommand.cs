using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace mi.identity.server.Data.DataAccess.Commands
{
    public class RoleCreateCommand : IRequest<IdentityRole>
    {
        public string RoleName { get; set; }

        public RoleCreateCommand()
        {
        }

        public RoleCreateCommand(string roleName)
        {
            RoleName = roleName;
        }
    }

    public class RoleCreateHandler : IRequestHandler<RoleCreateCommand, IdentityRole>
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMediator _mediator;

        public DataAccessConstants DataAccessConstants { get; set; }

        public RoleCreateHandler(RoleManager<IdentityRole> roleManager, IMediator mediator)
        {
            _roleManager = roleManager;
            _mediator = mediator;

            var appSettings = new AppSettings();
            DataAccessConstants = appSettings.DataAccessConstants;
        }

        public async Task<IdentityRole> Handle(RoleCreateCommand request, CancellationToken cancellationToken)
        {
            var role = await _roleManager.FindByNameAsync(request.RoleName);

            if (role == null)
            {
                await _roleManager.CreateAsync(new IdentityRole(request.RoleName));

                await CreateClaimsByRole(request.RoleName);
            }
            return await _roleManager.FindByNameAsync(request.RoleName);
        }

        private async Task CreateClaimsByRole(string roleName)
        {
            var permissions = DataAccessConstants.RolePermissions.FirstOrDefault(role => role.Key == roleName);

            foreach (var permission in permissions.Value)
            {
                await _mediator.Send(new ClaimCreateCommand(permission));
            }
        }
    }   
}
