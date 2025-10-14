using MediatR;
using SimonMovilidad.Domain.Abstractions;

namespace SimonMovilidad.Application.Abstractions.Messaging
{
    public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, Result>
        where TCommand : ICommand
    {
    }

    public interface ICommandHandler<TCommand, TResult> : IRequestHandler<TCommand, Result<TResult>>
        where TCommand : ICommand<TResult>
    {
    }
}
