using ArkFunds.App.Web.Api;

namespace ArkFunds.App.Web.Components.Pages;

public partial class History
{
    private ICollection<Report> history = new List<Report>();

    private Dictionary<Guid, bool> isActive = new(); 
    
    private bool isLoading = true;

    protected override async Task OnInitializedAsync()
    {
        history = await reportsClient.HistoryAsync();
        history = history.OrderByDescending(i => new DateTime(i.Year, i.Month, 1)).ToList();
        foreach (var item in history)
        {
            isActive[item.Id] = false;
        }
        isLoading = false;
    }
    
    private void ToggleTable(Guid id)
    {
        isActive[id] = !isActive[id];
    }
}
