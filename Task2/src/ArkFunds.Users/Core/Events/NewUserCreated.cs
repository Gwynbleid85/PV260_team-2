namespace ArkFunds.Users.Core.Events;

public record NewUserCreated(Guid Id, string Name, string Email);