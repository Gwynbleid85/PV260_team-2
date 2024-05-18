using CommunityToolkit.Diagnostics;
using Mapster;
using Marten;
using ArkFunds.Reports.Application.Queries.CompiledQueries;
using ArkFunds.Reports.Application.ServiceInterfaces;
using ArkFunds.Reports.Core;
using ArkFunds.Reports.Core.Events;
using Wolverine;

namespace ArkFunds.Reports.Application.Commands;

public class GenerateReportCommandHandler
{
    public static async Task LoadAsync(GenerateReportCommand command, IQuerySession session,
        CancellationToken cancellationToken)
    {
        var time = new DateTime(command.Year, command.Month, 1);
        var report = await session.QueryAsync(new GetCurrentReportQuery(time), cancellationToken);
        //Guard.IsFalse(report.Any(), "Report already exists for this month");
    }

    public static async Task<ReportGenerated> Handle(GenerateReportCommand command, IReportGenerator reportGenerator,
        IMessageBus bus,
        IDocumentSession session, CancellationToken cancellationToken)
    {
        var previousMonthDate = new DateTime(command.Year, command.Month, 1)
            .AddMonths(-1);

        var previousReport = await session.QueryAsync(new GetReportQuery(previousMonthDate), cancellationToken) ??
                             new Report();
        var report = await reportGenerator.GenerateReportAsync(command.Year, command.Month, previousReport);

        session.Store(report);
        await session.SaveChangesAsync(cancellationToken);

        Console.WriteLine("=== Event ===");
        Console.WriteLine("Report generated");
        Console.WriteLine("=============");


        var newEvent = report.Adapt<ReportGenerated>();
        await bus.InvokeAsync(newEvent, cancellationToken);

        return newEvent;
    }
}