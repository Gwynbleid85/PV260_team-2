using WebApp.Api;

namespace WebApp.Components.Pages;

public partial class Reports
{
    private Api.Report report;

    private bool isLoading = true;

    protected override async Task OnInitializedAsync()
    {
        report = await reportsClient.CurrentAsync();
        isLoading = false;
    }
}
