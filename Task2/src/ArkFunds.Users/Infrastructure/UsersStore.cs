using Marten;

namespace ArkFunds.Users.Infrastructure;

public class UsersStore(StoreOptions options) : DocumentStore(options)
{
}