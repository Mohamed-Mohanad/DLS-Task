namespace DLS.Application.Features.Products.Commands.CreateProduct;

internal sealed class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, long>
{
    private readonly IGenericRepository<Product> _productRepo;

    public CreateProductCommandHandler(IGenericRepository<Product> productRepo)
    {
        _productRepo = productRepo;
    }

    public async Task<Result<long>> Handle(
        CreateProductCommand request,
        CancellationToken cancellationToken)
    {
        var product = Product.Create(
            request.Name,
            request.Description,
            request.Price,
            request.CategoryId);

        await _productRepo.AddAsync(product, cancellationToken);
        await _productRepo.SaveChangesAsync(cancellationToken);

        return product.Id;
    }
}