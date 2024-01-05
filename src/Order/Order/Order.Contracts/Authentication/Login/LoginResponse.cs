namespace Order.Contracts.Authentication.Login
{
    public record LoginResponse(
        string FirstName,
        string LastName,
        string Email,
        string Password,
        string Token);
}
