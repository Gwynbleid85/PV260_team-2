using System.Globalization;
using ArkFunds.Reports.Application.ServiceInterfaces;
using ArkFunds.Reports.Core;
using ArkFunds.Reports.Core.Dto;
using CommunityToolkit.Diagnostics;
using CsvHelper;
using CsvHelper.Configuration;

namespace ArkFunds.Reports.Infrastructure;

public class CsvReportParser : IReportParser
{
    private static readonly List<string> SupportedHoldingCultures = ["en-US"];

    public async Task<List<Holdings>> ParseAsync(string rawData)
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            IgnoreBlankLines = true,
            ShouldSkipRecord = record => record.Row.ColumnCount <= 1
        };
        using var reader = new StringReader(rawData);
        using var csv = new CsvReader(reader, config);
        
        var holdingDtos = await csv.GetRecordsAsync<CsvHoldingsDto>().ToListAsync();
        return holdingDtos?.Select(ParseHoldings).ToList() ?? [];
    }
    
    private Holdings ParseHoldings(CsvHoldingsDto csvHoldingsDto)
    {
        // get supported culture that uses the specific holding's currency symbol
        var allCultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
        var culture = allCultures
            .Where(x => SupportedHoldingCultures.Contains(x.Name))
            .FirstOrDefault(c => csvHoldingsDto.MarketValue.Contains(c.NumberFormat.CurrencySymbol));
        
        Guard.IsNotNull(culture, "Holding's culture");

        var date = DateOnly.Parse(csvHoldingsDto.Date, culture);
        var shares = int.Parse(csvHoldingsDto.Shares, NumberStyles.AllowThousands, culture);
        var marketValue = double.Parse(csvHoldingsDto.MarketValue,
            NumberStyles.AllowThousands | NumberStyles.AllowCurrencySymbol | NumberStyles.AllowDecimalPoint,
            culture);
        var weight = double.Parse(csvHoldingsDto.Weight.Replace(culture.NumberFormat.PercentSymbol, ""), culture);
        
        return new Holdings
        {
            Date = date,
            Fund = csvHoldingsDto.Fund,
            Company = csvHoldingsDto.Company,
            Ticker = csvHoldingsDto.Ticker,
            Cusip = csvHoldingsDto.Cusip,
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