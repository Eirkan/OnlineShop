namespace Order.Core.Domain.Messaging
{

    public interface ICacheInvalidator<in TRequest>
    {
        Task Invalidate(TRequest request);
    }
}