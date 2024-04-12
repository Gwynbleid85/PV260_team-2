using System.Linq.Expressions;
using Marten.Linq;
using Reports.Core;

namespace Reports.Application.Queries;

public class GetReportHistoryQuery() : ICompiledListQuery<Report>
{
    public Expression<Func<IMartenQueryable<Report>, IEnumerable<Report>>> QueryIs()
    {
        return report => report.OrderBy(r => r.Year).ThenBy(r => r.Month);
    }
}