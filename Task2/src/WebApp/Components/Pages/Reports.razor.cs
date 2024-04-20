using WebApp.Api;

namespace WebApp.Components.Pages;

public partial class Reports
{
    private Api.Report report;

    private bool isLoading = true;
    private bool isGeneratingReport;
    private bool currentReportExists;

    private string? error;
    
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

    private async Task GenerateReport()
    {
        try
        {
            isGeneratingReport = true;
            await reportsClient.ReportsAsync();
            await GetCurrentMonthReport();
        }
        catch (Exception ex)
        {
            error = "Report for this month already exists.";
        }
        finally
        {
            isLoading = false;
            isGeneratingReport = false;
        }
    }

    private async Task GetCurrentMonthReport()
    {
        isLoading = true;
        report = await reportsClient.CurrentAsync();
        currentReportExists = true;
        error = null;
    }
}
