using ArkFunds.Reports.Core;

namespace ArkFunds.Reports.Application.Queries;

public record GetThreeMonthOldReportQuery(DateTime CurrentTime)
{
    public record Response(Report? Report);
}