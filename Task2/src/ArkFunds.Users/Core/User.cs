namespace ArkFunds.Users.Core;

public class User
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public bool IsSubscribed { get; set; }
    public required string Email { get; set; }
    
}