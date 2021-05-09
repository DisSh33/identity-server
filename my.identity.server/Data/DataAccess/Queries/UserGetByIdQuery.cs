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
    public class UserGetByIdQuery : IRequest<ProfileViewModel>
    {
        public string UserId { get; set; }

        public UserGetByIdQuery(string userId)
        {
            UserId = userId;
        }
    }

    public class UserGetByIdHandler : IRequestHandler<UserGetByIdQuery, ProfileViewModel>
    {
        private readonly AppDbContext _appDbContext;

        public UserGetByIdHandler(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<ProfileViewModel> Handle(UserGetByIdQuery request, CancellationToken cancellationToken)
        {
            return new ProfileViewModel(_appDbContext.Users.FirstOrDefault(user => user.Id == request.UserId));
        }
    }
}
