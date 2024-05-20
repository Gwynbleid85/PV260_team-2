using ArkFunds.Reports.Application.Commands;
using ArkFunds.Reports.Application.ServiceInterfaces;
using Coravel.Invocable;
using Wolverine;

namespace ArkFunds.Reports.Infrastructure;

public class ScheduledReportGenerationInvocable(IMessageBus bus, ITimeProvider timeProvider) : IInvocable
{
    public async Task Invoke()
    {
        Console.WriteLine("Scheduled report generation invocable invoked");
        var now = timeProvider.GetCurrentTime();
        var command = new GenerateReportCommand(now.Year, now.Month);
        await bus.InvokeAsync(command);
    }
}