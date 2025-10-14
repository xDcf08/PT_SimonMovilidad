using MediatR;
using SimonMovilidad.Domain.Abstractions;

namespace SimonMovilidad.Application.Abstractions.Messaging
{
    public interface IQuery<TResponse> : IRequest<Result<TResponse>>
    {
    }
}
