using Marten;

namespace ArkFunds.Reports.Application.Queries;

public class GetThreeMonthOldReportQueryHandler
{
    public static async Task<GetCurrentReportQuery.Response> Handle(GetThreeMonthOldReportQuery query,
        IQuerySession session)
    {
        var report = await session.QueryAsync(new CompiledQueries.GetCurrentReportQuery(query.CurrentTime));

        return new GetCurrentReportQuery.Response(report.FirstOrDefault());
    }
}