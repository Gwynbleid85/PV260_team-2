using ArkFunds.Users.Core;
using ArkFunds.Users.Core.Events;
using CommunityToolkit.Diagnostics;
using Mapster;
using Marten;

namespace ArkFunds.Users.Application.Commands;

public class UnsubscribeUserCommandHandler
{
    public static async Task<User> LoadAsync(UnsubscribeUserCommand command, IQuerySession session)
    {
        var user = await session.LoadAsync<User>(command.UserId);
        Guard.IsNotNull(user);
        return user;
    }
    
    public static async Task<UserUnsubscribed> Handle(UnsubscribeUserCommand command, User user, IDocumentSession session, CancellationToken cancellationToken)
    {
        user.IsSubscribed = false;
        session.Update(user);
        await session.SaveChangesAsync(cancellationToken);
        return command.Adapt<UserUnsubscribed>();
    }
}