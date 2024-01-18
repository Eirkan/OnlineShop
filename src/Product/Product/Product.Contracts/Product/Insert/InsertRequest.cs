namespace Product.Contracts.Product.Insert
{
    public record InsertRequest(
        string Name,
        string Description,
        decimal Price,
        int AvailableStock);
}
