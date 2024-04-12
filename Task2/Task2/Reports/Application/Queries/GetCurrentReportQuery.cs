using System.Linq.Expressions;
using Marten.Linq;
using Reports.Core;

namespace Reports.Application.Queries;

public class GetCurrentReportQuery(DateTime now) : ICompiledListQuery<Report>
{
    public DateTime Now = now;

    public Expression<Func<IMartenQueryable<Report>, IEnumerable<Report>>> QueryIs()
    {
        return report => report.Where(r => r.Year == Now.Year && r.Month == Now.Month);
    }
}