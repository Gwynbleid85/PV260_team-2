using ArkFunds.Reports.Application.ServiceInterfaces;

namespace ArkFunds.Reports.Infrastructure;

public class CsvReportReader(IHttpClientFactory httpClientFactory) : IReportReader
{
    public async Task<string> GetAsync(string path)
    {
        using var client = httpClientFactory.CreateClient(DependencyInjection.ArkHttpClientName);
        
        var response = await client.GetAsync(path);
        response.EnsureSuccessStatusCode();
        
        var responseBody = await response.Content.ReadAsStringAsync();
        
        return responseBody;
    }
}