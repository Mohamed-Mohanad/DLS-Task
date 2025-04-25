namespace DLS.Application.Features.Categories.Commands.DeleteCategory;

public sealed record DeleteCategoryCommand : ICommand<bool>
{
    public long Id { get; init; }
}