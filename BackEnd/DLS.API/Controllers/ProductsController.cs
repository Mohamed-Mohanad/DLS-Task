using Api.Abstractions;
using DLS.Application.Features.Products.Commands.CreateProduct;
using DLS.Application.Features.Products.Commands.DeleteProduct;
using DLS.Application.Features.Products.Commands.UpdateProduct;
using DLS.Application.Features.Products.Queries.GetProduct;
using DLS.Application.Features.Products.Queries.GetProductsList;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DLS.Presentation.Controllers
{
    [Route("api/v{version:apiVersion}/products")]
    public sealed class ProductsController : ApiController
    {
        public ProductsController(ISender sender) : base(sender)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] long? categoryId = null,
            CancellationToken cancellationToken = default)
        {
            var query = new GetProductsListQuery()
            {
                Page = page,
                PageSize = pageSize,
                CategoryId = categoryId
            };
            var result = await Sender.Send(query, cancellationToken);
            return HandleResult(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(
            long id,
            CancellationToken cancellationToken = default)
        {
            var query = new GetProductQuery()
            {
                Id = id
            };
            var result = await Sender.Send(query, cancellationToken);
            return HandleResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateProductCommand command,
            CancellationToken cancellationToken = default)
        {
            var result = await Sender.Send(command, cancellationToken);
            return HandleResult(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(
            [FromBody] UpdateProductCommand command,
            CancellationToken cancellationToken = default)
        {
            var result = await Sender.Send(command, cancellationToken);
            return HandleResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(
            long id,
            CancellationToken cancellationToken = default)
        {
            var command = new DeleteProductCommand()
            {
                Id = id
            };
            var result = await Sender.Send(command, cancellationToken);
            return HandleResult(result);
        }
    }
}