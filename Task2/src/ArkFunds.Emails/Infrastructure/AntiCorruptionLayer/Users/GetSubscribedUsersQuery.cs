using ArkFunds.Emails.Core;
using Mapster;
using Wolverine;
using GetSubscribedUsersQueryExternal = ArkFunds.Users.Application.Queries.GetSubscribedUsersQuery;

namespace ArkFunds.Emails.Infrastructure.AntiCorruptionLayer.Users;

public record GetSubscribedUsersQuery()
{
    public record Response(IEnumerable<User> Users);
};

public class AntiCorruptionLayerGetSubscribedUsersQueryHandler
{
    public static async Task<GetSubscribedUsersQuery.Response> Handle(GetSubscribedUsersQuery internalQuery,
        IMessageBus bus)
    {
        var externalQuery = new GetSubscribedUsersQueryExternal();

        var externalResponse =
            await bus.InvokeAsync<GetSubscribedUsersQueryExternal.Response>(externalQuery);

        var internalResponse = externalResponse.Adapt<GetSubscribedUsersQuery.Response>();
        return internalResponse;
    }
}