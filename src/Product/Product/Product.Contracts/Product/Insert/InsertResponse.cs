namespace Product.Contracts.Product.Insert
{

    public record InsertResponse(
        Guid Id,
        string Name,
        string Description,
        decimal Price,
        int AvailableStock);
}
