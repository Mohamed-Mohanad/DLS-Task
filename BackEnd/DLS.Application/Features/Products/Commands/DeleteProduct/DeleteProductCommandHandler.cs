using DLS.Application.Specifications.ProductSpecs;

namespace DLS.Application.Features.Products.Commands.DeleteProduct;

internal sealed class DeleteProductCommandHandler : ICommandHandler<DeleteProductCommand, bool>
{
    private readonly IGenericRepository<Product> _productRepo;

    public DeleteProductCommandHandler(IGenericRepository<Product> productRepo)
    {
        _productRepo = productRepo;
    }

    public async Task<Result<bool>> Handle(
        DeleteProductCommand request,
        CancellationToken cancellationToken)
    {
        var spec = new GetProductByIdSpecification(request.Id);
        var product = await _productRepo.GetEntityWithSpecAsync(spec, cancellationToken);

        if (product is null)
            return Result.Failure<bool>(Error.NotFound());

        _productRepo.Delete(product);
        var result = await _productRepo.SaveChangesAsync(cancellationToken);

        return result;
    }
}