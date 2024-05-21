using ArkFunds.Users.Application.Commands;
using ArkFunds.Users.Core;
using ArkFunds.Users.Core.Events;
using Mapster;
using Marten;
using Microsoft.AspNetCore.Mvc;
using Wolverine;
using Wolverine.Http;
using Wolverine.Http.Marten;

namespace ArkFunds.Users.Api;

public record UserEmailChangedRequest(string UserEmail);

public record NewUserRequest(string Name, string Email);

public class UsersEndpoint
{
    //TODO: Add authorization
    /// <summary>
    /// Get user by ID
    /// </summary>
    /// <param name="user"></param>
    /// <returns>user</returns>
    [WolverineGet("/users/{id}")]
    public static User UserGet([Document] User user)
    {
        return user;
    }

    //TODO: remove before production
    /// <summary>
    /// Create test user
    /// </summary>
    /// <param name="request"></param>
    /// <param name="bus"></param>
    /// <returns>user</returns>
    [WolverinePost("/users")]
    public static async Task<NewUserCreated> UserCreate(NewUserRequest request, IMessageBus bus)
    {
        var command = request.Adapt<CreateNewUserCommand>();
        return await bus.InvokeAsync<NewUserCreated>(command);
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
    public static async Task<UserEmailChanged> UserChangeEmail(Guid id, UserEmailChangedRequest request,
        IMessageBus bus)
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
    //TODO: Add authorization
    /// <summary>
    /// Unsubscribe user
    /// </summary>
    /// <param name="id"></param>
    /// <param name="bus"></param>
    /// <returns>id</returns>
    [WolverinePost("/users/{id}/unsubscription")]
    public static async Task<UserUnsubscribed> UserUnsubscribe(Guid id, IMessageBus bus)
    {
        var command = new UnsubscribeUserCommand(id);
        return await bus.InvokeAsync<UserUnsubscribed>(command);
    }
}