using ArkFunds.App.Web.Api;

namespace ArkFunds.App.Web.Pages;

public partial class History
{
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
