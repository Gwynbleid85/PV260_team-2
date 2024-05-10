using Mapster;
using Wolverine;

namespace ArkFunds.Emails.Infrastructure.AntiCorruptionLayer.Reports;

public record ReportGenerated(Guid Id, int Year, int Month);

public class AntiCorruptionLayerReportGeneratedHandler
{
    public static async Task Handle(ArkFunds.Reports.Core.Events.ReportGenerated externalEvent, IMessageBus bus)
    {
        var internalEvent = externalEvent.Adapt<ReportGenerated>();
        await bus.InvokeAsync(internalEvent);
    }
}