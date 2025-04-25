using Domain.Shared;
using MediatR;


namespace Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}

public interface IPaginateQuery<TResponse> : IRequest<Pagination<TResponse>>
{
}