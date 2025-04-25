using Api.Abstractions;
using DLS.Application.Features.Authentication.Commands.Login;
using DLS.Application.Features.Authentication.Commands.Register;
using DLS.Application.Features.Authentication.Queries.GetCurrentUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DLS.Presentation.Controllers
{
    [Route("api/v{version:apiVersion}/authentication")]
    public sealed class AuthenticationController : ApiController
    {
        public AuthenticationController(ISender sender) : base(sender)
        {
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginCommand command,
            CancellationToken cancellationToken = default)
        {
            var result = await Sender.Send(command, cancellationToken);
            return HandleResult(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(
            [FromBody] RegisterCommand command,
            CancellationToken cancellationToken = default)
        {
            var result = await Sender.Send(command, cancellationToken);
            return HandleResult(result);
        }

        [Authorize]
        [HttpGet("get-current-user")]
        public async Task<IActionResult> GetCurrentUser(
            CancellationToken cancellationToken = default)
        {
            var query = new GetCurrentUserQuery();
            var result = await Sender.Send(query, cancellationToken);
            return HandleResult(result);
        }
    }
}
