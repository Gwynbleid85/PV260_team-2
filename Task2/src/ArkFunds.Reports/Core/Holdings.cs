using ArkFunds.Reports.Dto;

namespace ArkFunds.Reports.Infrastructure;

public record Holdings
{
    public DateOnly Date { get; set; }
    public string Fund { get; set; }
    public string Company { get; set; }
    public string Ticker { get; set; }
    public string Cusip { get; set; }
    public int Shares { get; set; }
    
    public int SharesDifference { get; set; } // TODO: this should be percentage change? we shouldn't calculate it on FE
    public MarketValueCurrency MarketValue { get; set; } = new ();
    public double Weight { get; set; }
}