using ArkFunds.Reports.Application.ServiceInterfaces;

namespace ArkFunds.Reports.Infrastructure;

public class FileReportReader : IReportReader
{
    public async Task<string> GetAsync(string path)
    {
        using var reader = new StreamReader(path);
        return await reader.ReadToEndAsync();
    }
}