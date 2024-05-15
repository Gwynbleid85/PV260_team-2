namespace ArkFunds.Users.Core.Events;

public record UserEmailChanged(Guid UserId, string UserEmail);