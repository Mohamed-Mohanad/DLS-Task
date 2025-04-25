using Api.Abstractions;
using DLS.Application.Features.Cart.Commands.AddToCart;
using DLS.Application.Features.Cart.Commands.RemoveFromCart;
using DLS.Application.Features.Cart.Queries.GetCart;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DLS.Presentation.Controllers
{
    [Authorize]
    [Route("api/v{version:apiVersion}/cart")]
    public sealed class CartController : ApiController
    {
        public CartController(ISender sender) : base(sender)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetCart(
            CancellationToken cancellationToken = default)
        {
            var query = new GetCartQuery();
            var result = await Sender.Send(query, cancellationToken);
            return HandleResult(result);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToCart(
            [FromBody] AddToCartCommand command,
            CancellationToken cancellationToken = default)
        {
            var result = await Sender.Send(command, cancellationToken);
            return HandleResult(result);
        }

        [HttpPost("remove")]
        public async Task<IActionResult> RemoveFromCart(
            [FromBody] RemoveFromCartCommand command,
            CancellationToken cancellationToken = default)
        {
            var result = await Sender.Send(command, cancellationToken);
            return HandleResult(result);
        }
    }
} 