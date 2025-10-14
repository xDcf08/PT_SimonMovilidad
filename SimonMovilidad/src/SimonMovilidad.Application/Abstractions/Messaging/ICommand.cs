using MediatR;
using SimonMovilidad.Domain.Abstractions;

namespace SimonMovilidad.Application.Abstractions.Messaging
{
    public interface ICommand : IRequest<Result>
    {
    }

    public interface ICommand<TResponse> : IRequest<Result<TResponse>>
    {
    }

}
