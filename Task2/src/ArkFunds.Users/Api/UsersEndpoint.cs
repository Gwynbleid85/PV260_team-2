using ArkFunds.Users.Application.Commands;
using ArkFunds.Users.Core;
using ArkFunds.Users.Core.Events;
using Marten;
using Microsoft.AspNetCore.Mvc;
using Wolverine;
using Wolverine.Http;
using Wolverine.Http.Marten;

namespace ArkFunds.Users.Api;

public record ChangeUserEmailRequest(string UserEmail);

public class UsersEndpoint
{
    //TODO: Add authorization
    /// <summary>
    /// Get user by ID
    /// </summary>
    /// <param name="user"></param>
    /// <returns>user</returns>
    [WolverineGet("/users/{id}")]
    public static User UserGet([Document]User user)
    {
        return user;
    }
    
    //TODO: remove before production
    /// <summary>
    /// Create test user
    /// </summary>
    /// <param name="session"></param>
    /// <returns>user</returns>
    [WolverinePost("/users")]
    public static async Task<User> UserCreate(IDocumentSession session)
    {
        var user = new User{Id = Guid.NewGuid(), Name = "John Doe", IsSubscribed = false, Email = "test@test.cz"};
        session.Store(user);
        await session.SaveChangesAsync();
        return user;
    }
    
    //TODO: Add authorization
    /// <summary>
    /// Delete user by ID
    /// </summary>
    /// <param name="id"></param>
    /// <param name="bus"></param>
    /// <returns>id</returns>
    [WolverineDelete("/users/{id}")]
    public static async Task<UserDeleted> UserDelete(Guid id, IMessageBus bus)
    {
        var command = new DeleteUserCommand(id);
        return await bus.InvokeAsync<UserDeleted>(command);
    }
    
    //TODO: Add authorization
    /// <summary>
    /// Change user email
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <param name="bus"></param>
    /// <returns>id, email</returns>
    [WolverinePut("/users/{id}/email")]
    public static async Task<UserEmailChanged> UserChangeEmail(Guid id, ChangeUserEmailRequest request, IMessageBus bus)
    {
        var command = new ChangeUserEmailCommand(id, request.UserEmail);
        return await bus.InvokeAsync<UserEmailChanged>(command);
    }
    
    //TODO: Add authorization
    /// <summary>
    /// Subscribe user
    /// </summary>
    /// <param name="id"></param>
    /// <param name="bus"></param>
    /// <returns>id</returns>
    [WolverinePost("/users/{id}/subscription")]
    public static async Task<UserSubscribed> UserSubscribe(Guid id, IMessageBus bus)
    {
        var command = new SubscribeUserCommand(id);
        return await bus.InvokeAsync<UserSubscribed>(command);
    }
}