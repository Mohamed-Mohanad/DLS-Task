using DLS.Application.Features.Categories.Queries.GetCategoriesList;
using DLS.Application.Features.Categories.Queries.GetCategory;
using Mapster;

namespace DLS.Application.Features.Categories.MappingConfig;

internal class CategoryMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Category, CategoryResponse>()
            .Map(dest => dest.ParentName, src => src.Parent == null ? string.Empty : src.Parent.Name);

        config.NewConfig<Category, GetCategoriesListQueryResponse>()
            .Map(dest => dest.ParentName, src => src.Parent == null ? string.Empty : src.Parent.Name)
            .Map(dest => dest.ChildrenCount, src => src.Children.Count);
    }
}
