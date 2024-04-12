using Reports.Core;

namespace Reports.Application.ServiceInterfaces;

public interface IReportGenerator
{
    Report GenerateReport(int year, int month);
}