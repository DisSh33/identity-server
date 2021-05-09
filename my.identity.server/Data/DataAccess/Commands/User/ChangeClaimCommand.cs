using MediatR;
using mi.identity.server.Data.DataAccess.Responses;
using mi.identity.server.Models;
using mi.identity.server.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace mi.identity.server.Data.DataAccess.Commands.User
{
    public class ChangeClaimCommand : IRequest<UserClaimResponse>
    {
        public string UserId { get; set; }
        public ClaimViewModel Claim { get; set; }

        public ChangeClaimCommand()
        {
        }

        public ChangeClaimCommand(string userId, ClaimViewModel claim)
        {
            UserId = userId;
            Claim = claim;
        }
    }

    public class ChangeClaimHandler : IRequestHandler<ChangeClaimCommand, UserClaimResponse>
    {
        private readonly AppDbContext _appDbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMediator _mediator;

        public ChangeClaimHandler(
            AppDbContext appDbContext, 
            UserManager<ApplicationUser> userManager,
            IMediator mediator)
        {
            _appDbContext = appDbContext;
            _userManager = userManager;
            _mediator = mediator;
        }

        public async Task<UserClaimResponse> Handle(ChangeClaimCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);

            var claimToChange = _appDbContext.UserClaims.FirstOrDefault(c => c.ClaimType == request.Claim.Type && c.UserId == request.UserId);

            if (claimToChange != null)
            {
                await _userManager.ReplaceClaimAsync(user, claimToChange.ToClaim(), new Claim(request.Claim.Type, request.Claim.Value));
            }
            else
            {
                await _mediator.Send(new AddClaimCommand(request.UserId, request.Claim));
            }

            return new UserClaimResponse
            {
                Type = request.Claim.Type,
                Value = request.Claim.Value,
                User = new ProfileViewModel(user)
            };
        }
    }
}
