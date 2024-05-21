using ArkFunds.App.Web.Api;

namespace ArkFunds.App.Web.Pages;

public partial class UserSettings
{
    private User user;
    private string userName;

    private bool isLoading = true;

    protected override async Task OnInitializedAsync()
    {
        var state = await authProvider.GetAuthenticationStateAsync();
        try
        {
            if (state.User.Identity?.IsAuthenticated ?? false)
            {
                userName = state.User.Claims.First(x => x.Type == "name").Value;
                
                var id = new Guid(state.User.Claims.First(x => x.Type == "sub").Value);
                user = await usersClient.UsersGetAsync(id);
            }
        }
        finally
        {
            isLoading = false;
        }
    }

    public async Task ToggleSubscriptionSettingAsync()
    {
        await usersClient.SubscriptionAsync(user.Id);
        user = await usersClient.UsersGetAsync(user.Id);
    }

}
