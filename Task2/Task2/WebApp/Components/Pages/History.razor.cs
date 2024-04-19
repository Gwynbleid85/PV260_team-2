using WebApp.Api;

namespace WebApp.Components.Pages;

public partial class History
{
    private ICollection<Api.Report> history = new List<Api.Report>();
    
    private bool isLoading = true;

    protected override async Task OnInitializedAsync()
    {
        history = await reportsClient.HistoryAsync();
        isLoading = false;
    }
}
