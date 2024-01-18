namespace Product.Contracts.Product.GetById
{
    public record GetByIdResponse(
        Guid Id,
        string Name,
        string Description,
        decimal Price,
        int AvailableStock);
}
