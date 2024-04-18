namespace Reports.Infrastructure;

public record Holdings
{
    public DateOnly Date { get; set; }
    public string Fund { get; set; }
    public string Company { get; set; }
    public string Ticker { get; set; }
    public string Cusip { get; set; }
    public int Shares { get; set; }
    
    public int SharesDifference { get; set; } = 0;
    public double MarketValue { get; set; }
    public double Weight { get; set; }
}