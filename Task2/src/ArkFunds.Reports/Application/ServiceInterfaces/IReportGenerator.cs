using ArkFunds.Reports.Core;

namespace ArkFunds.Reports.Application.ServiceInterfaces;

public interface IReportGenerator
{
    Task<Report> GenerateReportAsync(int year, int month, Report? previousReport);
}