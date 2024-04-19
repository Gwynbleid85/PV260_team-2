using WebApp.Api;

namespace WebApp.Components.Pages;

public partial class History
{
    private ICollection<Api.Report> history = new List<Api.Report>();

    private Dictionary<Guid, bool> isActive = new Dictionary<Guid, bool>(); 
    
    private bool isLoading = true;

    protected override async Task OnInitializedAsync()
    {
        history = await reportsClient.HistoryAsync();
        foreach(var item in history)
        {
            isActive[item.Id] = false;
        }
        isLoading = false;
    }
    public void ToggleTable(Guid Id)
    {
        isActive[Id] = !isActive[Id];
    }
}
