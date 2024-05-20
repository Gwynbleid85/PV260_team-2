using ArkFunds.App.Web.Client.Api;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;

namespace ArkFunds.App.Web.Client.Pages;

public partial class History
{
    [CascadingParameter]
    private Task<AuthenticationState> authenticationStateTask { get; set; }
    private ICollection<Report> history = new List<Report>();

    private Dictionary<Guid, bool> isActive = new(); 
    
    private bool isLoading = true;
    private bool cannotLoad = false;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            history = await reportsClient.HistoryAsync();
        }
        catch (Exception ex)
        {
            cannotLoad = true;
        }
        finally
        {
            try
            {

            var user = (await authenticationStateTask).User;
            } catch (Exception ex)
            {
                 history.ToList().RemoveAll(e => e.Month > DateTime.UtcNow.AddMonths(-3).Date.Month);
            }
            finally
            {
                history = history.OrderByDescending(i => new DateTime(i.Year, i.Month, 1)).ToList();
            }

        foreach (var item in history)
        {
            isActive[item.Id] = false;
        }
            isLoading = false;
        }
    }
    
    private void ToggleTable(Guid id)
    {
        isActive[id] = !isActive[id];
    }
}
