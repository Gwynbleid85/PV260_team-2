using System.Linq.Expressions;
using Marten.Linq;
using Reports.Core;

namespace Reports.Application.Queries;

public class GetReportQuery(DateTime date) : ICompiledQuery<Report?>
{
    public DateTime Date = date;
    
    public Expression<Func<IMartenQueryable<Report>, Report>> QueryIs()
    {
        return report => report.FirstOrDefault(r => r.Year == Date.Year && r.Month == Date.Month);
    }
}