using Marten;
using Reports.Application.Commands;
using Reports.Application.Queries;
using Reports.Application.ServiceInterfaces;
using Reports.Core;
using Reports.Core.Events;
using Wolverine;
using Wolverine.Http;

namespace Reports.Api;

public class ReportsEndpoint
{
    //TODO: Add authorization
    /// <summary>
    /// Get current report
    /// </summary>
    /// <param name="session"></param>
    /// <param name="timeProvider"></param>
    /// <remarks>User has to be logged in for this endpoint</remarks>
    /// <returns>Current report</returns>
    [WolverineGet("/reports/current")]
    public static async Task<Report?> GetCurrentReport(IQuerySession session, ITimeProvider timeProvider)
    {
        var report = await session.QueryAsync(new GetCurrentReportQuery(timeProvider.GetCurrentTime()));
        return report.FirstOrDefault();
    }

    /// <summary>
    /// Get three months old report
    /// </summary>
    /// <param name="session"></param>
    /// <param name="timeProvider"></param>
    /// <returns>Three month old report </returns>
    [WolverineGet("/reports/three-months-old")]
    public static async Task<Report?> Get3MonthsOldReport(IQuerySession session,
        ITimeProvider timeProvider)
    {
        var report = await session.QueryAsync(new GetThreeMonthOldReportQuery(timeProvider.GetCurrentTime()));
        return report.FirstOrDefault();
    }

    //TODO: Add authorization
    /// <summary>
    /// Get report history
    /// </summary>
    /// <param name="session"></param>
    /// <remarks>User has to be logged in for this endpoint</remarks>
    /// <returns>Report history</returns>
    [WolverineGet("/reports/history")]
    public static async Task<IEnumerable<Report>> GetReportHistory(IQuerySession session)
    {
        return await session.QueryAsync(new GetReportHistoryQuery());
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