using System.Text.Json;
using ArkFunds.Reports.Core;
using ArkFunds.Reports.Infrastructure;

namespace ArkFunds.Tests.Reports;

public class CsvReportParserTests
{
    [Fact]
    public async void CsvReport_SingleLine_Correct()
    {
        var parser = new CsvReportParser();
        var testLine =
            "date,fund,company,ticker,cusip,shares,\"market value ($)\",\"weight (%)\"\n" +
            "05/21/2024,ARKK,\"COINBASE GLOBAL INC -CLASS A\",COIN,19260Q107,\"2,543,768\",\"$572,831,115.92\",8.58%\n";
        var expected = new List<Holdings>
        {
            new()
            {
                Date = new DateOnly(2024, 5, 21),
                Fund = "ARKK",
                Company = "COINBASE GLOBAL INC -CLASS A",
                Ticker = "COIN",
                Cusip = "19260Q107",
                Shares = 2543768,
                MarketValue = new MarketValueCurrency
                {
                    Value = 572831115.92,
                    Currency = "$"
                },
                Weight = 8.58
            }
        };


        (await new WhenCsvReportParser(parser)
                .IsGivenInput(testLine)
                .Then())
            .ResultIsNotEmpty()
            .ResultHasLength(1)
            .ResultIsEqualToExpected(expected);
    }

    [Fact]
    public async void CsvReport_SingleLine_Wrong()
    {
        var parser = new CsvReportParser();
        var testLine =
            "date,fund,company,ticker,cusip,shares,\"market value ($)\",\"weight (%)\"\n" +
            "WRONG_INPUT,ARKK,\"COINBASE GLOBAL INC -CLASS A\",COIN,19260Q107,\"2,543,768\",\"$572,831,115.92\",8.58%\n";

        (await new WhenCsvReportParser(parser)
                .IsGivenInput(testLine)
                .Then())
            .ResultWithException();
    }

    [Fact]
    public async void CsvReport_MultipleLines_Correct()
    {
        var parser = new CsvReportParser();
        var testLine =
            "date,fund,company,ticker,cusip,shares,\"market value ($)\",\"weight (%)\"\n" +
            "05/21/2024,ARKK,\"COINBASE GLOBAL INC -CLASS A\",COIN,19260Q107,\"2,543,768\",\"$572,831,115.92\",8.58%\n" +
            "05/21/2024,ARKK,\"ROKU INC\",ROKU,77543R102,\"9,367,268\",\"$555,197,974.36\",8.31%\n" +
            "05/21/2024,ARKK,\"BLOCK INC\",SQ,852234103,\"5,669,374\",\"$416,925,763.96\",6.24%\n" +
            "05/21/2024,ARKK,\"UIPATH INC - CLASS A\",PATH,90364P105,\"18,791,584\",\"$381,657,071.04\",5.71%\n" +
            "05/21/2024,ARKK,\"ROBLOX CORP -CLASS A\",RBLX,771049103,\"9,759,542\",\"$327,823,015.78\",4.91%\n" +
            "05/21/2024,ARKK,\"ROBINHOOD MARKETS INC - A\",HOOD,770700102,\"15,278,196\",\"$318,397,604.64\",4.77%\n" +
            "05/21/2024,ARKK,\"CRISPR THERAPEUTICS AG\",CRSP,H17182108,\"5,695,694\",\"$315,940,146.18\",4.73%\n";

        (await new WhenCsvReportParser(parser)
                .IsGivenInput(testLine)
                .Then())
            .ResultIsNotEmpty()
            .ResultHasLength(7);
    }


    [Fact]
    public async void CsvReport_MultipleLines_Wrong()
    {
        var parser = new CsvReportParser();
        var testLine =
            "date,fund,company,ticker,cusip,shares,\"market value ($)\",\"weight (%)\"\n" +
            "05/21/2024,ARKK,\"COINBASE GLOBAL INC -CLASS A\",COIN,19260Q107,\"2,543,768\",\"$572,831,115.92\",8.58%\n" +
            "05/21/2024,ARKK,\"ROKU INC\",ROKU,77543R102,\"9,367,268\",\"$555,197,974.36\",8.31%\n" +
            "05/21/2024,ARKK,\"BLOCK INC\",SQ,852234103,\"5,669,374\",\"$416,925,763.96\",6.24%\n" +
            "05/21/2024,ARKK,\"UIPATH INC - CLASS A\",PATH,90364P105,\"18,791,584\",\"$381,657,071.04\",5.71%\n" +
            "05/21/2024,ARKK,\"ROBLOX CORP -CLASS A\",RBLX,771049103,\"WRONG_INPUT\",\"$327,823,015.78\",4.91%\n" +
            "05/21/2024,ARKK,\"ROBINHOOD MARKETS INC - A\",HOOD,770700102,\"15,278,196\",\"$318,397,604.64\",4.77%\n" +
            "05/21/2024,ARKK,\"CRISPR THERAPEUTICS AG\",CRSP,H17182108,\"5,695,694\",\"$315,940,146.18\",4.73%\n";

        (await new WhenCsvReportParser(parser)
                .IsGivenInput(testLine)
                .Then())
            .ResultWithException();
    }

    [Fact]
    public async void CsvReport_EmptyLine_Correct()
    {
        var parser = new CsvReportParser();
        var testLine = "";

        (await new WhenCsvReportParser(parser)
                .IsGivenInput(testLine)
                .Then())
            .ResultIsEmpty();
    }

    [Fact]
    public async void CsvReport_EmptyLines_Correct()
    {
        var parser = new CsvReportParser();
        var testLine = "\n";

        (await new WhenCsvReportParser(parser)
                .IsGivenInput(testLine)
                .Then())
            .ResultIsEmpty();
    }

    [Theory]
    // Wrong date format
    [InlineData("date,fund,company,ticker,cusip,shares,\"market value ($)\",\"weight (%)\"\n" +
                "051/21/2024,ARKK,\"COINBASE GLOBAL INC -CLASS A\",COIN,19260Q107,\"2,543,768\",\"$572,831,115.92\",8.58%\n")]
    // Wrong shares format
    [InlineData("date,fund,company,ticker,cusip,shares,\"market value ($)\",\"weight (%)\"\n" +
                "01/21/2024,ARKK,\"COINBASE GLOBAL INC -CLASS A\",COIN,19260Q107,\"2,asd,768\",\"$572,831,115.92\",8.58%\n")]
    // Wrong market value format
    [InlineData("date,fund,company,ticker,cusip,shares,\"market value ($)\",\"weight (%)\"\n" +
                "01/21/2024,ARKK,\"COINBASE GLOBAL INC -CLASS A\",COIN,19260Q107,\"2,124,768\",\"$572,831,asf.92\",8.58%\n")]
    // Wrong weight format
    [InlineData("date,fund,company,ticker,cusip,shares,\"market value ($)\",\"weight (%)\"\n" +
                "01/21/2024,ARKK,\"COINBASE GLOBAL INC -CLASS A\",COIN,19260Q107,\"2,124,768\",\"$572,831,115.92\",s.58%\n")]
    public async void CsvReport_LineWithWrongData_Wrong(string testLine)
    {
        var parser = new CsvReportParser();


        (await new WhenCsvReportParser(parser)
                .IsGivenInput(testLine)
                .Then())
            .ResultWithException();
    }
}

internal sealed class WhenCsvReportParser(CsvReportParser parser)
{
    private string input = "";
    private List<Holdings> output = [];
    private bool wasException;

    public WhenCsvReportParser IsGivenInput(string testLine)
    {
        input = testLine;
        return this;
    }

    public async Task<WhenCsvReportParser> Then()
    {
        try
        {
            output = await parser.ParseAsync(input);
        }
        catch (Exception)
        {
            wasException = true;
        }

        return this;
    }

    public WhenCsvReportParser ResultIsNotEmpty()
    {
        Assert.NotEmpty(output);
        return this;
    }

    public WhenCsvReportParser ResultIsEmpty()
    {
        Assert.Empty(output);
        return this;
    }

    public WhenCsvReportParser ResultHasLength(int length)
    {
        Assert.Equal(length, output.Count);
        return this;
    }

    public WhenCsvReportParser ResultIsEqualToExpected(List<Holdings> expected)
    {
        var outputJson = JsonSerializer.Serialize(output);
        var expectedJson = JsonSerializer.Serialize(expected);
        Assert.Equal(expectedJson, outputJson);
        return this;
    }

    public WhenCsvReportParser ResultWithException()
    {
        Assert.True(wasException, "Parser did not throw an exception.");
        return this;
    }
}