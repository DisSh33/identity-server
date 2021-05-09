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
    public class UserDeleteCommand : IRequest<ProfileViewModel>
    {
        public string UserId { get; set; }

        public UserDeleteCommand(string userId)
        {
            UserId = userId;
        }
    }

    public class UserDeleteHandler : IRequestHandler<UserDeleteCommand, ProfileViewModel>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserDeleteHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ProfileViewModel> Handle(UserDeleteCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            await _userManager.DeleteAsync(user);

            return new ProfileViewModel(user);
        }
    }
}
