
namespace DLS.Application.Features.Authentication.Queries.GetCurrentUser;

internal sealed class GetCurrentUserQueryHandler : IQueryHandler<GetCurrentUserQuery, CurrentUserResponse>
{
    private readonly IGenericRepository<User> _userRepo;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public GetCurrentUserQueryHandler(
        IGenericRepository<User> userRepo,
        ICurrentUserService currentUserService,
        IMapper mapper)
    {
        _userRepo = userRepo;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<Result<CurrentUserResponse>> Handle(
        GetCurrentUserQuery request,
        CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;

        var user = await _userRepo.GetByIdAsync(userId);

        var result = _mapper.Map<CurrentUserResponse>(user!);

        return result;
    }
}