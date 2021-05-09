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
    public class RemoveClaimCommand : IRequest<UserClaimResponse>
    {
        public string UserId { get; set; }
        public ClaimViewModel Claim { get; set; }

        public RemoveClaimCommand()
        {
        }

        public RemoveClaimCommand(string userId, ClaimViewModel claim)
        {
            UserId = userId;
            Claim = claim;
        }
    }

    public class RemoveClaimHandler : IRequestHandler<RemoveClaimCommand, UserClaimResponse>
    {
        private readonly AppDbContext _appDbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public RemoveClaimHandler( AppDbContext appDbContext, UserManager<ApplicationUser> userManager)
        {
            _appDbContext = appDbContext;
            _userManager = userManager;
        }

        public async Task<UserClaimResponse> Handle(RemoveClaimCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            var claimToRemove = _appDbContext.UserClaims.FirstOrDefault(c => c.ClaimType == request.Claim.Type && c.UserId == request.UserId);

            if (claimToRemove != null)
            {
                await _userManager.RemoveClaimAsync(user, claimToRemove.ToClaim());
            }

            return new UserClaimResponse
            {
                Type = claimToRemove?.ClaimType,
                Value = claimToRemove?.ClaimValue,
                User = new ProfileViewModel(user)
            };
        }


    }
}
