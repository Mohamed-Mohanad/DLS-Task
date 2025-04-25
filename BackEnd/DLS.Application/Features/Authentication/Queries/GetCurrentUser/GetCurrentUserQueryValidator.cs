namespace DLS.Application.Features.Authentication.Queries.GetCurrentUser;

internal sealed class GetCurrentUserQueryValidator : AbstractValidator<GetCurrentUserQuery>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IGenericRepository<User> _userRepo;

    public GetCurrentUserQueryValidator(ICurrentUserService currentUserService, IGenericRepository<User> userRepo)
    {
        _currentUserService = currentUserService;
        _userRepo = userRepo;

        RuleFor(x => x)
            .Cascade(CascadeMode.Stop)
            .Must(Authorized).WithMessage(ErrorMessages.Unauthorized)
            .MustAsync(Exist).WithMessage(ErrorMessages.NotFound);
    }

    private bool Authorized(GetCurrentUserQuery query)
    {
        return _currentUserService.UserId.HasValue;
    }

    private async Task<bool> Exist(GetCurrentUserQuery query, CancellationToken cancellationToken)
    {
        return await _userRepo.IsExistAsync(x => x.Id == _currentUserService.UserId!.Value);
    }
}