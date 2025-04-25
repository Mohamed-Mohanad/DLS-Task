namespace DLS.Application.Features.Categories.Commands.UpdateCategory;

public sealed record UpdateCategoryCommand : ICommand
{
    public long Id { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public long? ParentId { get; init; }
}