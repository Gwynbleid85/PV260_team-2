using System.Linq.Expressions;
using ArkFunds.Reports.Core;
using Marten.Linq;

namespace ArkFunds.Reports.Application.Queries.CompiledQueries;

public class GetCurrentReportQuery(DateTime now) : ICompiledListQuery<Report>
{
    public DateTime Now = now;

    public Expression<Func<IMartenQueryable<Report>, IEnumerable<Report>>> QueryIs()
    {
        return report => report.Where(r => r.Year == Now.Year && r.Month == Now.Month);
    }
}