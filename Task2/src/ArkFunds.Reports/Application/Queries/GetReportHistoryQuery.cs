using ArkFunds.Reports.Core;

namespace ArkFunds.Reports.Application.Queries;

public record GetReportHistoryQuery()
{
    public record Response(IEnumerable<Report> ReportHistory);
}