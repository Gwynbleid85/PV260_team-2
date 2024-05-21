using ArkFunds.App.Web.Api;

namespace ArkFunds.App.Web.Pages;

public partial class Reports
{
    private Report report;
    private Report oldReport;

    private bool isLoading = true;
    private bool currentReportExists;
    
    protected override async Task OnInitializedAsync()
    {
        try
        {
            var state = await authProvider.GetAuthenticationStateAsync();
            if (state.User.Identity?.IsAuthenticated ?? false)
            {
                await GetCurrentMonthReport();
            } 
            else
            {
                await GetThreeMonthsOldReport();
            }
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
    private async Task GetThreeMonthsOldReport()
    {
        isLoading = true;
        oldReport = await reportsClient.ThreeMonthsOldAsync();
        currentReportExists = true;
    }
}
