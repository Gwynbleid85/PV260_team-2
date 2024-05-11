using ArkFunds.Users.Core;
using Marten;

namespace ArkFunds.Users.Application.Queries;

public class GetSubscribedUsersQueryHandler
{
    public static async Task<IEnumerable<User>> Handle(GetSubscribedUsersQuery query, IQuerySession session, CancellationToken cancellationToken)
    {
        return await session.Query<User>().Where(x => x.IsSubscribed).ToListAsync(cancellationToken);
    }
}