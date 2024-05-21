using System.Text.Json;
using ArkFunds.Reports.Application.ServiceInterfaces;
using ArkFunds.Reports.Core;
using ArkFunds.Reports.Infrastructure;
using Microsoft.Extensions.Configuration;
using Moq;

namespace ArkFunds.Tests.Reports;

public class ReportGeneratorTests
{
    private IConfiguration configuration;
    private Holdings testHoldings;

    public ReportGeneratorTests()
    {
        var inMemorySettings = new Dictionary<string, string>
        {
            {
                "ReportsSettings:DocumentPath",
                "https://ark-funds.com/wp-content/uploads/funds-etf-csv/ARK_INNOVATION_ETF_ARKK_HOLDINGS.csv"
            },
            { "ReportsSettings:SourceType", "0" }
        };
        configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings!)
            .Build();

        testHoldings = new Holdings
        {
            Ticker = "Ticker1",
            Company = "Company",
            Cusip = "Cusip",
            Date = new DateOnly(2021, 1, 1),
            Fund = "Fund",
            MarketValue = new MarketValueCurrency
            {
                Currency = "$",
                Value = 1
            },
            Shares = 1000,
            Weight = 1,
            SharesDifference = 1,
            SharesPercentageChange = 1
        };
    }

    [Fact]
    public async void GenerateReport_FromSimpleLine_Success()
    {
        var reportParserMock = new Mock<IReportParser>();
        reportParserMock
            .Setup(parser => parser.ParseAsync(It.IsAny<string>()))
            .Returns(Task.FromResult(new List<Holdings> { testHoldings }));

        var reportReaderMock = new Mock<IReportReader>();
        reportReaderMock
            .Setup(reader => reader.GetAsync(It.IsAny<string>()))
            .Returns(Task.FromResult(""));

        var generator = new ReportGenerator(configuration, reportParserMock.Object, reportReaderMock.Object);
        var year = 2021;
        var month = 1;


        (await new WhenReportGenerator(generator)
                .IsGivenYear(year)
                .IsGivenMonth(month)
                .IsGivenPreviousReport(null)
                .Then())
            .ResultsWithReport(new Report
            {
                Id = Guid.Empty,
                Year = year,
                Month = month,
                NewPositions = new List<Holdings> { testHoldings }
            });
    }


    [Fact]
    public async void GenerateReport_FromSimpleHolding_WithNewPosition()
    {
        var reportParserMock = new Mock<IReportParser>();
        reportParserMock
            .Setup(parser => parser.ParseAsync(It.IsAny<string>()))
            .Returns(Task.FromResult(new List<Holdings> { testHoldings }));

        var reportReaderMock = new Mock<IReportReader>();
        reportReaderMock
            .Setup(reader => reader.GetAsync(It.IsAny<string>()))
            .Returns(Task.FromResult(""));

        var generator = new ReportGenerator(configuration, reportParserMock.Object, reportReaderMock.Object);
        var year = 2021;
        var month = 1;
        var previousReport = new Report
        {
            Id = Guid.NewGuid(),
            Year = year,
            Month = month - 1,
            NewPositions = new List<Holdings>
            {
                new()
                {
                    Ticker = "Ticker2",
                    Company = "Company",
                    Cusip = "Cusip",
                    Date = new DateOnly(2021, 1, 1),
                    Fund = "Fund",
                    MarketValue = new MarketValueCurrency
                    {
                        Currency = "$",
                        Value = 1
                    },
                    Shares = 1,
                    Weight = 1,
                    SharesDifference = 1,
                    SharesPercentageChange = 1
                }
            }
        };

        (await new WhenReportGenerator(generator)
                .IsGivenYear(year)
                .IsGivenMonth(month)
                .IsGivenPreviousReport(previousReport)
                .Then())
            .ResultWithNewPositions(1)
            .ResultWithIncreasedPosition(0)
            .ResultWithReducedPosition(0);
    }

    [Fact]
    public async void GenerateReport_FromSimpleHolding_WithNoNewPosition()
    {
        var reportParserMock = new Mock<IReportParser>();
        reportParserMock
            .Setup(parser => parser.ParseAsync(It.IsAny<string>()))
            .Returns(Task.FromResult(new List<Holdings> { testHoldings }));

        var reportReaderMock = new Mock<IReportReader>();
        reportReaderMock
            .Setup(reader => reader.GetAsync(It.IsAny<string>()))
            .Returns(Task.FromResult(""));

        var generator = new ReportGenerator(configuration, reportParserMock.Object, reportReaderMock.Object);
        var year = 2021;
        var month = 1;
        var previousReport = new Report
        {
            Id = Guid.NewGuid(),
            Year = year,
            Month = month - 1,
            NewPositions = new List<Holdings>
            {
                new()
                {
                    Ticker = "Ticker1",
                    Company = "Company",
                    Cusip = "Cusip",
                    Date = new DateOnly(2021, 1, 1),
                    Fund = "Fund",
                    MarketValue = new MarketValueCurrency
                    {
                        Currency = "$",
                        Value = 1
                    },
                    Shares = 1,
                    Weight = 1,
                    SharesDifference = 1,
                    SharesPercentageChange = 1
                }
            }
        };

        (await new WhenReportGenerator(generator)
                .IsGivenYear(year)
                .IsGivenMonth(month)
                .IsGivenPreviousReport(previousReport)
                .Then())
            .ResultWithNewPositions(0);
    }

    [Fact]
    public async void GenerateReport_FromSimpleHolding_WithReducedPosition()
    {
        var reportParserMock = new Mock<IReportParser>();
        reportParserMock
            .Setup(parser => parser.ParseAsync(It.IsAny<string>()))
            .Returns(Task.FromResult(new List<Holdings> { testHoldings }));

        var reportReaderMock = new Mock<IReportReader>();
        reportReaderMock
            .Setup(reader => reader.GetAsync(It.IsAny<string>()))
            .Returns(Task.FromResult(""));

        var generator = new ReportGenerator(configuration, reportParserMock.Object, reportReaderMock.Object);
        var year = 2021;
        var month = 1;
        var previousReport = new Report
        {
            Id = Guid.NewGuid(),
            Year = year,
            Month = month - 1,
            NewPositions = new List<Holdings>
            {
                new()
                {
                    Ticker = "Ticker1",
                    Company = "Company",
                    Cusip = "Cusip",
                    Date = new DateOnly(2021, 1, 1),
                    Fund = "Fund",
                    MarketValue = new MarketValueCurrency
                    {
                        Currency = "$",
                        Value = 1234
                    },
                    Shares = 9000,
                    Weight = 1,
                    SharesDifference = 1,
                    SharesPercentageChange = 1
                }
            }
        };

        (await new WhenReportGenerator(generator)
                .IsGivenYear(year)
                .IsGivenMonth(month)
                .IsGivenPreviousReport(previousReport)
                .Then())
            .ResultWithNewPositions(0)
            .ResultWithReducedPosition(1)
            .ResultWithIncreasedPosition(0);
    }

    [Fact]
    public async void GenerateReport_FromSimpleHolding_WithIncreasedPosition()
    {
        var reportParserMock = new Mock<IReportParser>();
        reportParserMock
            .Setup(parser => parser.ParseAsync(It.IsAny<string>()))
            .Returns(Task.FromResult(new List<Holdings> { testHoldings }));

        var reportReaderMock = new Mock<IReportReader>();
        reportReaderMock
            .Setup(reader => reader.GetAsync(It.IsAny<string>()))
            .Returns(Task.FromResult(""));

        var generator = new ReportGenerator(configuration, reportParserMock.Object, reportReaderMock.Object);
        var year = 2021;
        var month = 1;
        var previousReport = new Report
        {
            Id = Guid.NewGuid(),
            Year = year,
            Month = month - 1,
            NewPositions = new List<Holdings>
            {
                new()
                {
                    Ticker = "Ticker1",
                    Company = "Company",
                    Cusip = "Cusip",
                    Date = new DateOnly(2021, 1, 1),
                    Fund = "Fund",
                    MarketValue = new MarketValueCurrency
                    {
                        Currency = "$",
                        Value = 1234
                    },
                    Shares = 100,
                    Weight = 1,
                    SharesDifference = 1,
                    SharesPercentageChange = 1
                }
            }
        };

        (await new WhenReportGenerator(generator)
                .IsGivenYear(year)
                .IsGivenMonth(month)
                .IsGivenPreviousReport(previousReport)
                .Then())
            .ResultWithNewPositions(0)
            .ResultWithReducedPosition(0)
            .ResultWithIncreasedPosition(1);
    }
}

internal sealed class WhenReportGenerator(IReportGenerator generator)
{
    private int year;
    private int month;
    private Report? previousReport = null;
    private Report output;

    public WhenReportGenerator IsGivenYear(int testYear)
    {
        year = testYear;
        return this;
    }

    public WhenReportGenerator IsGivenMonth(int testMonth)
    {
        month = testMonth;
        return this;
    }

    public WhenReportGenerator IsGivenPreviousReport(Report? testPreviousReport)
    {
        previousReport = testPreviousReport;
        return this;
    }

    public async Task<WhenReportGenerator> Then()
    {
        output = await generator.GenerateReportAsync(year, month, previousReport);
        return this;
    }

    public WhenReportGenerator ResultsWithReport(Report expectedReport)
    {
        expectedReport.Id = output.Id;
        var outputJson = JsonSerializer.Serialize(output);
        var expectedJson = JsonSerializer.Serialize(expectedReport);
        Assert.Equal(outputJson, expectedJson);
        return this;
    }

    public WhenReportGenerator ResultWithNewPositions(int count)
    {
        Assert.Equal(count, output.NewPositions.Count);
        return this;
    }

    public WhenReportGenerator ResultWithIncreasedPosition(int count)
    {
        Assert.Equal(count, output.IncreaedPositions.Count);
        return this;
    }

    public WhenReportGenerator ResultWithReducedPosition(int count)
    {
        Assert.Equal(count, output.ReducedPositions.Count);
        return this;
    }
}