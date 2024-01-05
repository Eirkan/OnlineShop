namespace Product.Core.Domain.Messaging
{

    public interface ICacheInvalidator<in TRequest>
    {
        Task Invalidate(TRequest request);
    }
}