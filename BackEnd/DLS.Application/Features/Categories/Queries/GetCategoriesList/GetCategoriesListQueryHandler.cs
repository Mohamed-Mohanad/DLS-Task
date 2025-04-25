using DLS.Application.Specifications.CategorySpecs;

namespace DLS.Application.Features.Categories.Queries.GetCategoriesList;

internal sealed class GetCategoriesListQueryHandler : IPaginateQueryHandler<GetCategoriesListQuery, GetCategoriesListQueryResponse>
{
    private readonly IMapper _mapper;
    private readonly IGenericRepository<Category> _categoryRepo;

    public GetCategoriesListQueryHandler(
        IMapper mapper,
        IGenericRepository<Category> categoryRepo)
    {
        _mapper = mapper;
        _categoryRepo = categoryRepo;
    }

    public async Task<Pagination<GetCategoriesListQueryResponse>> Handle(
        GetCategoriesListQuery request,
        CancellationToken cancellationToken)
    {
        var spec = new GetAllCategoriesSpecification(
            request.Page,
            request.PageSize);

        var (queryResult, totalCount) = await _categoryRepo.GetWithSpecAsync(spec, cancellationToken);

        var categories = _mapper.Map<List<GetCategoriesListQueryResponse>>(queryResult);

        var result = new Pagination<GetCategoriesListQueryResponse>(request.Page, request.PageSize, totalCount, categories);

        return result;
    }
}