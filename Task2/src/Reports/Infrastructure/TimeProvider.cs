using Reports.Application.ServiceInterfaces;

namespace Reports.Infrastructure;

public class TimeProvider() : ITimeProvider
{
    public DateTime GetCurrentTime()
    {
        return DateTime.Now;
    }
}