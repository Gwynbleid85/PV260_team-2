using ArkFunds.Users.Core;
using ArkFunds.Users.Core.Events;
using CommunityToolkit.Diagnostics;
using Mapster;
using Marten;

namespace ArkFunds.Users.Application.Commands;

public class DeleteUserCommandHandler
{
    public static async Task<User> LoadAsync(DeleteUserCommand command, IQuerySession session)
    {
        var user = await session.LoadAsync<User>(command.UserId);
        Guard.IsNotNull(user);
        return user;
    }

    public static async Task<UserDeleted> Handle(DeleteUserCommand command, User user, IDocumentSession session,
        CancellationToken cancellationToken)
    {
        session.Delete(user);
        await session.SaveChangesAsync(cancellationToken);
        return command.Adapt<UserDeleted>();
    }
}