using ArkFunds.Reports.Infrastructure;

namespace ArkFunds.Reports.Core;

public class Report
{
    public Guid Id { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public List<Holdings> NewPositions { get; set; } = [];
    public List<Holdings> IncreaedPositions { get; set; } = [];
    public List<Holdings> ReducedPositions { get; set; } = [];
}