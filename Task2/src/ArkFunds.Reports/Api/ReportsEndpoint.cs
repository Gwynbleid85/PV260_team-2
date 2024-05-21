using ArkFunds.Reports.Application.Commands;
using ArkFunds.Reports.Application.Queries;
using ArkFunds.Reports.Application.ServiceInterfaces;
using ArkFunds.Reports.Core;
using ArkFunds.Reports.Core.Events;
using Microsoft.AspNetCore.Authorization;
using Wolverine;
using Wolverine.Http;

namespace ArkFunds.Reports.Api;

public class ReportsEndpoint
{
    /// <summary>
    /// Get current report
    /// </summary>
    /// <param name="bus"></param>
    /// <param name="timeProvider"></param>
    /// <remarks>User has to be logged in for this endpoint</remarks>
    /// <returns>Current report</returns>
    [WolverineGet("/reports/current")]
    public static async Task<Report?> GetCurrentReport(IMessageBus bus, ITimeProvider timeProvider)
    {
        var query = new GetCurrentReportQuery(timeProvider.GetCurrentTime());

        var queryResponse = await bus.InvokeAsync<GetCurrentReportQuery.Response>(query);
        return queryResponse.Report;
    }

    /// <summary>
    /// Get three months old report
    /// </summary>
    /// <param name="bus"></param>
    /// <param name="timeProvider"></param>
    /// <returns>Three month old report </returns>
    [AllowAnonymous]
    [WolverineGet("/reports/three-months-old")]
    public static async Task<Report?> GetThreeMonthsOldReport(IMessageBus bus,
        ITimeProvider timeProvider)
    {
        var query = new GetThreeMonthOldReportQuery(timeProvider.GetCurrentTime());

        var queryResponse = await bus.InvokeAsync<GetThreeMonthOldReportQuery.Response>(query);
        return queryResponse?.Report;
    }

    /// <summary>
    /// Get report history
    /// </summary>
    /// <param name="bus"></param>
    /// <remarks>User has to be logged in for this endpoint</remarks>
    /// <returns>Report history</returns>
    [WolverineGet("/reports/history")]
    public static async Task<IEnumerable<Report>> GetReportHistory(IMessageBus bus)
    {
        var query = new GetReportHistoryQuery();

        var queryResponse = await bus.InvokeAsync<GetReportHistoryQuery.Response>(query);
        return queryResponse.ReportHistory;
    }

    //TODO: Remove after testing
    /// <summary>
    /// Generate new report manually
    /// </summary>
    /// <param name="bus"></param>
    /// <param name="timeProvider"></param>
    /// <remarks>This endpoint is intended only for testing</remarks>
    /// <returns>Generated report info</returns>
    [WolverinePost("/reports")]
    public static async Task<ReportGenerated> GenerateReport(IMessageBus bus, ITimeProvider timeProvider)
    {
        var now = timeProvider.GetCurrentTime();
        var command = new GenerateReportCommand(now.Year, now.Month);

        return await bus.InvokeAsync<ReportGenerated>(command);
    }
}