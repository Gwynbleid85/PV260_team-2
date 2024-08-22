namespace ArkFunds.Reports.Application.ServiceInterfaces;

public interface IReportReader
{
    public Task<string> GetAsync(string path);
}