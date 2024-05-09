using System.Linq.Expressions;
using ArkFunds.Reports.Core;
using Marten.Linq;

namespace ArkFunds.Reports.Application.Queries.CompiledQueries;

public class GetReportQuery(DateTime date) : ICompiledQuery<Report>
{
    public DateTime Date = date;

    public Expression<Func<IMartenQueryable<Report>, Report>> QueryIs()
    {
        return report => report.First(r => r.Year == Date.Year && r.Month == Date.Month);
    }
}