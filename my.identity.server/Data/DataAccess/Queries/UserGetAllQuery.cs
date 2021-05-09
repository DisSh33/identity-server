using MediatR;
using mi.identity.server.Data.DataAccess.Responses;
using mi.identity.server.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace mi.identity.server.Data.DataAccess.Queries
{
    public class UserGetAllQuery : IRequest<List<ProfileViewModel>>
    {

    }

    public class UserGetAllHandler : IRequestHandler<UserGetAllQuery, List<ProfileViewModel>>
    {
        private readonly AppDbContext _appDbContext;

        public UserGetAllHandler(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<ProfileViewModel>> Handle(UserGetAllQuery request, CancellationToken cancellationToken)
        {
            return _appDbContext.Users.Select(user => new ProfileViewModel(user)).ToList();
        }
    }
}
