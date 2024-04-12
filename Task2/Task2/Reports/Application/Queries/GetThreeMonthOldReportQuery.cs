using System.Linq.Expressions;
using Marten.Linq;
using Reports.Core;

namespace Reports.Application.Queries;

public class GetThreeMonthOldReportQuery(DateTime now) : ICompiledListQuery<Report>
{
    public DateTime Now = now;

    public Expression<Func<IMartenQueryable<Report>, IEnumerable<Report>>> QueryIs()
    {
        var threeMonthsAgo = Now.AddMonths(-3);
        return report => report.Where(r => r.Year == threeMonthsAgo.Year && r.Month == threeMonthsAgo.Month);
    }
}