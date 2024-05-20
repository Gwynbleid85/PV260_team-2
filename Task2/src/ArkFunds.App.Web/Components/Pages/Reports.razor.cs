using ArkFunds.App.Web.Api;

namespace ArkFunds.App.Web.Components.Pages;

public partial class Reports
{
    private Report report;

    private bool isLoading = true;
    private bool currentReportExists;
    
    protected override async Task OnInitializedAsync()
    {
        try
        {
            await GetCurrentMonthReport();
        }
        catch (Exception ex)
        {
            currentReportExists = false;
        }
        finally
        {
            isLoading = false;
        }
    }

    private async Task GetCurrentMonthReport()
    {
        isLoading = true;
        report = await reportsClient.CurrentAsync();
        currentReportExists = true;
    }
}
