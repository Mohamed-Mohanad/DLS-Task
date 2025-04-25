namespace DLS.Application.Specifications.UserSpecs;

internal class GetUserByUsernameSpecification : Specification<User>
{
    public GetUserByUsernameSpecification(string username)
    {
        AddCriteria(x => x.Username == username);
    }
}
