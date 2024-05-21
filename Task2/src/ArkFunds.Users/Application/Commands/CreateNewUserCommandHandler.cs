using ArkFunds.Users.Core;
using ArkFunds.Users.Core.Events;
using Mapster;
using Marten;

namespace ArkFunds.Users.Application.Commands;

public class CreateNewUserCommandHandler
{
    public static async Task<NewUserCreated> Handle(CreateNewUserCommand command, IDocumentSession session,
        CancellationToken cancellationToken)
    {
        var user = command.Adapt<User>();
        user.IsSubscribed = false;

        session.Store(user);
        await session.SaveChangesAsync(cancellationToken);

        return command.Adapt<NewUserCreated>() with { Id = user.Id };
    }
}