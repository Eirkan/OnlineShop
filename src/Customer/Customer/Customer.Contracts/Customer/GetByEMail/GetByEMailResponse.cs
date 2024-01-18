namespace Customer.Contracts.Customer.GetByEMail
{
    public record GetByEMailResponse(
        Guid CustomerId,
        string FirstName,
        string LastName,
        string Email);
}
