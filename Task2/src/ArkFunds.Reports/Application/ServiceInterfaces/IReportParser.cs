using ArkFunds.Reports.Core;
using ArkFunds.Reports.Infrastructure;

namespace ArkFunds.Reports.Application.ServiceInterfaces;

public interface IReportParser
{
    public Task<List<Holdings>> ParseAsync(string rawData);
}