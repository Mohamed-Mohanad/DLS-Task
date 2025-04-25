using Api.Abstractions;
using DLS.Application.Features.Categories.Commands.CreateCategory;
using DLS.Application.Features.Categories.Commands.DeleteCategory;
using DLS.Application.Features.Categories.Commands.UpdateCategory;
using DLS.Application.Features.Categories.Queries.GetCategoriesList;
using DLS.Application.Features.Categories.Queries.GetCategory;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DLS.Presentation.Controllers
{
    [Authorize]
    [Route("api/v{version:apiVersion}/categories")]
    public sealed class CategoriesController : ApiController
    {
        public CategoriesController(ISender sender) : base(sender)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var query = new GetCategoriesListQuery()
            {
                Page = page,
                PageSize = pageSize,
            };
            var result = await Sender.Send(query, cancellationToken);
            return HandleResult(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(
            long id,
            CancellationToken cancellationToken = default)
        {
            var query = new GetCategoryQuery()
            {
                Id = id,
            };
            var result = await Sender.Send(query, cancellationToken);
            return HandleResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateCategoryCommand command,
            CancellationToken cancellationToken = default)
        {
            var result = await Sender.Send(command, cancellationToken);
            return HandleResult(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(
            [FromBody] UpdateCategoryCommand command,
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
            var command = new DeleteCategoryCommand()
            {
                Id = id,
            };
            var result = await Sender.Send(command, cancellationToken);
            return HandleResult(result);
        }
    }
}