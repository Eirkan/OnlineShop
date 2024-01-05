using MediatR;

namespace Product.Core.Domain.Messaging
{
    /// <summary>
    /// Represents the marker interface for a command.
    /// </summary>
    /// <typeparam name="TResult">The command result type.</typeparam>
    public interface ICommand<out TResult> : IRequest<TResult>
    {
    }

}
