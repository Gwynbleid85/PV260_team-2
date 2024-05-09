using ArkFunds.Reports.Core;

namespace ArkFunds.Reports.Application.Queries;

public record GetCurrentReportQuery(DateTime CurrentTime)
{
    public record Response(Report? Report);
};