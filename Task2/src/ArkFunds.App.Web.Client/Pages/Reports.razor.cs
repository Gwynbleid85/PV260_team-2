using ArkFunds.App.Web.Client.Api;

namespace ArkFunds.App.Web.Client.Pages;

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
            try
            {
                await GetThreeMonthsOldReport();
            } catch (Exception ex2)
            {
                currentReportExists = false;
            }
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
        report = await reportsClient.ThreeMonthsOldAsync();
        currentReportExists = true;
    }
}
