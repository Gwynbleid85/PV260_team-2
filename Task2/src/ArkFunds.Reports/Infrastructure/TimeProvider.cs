using ArkFunds.Reports.Application.ServiceInterfaces;

namespace ArkFunds.Reports.Infrastructure;

public class TimeProvider() : ITimeProvider
{
    public DateTime GetCurrentTime()
    {
        return DateTime.Now;
    }
}