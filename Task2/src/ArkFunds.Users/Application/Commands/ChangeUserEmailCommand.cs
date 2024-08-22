namespace ArkFunds.Users.Application.Commands;

public record ChangeUserEmailCommand(Guid UserId, string UserEmail);
