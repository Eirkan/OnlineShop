namespace Product.Contracts.Authentication.Register
{

    public record RegisterResponse(
        string FirstName,
        string LastName,
        string Email,
        string Password,
        string Token);
}
