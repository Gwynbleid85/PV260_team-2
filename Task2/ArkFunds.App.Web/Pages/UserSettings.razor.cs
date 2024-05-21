using ArkFunds.App.Web.Api;

namespace ArkFunds.App.Web.Pages;

public partial class UserSettings
{
    private User user;

    private bool isLoading = true;
    private bool userExists;
    private bool isChanging = false;
    private string? error;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            await GetUserInfoAsync();
        }
        catch (Exception ex)
        {
            userExists = false;
        }
        finally
        {
            isLoading = false;
        }
    }

    private async Task GetUserInfoAsync()
    {
        isLoading = true;
        user = await usersClient.UsersGetAsync(new Guid("1805258c-3d1b-4553-8540-279c6e3e7570"));
        await Task.CompletedTask;
        userExists = true;
        error = null;
    }

    public async Task ToggleSubscriptionSettingAsync()
    {
        await usersClient.SubscriptionAsync(user.Id);
        user = await usersClient.UsersGetAsync(user.Id);
    }

}
