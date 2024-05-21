using Marten;

namespace ArkFunds.Reports.Application.Queries;

public class GetThreeMonthOldReportQueryHandler
{
    public static async Task<GetThreeMonthOldReportQuery.Response> Handle(GetThreeMonthOldReportQuery query,
        IQuerySession session)
    {
        var report = await session.QueryAsync(new CompiledQueries.GetThreeMonthOldReportQuery(query.CurrentTime));

        return new GetThreeMonthOldReportQuery.Response(report.FirstOrDefault());
    }
}