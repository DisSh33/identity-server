using System.Threading.Tasks;
using MediatR;
using mi.identity.server.Data.DataAccess.Commands;
using mi.identity.server.Data.DataAccess.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace mi.identity.server.Controllers
{
    [Route("api/claim")]
    [ApiController]
    [Authorize]
    public class ClaimController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ClaimController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("")]
        public async Task<ClaimResponse> Create([FromBody]ClaimCreateCommand command)
        {
            return await _mediator.Send(command);
        }
        
        [HttpDelete]
        [Route("")]
        public async Task<ClaimResponse> Delete([FromBody]ClaimDeleteCommand command)
        {
            return await _mediator.Send(command);
        }
    }
}
