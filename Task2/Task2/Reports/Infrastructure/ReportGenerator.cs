using System.Globalization;
using System.Net;
using CsvHelper;
using CsvHelper.Configuration;
using Reports.Application.ServiceInterfaces;
using Reports.Core;
using Reports.Dto;

namespace Reports.Infrastructure;

public class ReportGenerator : IReportGenerator
{
    public Report GenerateReport(int year, int month)
    {
        var csvUrl = "https://ark-funds.com/wp-content/uploads/funds-etf-csv/ARK_INNOVATION_ETF_ARKK_HOLDINGS.csv";

        //string csvContent = GetCsv(csvUrl).Result; //TODO remove blocking call, and fix 403 error
        string csvContent = ReadFile("/home/davidnovak/Downloads/ARK_INNOVATION_ETF_ARKK_HOLDINGS.csv");


        List<HoldingsDto> currentHoldingsDtos = ParseCsv(csvContent);


        List<Holdings> currentHoldings= ParseHoldingsDtos(currentHoldingsDtos);

        Report oldReport;

        List<Holdings> oldHoldings = []; //todo how to get old holdings?

        return new Report
        {
            Id = Guid.NewGuid(),
            Year = year,
            Month = month,
            NewPositions = FindNewPositions(oldHoldings, currentHoldings),
            IncreaedPositions = FindIncreasedPositions(oldHoldings, currentHoldings),
            ReducedPositions = FindReducedPositions(oldHoldings, currentHoldings)
        };
    }

    private List<Holdings> FindReducedPositions(List<Holdings> oldHoldings, List<Holdings> holdings)
    {
        List<Holdings> reducedPositions = new List<Holdings>();

        foreach (var holding in holdings)
        {
            var oldHolding = oldHoldings.FirstOrDefault(old => old.Ticker == holding.Ticker);
        
            if (oldHolding != null && holding.Shares < oldHolding.Shares)
            {
                holding.SharesDifference = holding.Shares - oldHolding.Shares;
                reducedPositions.Add(holding);
            }
        }

        return reducedPositions;    }

    private List<Holdings> FindIncreasedPositions(List<Holdings> oldHoldings, List<Holdings> holdings)
    {
        List<Holdings> increasedPositions = new List<Holdings>();

        foreach (var holding in holdings)
        {
            var oldHolding = oldHoldings.FirstOrDefault(old => old.Ticker == holding.Ticker);
        
            if (oldHolding != null && holding.Shares >= oldHolding.Shares)
            {
                holding.SharesDifference = holding.Shares - oldHolding.Shares;
                increasedPositions.Add(holding);
            }
        }

        return increasedPositions;
    }

    private List<Holdings> FindNewPositions(List<Holdings> oldHoldings, List<Holdings> holdings)
    {
        List<Holdings> newPositions = holdings
            .Where(h => oldHoldings
                .All(old => old.Ticker != h.Ticker))
            .ToList();

        return newPositions;
    }

    private List<HoldingsDto> ParseCsv(string csvContent)
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            IgnoreBlankLines = true,
            ShouldSkipRecord = record => record.Row.ColumnCount <= 1
        };
        using (var reader = new StringReader(csvContent))
        using (var csv = new CsvReader(reader, config))
        {
            return csv.GetRecords<HoldingsDto>().ToList();
        }
    }
    
    private static async Task<string> GetCsv(string url)
    {
        try
        {
            using HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"HTTP request failed: {ex.Message}");
            throw;
        }
    }

    private static string ReadFile(string path)
    {
        if (File.Exists(path))
        {
            return File.ReadAllText(path);
        }

        throw new ArgumentException($"File '{path}' does not exist.");
    }

    private static List<Holdings> ParseHoldingsDtos(List<HoldingsDto> holdingsDtos)
    {
        List<Holdings> parsedHoldings = [];
        foreach (var holdingsDto in holdingsDtos)
        {
            parsedHoldings.Add(ParseHoldingsDto(holdingsDto));
        }

        return parsedHoldings;
    }


    private static Holdings ParseHoldingsDto(HoldingsDto holdingsDto)
    {
        var date = DateOnly.Parse(holdingsDto.Date);
        var shares = int.Parse(holdingsDto.Shares.Replace(",", ""));
        var marketValue = double.Parse(holdingsDto.MarketValue.Replace("$", "").Replace(",", ""));
        var weight = double.Parse(holdingsDto.Weight.Replace("%", ""));

        return new Holdings
        {
            Date = date,
            Fund = holdingsDto.Fund,
            Company = holdingsDto.Company,
            Ticker = holdingsDto.Ticker,
            Cusip = holdingsDto.Cusip,
            Shares = shares,
            MarketValue = marketValue,
            Weight = weight
        };
    }
}