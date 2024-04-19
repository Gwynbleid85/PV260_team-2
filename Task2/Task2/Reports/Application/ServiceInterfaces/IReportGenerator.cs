using Reports.Core;

namespace Reports.Application.ServiceInterfaces;

public interface IReportGenerator
{
    Task<Report> GenerateReportAsync(int year, int month, Report? previousReport);
}