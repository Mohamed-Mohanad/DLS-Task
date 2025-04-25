using DLS.Application.Specifications.ProductSpecs;

namespace DLS.Application.Features.Products.Commands.UpdateProduct;

internal sealed class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand>
{
    private readonly IGenericRepository<Product> _productRepo;

    public UpdateProductCommandHandler(IGenericRepository<Product> productRepo)
    {
        _productRepo = productRepo;
    }

    public async Task<Result> Handle(
        UpdateProductCommand request,
        CancellationToken cancellationToken)
    {
        var spec = new GetProductByIdSpecification(request.Id);
        var product = await _productRepo.GetEntityWithSpecAsync(spec, cancellationToken);

        if (product is null)
            return Result.Failure<bool>(Error.NotFound());

        product.Update(
            request.Name,
            request.Description,
            request.Price,
            request.CategoryId);

        await _productRepo.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}