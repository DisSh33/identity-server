using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;
using MediatR;
using mi.identity.server.Data.DataAccess.Responses;
using mi.identity.server.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace mi.identity.server.Data.DataAccess.Commands
{
    public class ClaimDeleteCommand : IRequest<ClaimResponse>
    {
        public string Type { get; set; }
        public string ApiResourceName { get; set; }
        public string IdentityResourceName { get; set; }

        public ClaimDeleteCommand()
        {
        }

        public ClaimDeleteCommand(string type)
        {
            Type = type;
        }

        public ClaimDeleteCommand(string type, string apiResourceName, string identityResourceName)
        {
            Type = type;
            ApiResourceName = apiResourceName;
            IdentityResourceName = identityResourceName;
        }
    }

    public class ClaimDeleteHandler : IRequestHandler<ClaimDeleteCommand, ClaimResponse>
    {
        private readonly ConfigurationDbContext _configurationDbContext;

        public DataAccessConstants DataAccessConstants { get; set; }

        public ClaimDeleteHandler(ConfigurationDbContext configurationDbContext)
        {
            _configurationDbContext = configurationDbContext;

            var appSettings = new AppSettings();
            DataAccessConstants = appSettings.DataAccessConstants;
        }

        public async Task<ClaimResponse> Handle(ClaimDeleteCommand request, CancellationToken cancellationToken)
        {
            var apiResource = await RemoveFromApiResources(request);
            var identityResource = await RemoveFromIdentityResources(request);

            return new ClaimResponse
            {
                Type = request.Type,
                ApiResource = new ResourceViewModel
                {
                    Id = apiResource.Id,
                    Name = apiResource.Name
                },
                IdentityResource = new ResourceViewModel
                {
                    Id = identityResource.Id,
                    Name = identityResource.Name
                }
            };
        }

        private async Task<ApiResource> RemoveFromApiResources(ClaimDeleteCommand request)
        {
            var apiResourceName = request.ApiResourceName;
            if (string.IsNullOrEmpty(request.ApiResourceName))
            {
                apiResourceName = DataAccessConstants.DefaultApiResourceName;
            }

            var apiResource =
                _configurationDbContext.ApiResources
                .Include(x => x.UserClaims)
                .FirstOrDefault(resource => resource.Name == apiResourceName);

            if (apiResource != null)
            {
                var apiClaims = apiResource.UserClaims ?? new List<ApiResourceClaim>();
                var claimToRemove = apiClaims.FirstOrDefault(claim => claim.Type == request.Type);

                if (claimToRemove != null)
                {
                    apiClaims.Remove(claimToRemove);

                    apiResource.UserClaims = apiClaims;

                    _configurationDbContext.Update(apiResource);
                    await _configurationDbContext.SaveChangesAsync();
                }
            }

            return apiResource;
        }

        private async Task<IdentityResource> RemoveFromIdentityResources(ClaimDeleteCommand request)
        {
            var identityResourceName = request.IdentityResourceName;
            if (string.IsNullOrEmpty(request.IdentityResourceName))
            {
                identityResourceName = DataAccessConstants.DefaultIdentityResourceName;
            }

            var identityResource =
                _configurationDbContext.IdentityResources
                .Include(x => x.UserClaims)
                .FirstOrDefault(resource => resource.Name == identityResourceName);

            if (identityResource != null)
            {
                var identityClaims = identityResource.UserClaims ?? new List<IdentityResourceClaim>();
                var claimToRemove = identityClaims.FirstOrDefault(claim => claim.Type == request.Type);

                if (claimToRemove != null)
                {
                    identityClaims.Remove(claimToRemove);

                    identityResource.UserClaims = identityClaims;

                    _configurationDbContext.Update(identityResource);
                    await _configurationDbContext.SaveChangesAsync();
                }
            }

            return identityResource;
        }
    }
}
