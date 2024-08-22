using ArkFunds.Users.Core;
using Marten;

namespace ArkFunds.Users.Application.Queries;

public class GetSubscribedUsersQueryHandler
{
    public static async Task<GetSubscribedUsersQuery.Response> Handle(GetSubscribedUsersQuery query,
        IQuerySession session, CancellationToken cancellationToken)
    {
        var subscribedUsers = await session.Query<User>().Where(x => x.IsSubscribed).ToListAsync(cancellationToken);
        return new GetSubscribedUsersQuery.Response(subscribedUsers);
    }
}