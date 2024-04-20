using System.Globalization;
using ArkFunds.Reports.Application.ServiceInterfaces;
using ArkFunds.Reports.Core;
using ArkFunds.Reports.Dto;
using CommunityToolkit.Diagnostics;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Configuration;

namespace ArkFunds.Reports.Infrastructure;

public class CsvReportGenerator : IReportGenerator
{
    private static readonly List<string> SupportedHoldingCultures = ["en-US"];

    private readonly string csvRelativeUrl;
    private readonly IHttpClientFactory httpClientFactory;
    
    public CsvReportGenerator(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        this.httpClientFactory = httpClientFactory;
        csvRelativeUrl = configuration.GetSection("ReportsSettings")["CsvRelativeUrl"] ?? string.Empty;
        
        Guard.IsNotNullOrEmpty(csvRelativeUrl, "Csv relative url");
    }

    public async Task<Report> GenerateReportAsync(int year, int month, Report? previousReport)
    {
        var csvContent = await GetCsvAsync(csvRelativeUrl);

        var currentHoldings = ParseCsvDocument(csvContent);

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
    
    private async Task<string> GetCsvAsync(string url)
    {
        using var client = httpClientFactory.CreateClient(DependencyInjection.ArkClientName);
        
        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        
        var responseBody = await response.Content.ReadAsStringAsync();
        
        return responseBody;
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
                reducedPositions.Add(holding);
            }
            else
            {
                holding.SharesDifference = holding.Shares - oldHolding.Shares;
                increasedPositions.Add(holding);
            }
        }
    }

    private List<Holdings> ParseCsvDocument(string csvContent)
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            IgnoreBlankLines = true,
            ShouldSkipRecord = record => record.Row.ColumnCount <= 1
        };
        using var reader = new StringReader(csvContent);
        using var csv = new CsvReader(reader, config);
        
        return csv.GetRecords<HoldingsDto>()
            .Select(holdingsDto => ParseHoldings(holdingsDto))
            .ToList();
    }

    private Holdings ParseHoldings(HoldingsDto holdingsDto)
    {
        // get supported culture that uses the specific holding's currency symbol
        var allCultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
        var culture = allCultures
            .Where(x => SupportedHoldingCultures.Contains(x.Name))
            .FirstOrDefault(c => holdingsDto.MarketValue.Contains(c.NumberFormat.CurrencySymbol));
        
        Guard.IsNotNull(culture, "Holding's culture");

        var date = DateOnly.Parse(holdingsDto.Date, culture);
        var shares = int.Parse(holdingsDto.Shares, NumberStyles.AllowThousands, culture);
        var marketValue = double.Parse(holdingsDto.MarketValue,
            NumberStyles.AllowThousands | NumberStyles.AllowCurrencySymbol | NumberStyles.AllowDecimalPoint,
            culture);
        var weight = double.Parse(holdingsDto.Weight.Replace(culture.NumberFormat.PercentSymbol, ""), culture);
        
        return new Holdings
        {
            Date = date,
            Fund = holdingsDto.Fund,
            Company = holdingsDto.Company,
            Ticker = holdingsDto.Ticker,
            Cusip = holdingsDto.Cusip,
            Shares = shares,
            MarketValue = new MarketValueCurrency
            {
                Value = marketValue,
                Currency = culture.NumberFormat.CurrencySymbol
            },
            Weight = weight
        };
    }
}