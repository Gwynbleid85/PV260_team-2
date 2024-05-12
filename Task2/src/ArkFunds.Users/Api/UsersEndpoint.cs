using ArkFunds.Users.Application.Commands;
using ArkFunds.Users.Core;
using ArkFunds.Users.Core.Events;
using Marten;
using Microsoft.AspNetCore.Mvc;
using Wolverine;
using Wolverine.Http;
using Wolverine.Http.Marten;

namespace ArkFunds.Users.Api;

public class UsersEndpoint
{
    [WolverineGet("/users/{id}")]
    public static User UserGet([Document]User user)
    {
        return user;
    }
    
    [WolverinePost("/users")]
    public static async Task<User> UserCreate(IDocumentSession session)
    {
        var user = new User{Id = Guid.NewGuid(), Name = "John Doe", IsSubscribed = false, Email = "test@test.cz"};
        session.Store(user);
        await session.SaveChangesAsync();
        return user;
    }
    
    [WolverineDelete("/users/{id}")]
    public static async Task<UserDeleted> UserDelete(IMessageBus bus)
    {
        var command = new DeleteUserCommand();
        return await bus.InvokeAsync<UserDeleted>(command);
    }
    
    [WolverinePut("/users/{id}/email")]
    public static async Task<UserEmailChanged> UserChangeEmail(Guid id, [FromBody] string userEmail, IMessageBus bus)
    {
        var command = new ChangeUserEmailCommand(id, userEmail);
        return await bus.InvokeAsync<UserEmailChanged>(command);
    }
    
    [WolverinePost("/users/{id}/subscription")]
    public static async Task<UserSubscribed> UserSubscribe(Guid id, IMessageBus bus)
    {
        var command = new SubscribeUserCommand(id);
        return await bus.InvokeAsync<UserSubscribed>(command);
    }
}