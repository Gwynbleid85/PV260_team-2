using WebApp.Api;

namespace WebApp.Components.Pages;

public partial class Reports
{
    private Api.Report report;

    private bool isLoading = true;
    private bool isEmpty = false;
    protected override async Task OnInitializedAsync()
    {
        try
        {
            report = await reportsClient.CurrentAsync();
        } catch(Exception ex) {
            isEmpty = true;
        }
        isLoading = false;
    }
}
