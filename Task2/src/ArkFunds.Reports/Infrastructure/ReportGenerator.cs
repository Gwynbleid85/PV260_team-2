using ArkFunds.Reports.Application.ServiceInterfaces;
using ArkFunds.Reports.Core;
using CommunityToolkit.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace ArkFunds.Reports.Infrastructure;

public class ReportGenerator : IReportGenerator
{
    private readonly string documentPath;

    private readonly IReportParser reportParser;
    private readonly IReportReader reportReader;
    
    public ReportGenerator(IConfiguration configuration, IReportParser reportParser, IReportReader reportReader)
    {
        this.reportParser = reportParser;
        this.reportReader = reportReader;
        
        documentPath = configuration.GetSection("ReportsSettings")["DocumentPath"] ?? string.Empty;
        Guard.IsNotNullOrEmpty(documentPath, "Document path");
    }

    public async Task<Report> GenerateReportAsync(int year, int month, Report? previousReport)
    {
        var rawData = await reportReader.GetAsync(documentPath);
        var currentHoldings = await reportParser.ParseAsync(rawData);

        var oldHoldings = previousReport?.IncreaedPositions
            .Concat(previousReport.ReducedPositions)
            .Concat(previousReport.NewPositions)
            .ToList();
        
        var newReport = new Report
        {
            Id = Guid.NewGuid(),
            Year = year,
            Month = month
        };

        CalculatePositions(oldHoldings, 
            currentHoldings, 
            newReport.ReducedPositions, 
            newReport.IncreaedPositions,
            newReport.NewPositions);

        return newReport;
    }

    private void CalculatePositions(
        List<Holdings>? oldHoldings,
        List<Holdings> holdings,
        List<Holdings> reducedPositions,
        List<Holdings> increasedPositions,
        List<Holdings> newPositions)
    {
        if (oldHoldings == null)
        {
            newPositions.AddRange(holdings);
            return;
        }
        
        foreach (var holding in holdings)
        {
            var oldHolding = oldHoldings.FirstOrDefault(old => old.Ticker == holding.Ticker);

            if (oldHolding == null)
            {
                newPositions.Add(holding);
            } else if (holding.Shares < oldHolding.Shares)
            {
                holding.SharesDifference = holding.Shares - oldHolding.Shares;
                holding.SharesPercentageChange = (double)holding.Shares / oldHolding.Shares - 1;
                reducedPositions.Add(holding);
            }
            else
            {
                holding.SharesDifference = holding.Shares - oldHolding.Shares;
                holding.SharesPercentageChange = 1 - (double)oldHolding.Shares / holding.Shares;
                increasedPositions.Add(holding);
            }
        }
    }
}