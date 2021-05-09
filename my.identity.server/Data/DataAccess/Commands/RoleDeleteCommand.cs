using MediatR;
using mi.identity.server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace mi.identity.server.Data.DataAccess.Commands
{
    public class RoleDeleteCommand : IRequest<IdentityRole>
    {
        public string RoleName { get; set; }
    }

    public class RoleDeleteHandler : IRequestHandler<RoleDeleteCommand, IdentityRole>
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _appDbContext;

        public RoleDeleteHandler(RoleManager<IdentityRole> roleManager, AppDbContext appDbContext)
        {
            _roleManager = roleManager;
            _appDbContext = appDbContext;
        }

        public async Task<IdentityRole> Handle(RoleDeleteCommand request, CancellationToken cancellationToken)
        {
            var roleToDelete = await _roleManager.FindByNameAsync(request.RoleName);
            var userWithRole = _appDbContext.UserRoles.Where(x => x.RoleId == roleToDelete.Id).ToList();

            if (roleToDelete != null && userWithRole.Count == 0)
            {
                await _roleManager.DeleteAsync(roleToDelete);
                return roleToDelete;
            }

            return null;
        }
    }
}
