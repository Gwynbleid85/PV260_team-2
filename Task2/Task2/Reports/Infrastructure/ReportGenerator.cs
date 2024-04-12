using Reports.Application.ServiceInterfaces;
using Reports.Core;

namespace Reports.Infrastructure;

public class ReportGenerator : IReportGenerator
{
    public Report GenerateReport(int year, int month)
    {
        //TODO: Implement report generation
        return new Report
        {
            Id = Guid.NewGuid(),
            Year = year,
            Month = month
        };
    }
}