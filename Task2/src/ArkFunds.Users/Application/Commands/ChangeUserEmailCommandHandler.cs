using ArkFunds.Users.Core;
using ArkFunds.Users.Core.Events;
using CommunityToolkit.Diagnostics;
using Mapster;
using Marten;

namespace ArkFunds.Users.Application.Commands;

public class ChangeUserEmailCommandHandler
{
    public static async Task<User> LoadAsync(ChangeUserEmailCommand command, IQuerySession session)
    {
        var user = await session.LoadAsync<User>(command.UserId);
        Guard.IsNotNull(user);
        return user;
    }
    
    public static async Task<UserEmailChanged> Handle(ChangeUserEmailCommand command, User user, IDocumentSession session, CancellationToken cancellationToken)
    {
        user.Email = command.UserEmail;
        session.Update(user);
        await session.SaveChangesAsync(cancellationToken);
        return command.Adapt<UserEmailChanged>();
    }
}