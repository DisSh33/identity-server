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
    public class ClaimCreateCommand : IRequest<ClaimResponse>
    {
        public string Type { get; set; }
        public string ApiResourceName { get; set; }
        public string IdentityResourceName { get; set; }

        public ClaimCreateCommand()
        {
        }

        public ClaimCreateCommand(string type)
        {
            Type = type;
        }

        public ClaimCreateCommand(string type, string apiResourceName, string identityResourceName)
        {
            Type = type;
            ApiResourceName = apiResourceName;
            IdentityResourceName = identityResourceName;
        }
    }

    public class ClaimCreateHandler : IRequestHandler<ClaimCreateCommand, ClaimResponse>
    {
        private readonly ConfigurationDbContext _configurationDbContext;

        public DataAccessConstants DataAccessConstants { get; set; }

        public ClaimCreateHandler(ConfigurationDbContext configurationDbContext)
        {
            _configurationDbContext = configurationDbContext;

            var appSettings = new AppSettings();
            DataAccessConstants = appSettings.DataAccessConstants;
        }

        public async Task<ClaimResponse> Handle(ClaimCreateCommand request, CancellationToken cancellationToken)
        {
            var apiResource = await AddToApiResources(request);
            var identityResource = await AddToIdentityResources(request);

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

        private async Task<ApiResource> AddToApiResources(ClaimCreateCommand request)
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
                
                if (apiClaims.FirstOrDefault(claim => claim.Type == request.Type) == null)
                {
                    apiClaims.Add(new ApiResourceClaim
                    {
                        Type = request.Type,
                        ApiResourceId = apiResource.Id
                    });

                    apiResource.UserClaims = apiClaims;

                    _configurationDbContext.Update(apiResource);
                    await _configurationDbContext.SaveChangesAsync();
                }                
            }
            
            return apiResource;
        }

        private async Task<IdentityResource> AddToIdentityResources(ClaimCreateCommand request)
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

                if (identityClaims.FirstOrDefault(claim => claim.Type == request.Type) == null)
                {
                    identityClaims.Add(new IdentityResourceClaim()
                    {
                        Type = request.Type,
                        IdentityResourceId = identityResource.Id
                    });

                    identityResource.UserClaims = identityClaims;

                    _configurationDbContext.Update(identityResource);
                    await _configurationDbContext.SaveChangesAsync();
                }
            }

            return identityResource;
        }

    }
}
