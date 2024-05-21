using ArkFunds.App.Web.Api;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace ArkFunds.App.Web.Pages;

public partial class History
{
    private ICollection<Report> history = new List<Report>();

    private Dictionary<Guid, bool> isActive = new(); 
    
    private bool isLoading = true;
    private bool cannotLoad = false;

    protected override async Task OnInitializedAsync()
    {
        var state = await authProvider.GetAuthenticationStateAsync();

        try
        {
            if (state.User.Identity?.IsAuthenticated ?? false)
            {
                history = await reportsClient.HistoryAsync();
            }
            else
            {
                // TODO: three-monhts-old history
                history = await reportsClient.HistoryAsync();
                history.ToList().RemoveAll(e => e.Month > DateTime.UtcNow.AddMonths(-3).Date.Month);
            }
        }
        catch (Exception ex)
        {
            cannotLoad = true;
        }
        finally
        {
            isLoading = false;
            foreach (var item in history)
            {
                isActive[item.Id] = false;
            }
        }
    }
    
    private void ToggleTable(Guid id)
    {
        isActive[id] = !isActive[id];
    }
}
