namespace Customer.Core.Domain.Messaging
{

    public interface ICacheInvalidator<in TRequest>
    {
        Task Invalidate(TRequest request);
    }
}