using System.Linq.Expressions;
using ArkFunds.Reports.Core;
using Marten.Linq;

namespace ArkFunds.Reports.Application.Queries;

public class GetReportHistoryQuery() : ICompiledListQuery<Report>
{
    public Expression<Func<IMartenQueryable<Report>, IEnumerable<Report>>> QueryIs()
    {
        return report => report.OrderBy(r => r.Year).ThenBy(r => r.Month);
    }
}