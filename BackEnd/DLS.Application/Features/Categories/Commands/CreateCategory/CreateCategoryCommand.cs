namespace DLS.Application.Features.Categories.Commands.CreateCategory;

public sealed record CreateCategoryCommand : ICommand<long>
{
    public string Name { get; init; } = null!;
    public string Description { get; init; } = null!;
    public long? ParentId { get; init; }
}