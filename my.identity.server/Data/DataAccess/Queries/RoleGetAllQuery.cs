using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace mi.identity.server.Data.DataAccess.Queries
{
    public class RoleGetAllQuery : IRequest<List<IdentityRole>>
    {
    }

    public class RoleGetAllHandler : IRequestHandler<RoleGetAllQuery, List<IdentityRole>>
    {
        private readonly AppDbContext _appDbContext;

        public RoleGetAllHandler(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<IdentityRole>> Handle(RoleGetAllQuery request, CancellationToken cancellationToken)
        {
            return _appDbContext.Roles.ToList();
        }
    }
}
