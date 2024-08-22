using Marten;

namespace ArkFunds.Reports.Application.Queries;

public class GetReportHistoryQueryHandler
{
    public static async Task<GetReportHistoryQuery.Response> Handle(GetReportHistoryQuery query, IQuerySession session)
    {
        var history = await session.QueryAsync(new CompiledQueries.GetReportHistoryQuery());

        return new GetReportHistoryQuery.Response(history);
    }
}