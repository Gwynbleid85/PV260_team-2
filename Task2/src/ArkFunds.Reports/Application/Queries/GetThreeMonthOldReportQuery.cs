using System.Linq.Expressions;
using ArkFunds.Reports.Core;
using Marten.Linq;

namespace ArkFunds.Reports.Application.Queries;

public class GetThreeMonthOldReportQuery(DateTime now) : ICompiledListQuery<Report>
{
    public DateTime Now = now;

    public Expression<Func<IMartenQueryable<Report>, IEnumerable<Report>>> QueryIs()
    {
        var threeMonthsAgo = Now.AddMonths(-3);
        return report => report.Where(r => r.Year == threeMonthsAgo.Year && r.Month == threeMonthsAgo.Month);
    }
}