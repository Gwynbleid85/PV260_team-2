using Marten;

namespace ArkFunds.Reports.Application.Queries;

public class GetCurrentReportQueryHandler
{
    public static async Task<GetCurrentReportQuery.Response>
        Handler(GetCurrentReportQuery query, IQuerySession session)
    {
        var report = await session.QueryAsync(new CompiledQueries.GetCurrentReportQuery(query.CurrentTime));

        return new GetCurrentReportQuery.Response(report.FirstOrDefault());
    }
}