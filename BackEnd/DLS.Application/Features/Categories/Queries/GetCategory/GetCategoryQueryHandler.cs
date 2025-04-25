using DLS.Application.Specifications.CategorySpecs;

namespace DLS.Application.Features.Categories.Queries.GetCategory;

internal sealed class GetCategoryQueryHandler : IQueryHandler<GetCategoryQuery, CategoryResponse>
{
    private readonly IMapper _mapper;
    private readonly IGenericRepository<Category> _categoryRepo;

    public GetCategoryQueryHandler(IMapper mapper, IGenericRepository<Category> categoryRepo)
    {
        _mapper = mapper;
        _categoryRepo = categoryRepo;
    }

    public async Task<Result<CategoryResponse>> Handle(
        GetCategoryQuery request,
        CancellationToken cancellationToken)
    {
        var spec = new GetCategoryByIdSpecification(request.Id);
        var category = await _categoryRepo.GetEntityWithSpecAsync(spec, cancellationToken);

        if (category is null)
            return Result.Failure<CategoryResponse>(Error.NotFound());

        var response = _mapper.Map<CategoryResponse>(category);

        return response;
    }
}