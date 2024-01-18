namespace Customer.Contracts.Customer.Insert
{
    public record InsertResponse(
        Guid CustomerId,
        string FirstName,
        string LastName,
        string Email);
}
