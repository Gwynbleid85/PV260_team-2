using ArkFunds.Users.Core;

namespace ArkFunds.Users.Application.Queries;

public record GetSubscribedUsersQuery()
{
    public record Response(IEnumerable<User> Users);
};
