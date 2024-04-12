using CommunityToolkit.Diagnostics;
using Mapster;
using Marten;
using Reports.Application.Queries;
using Reports.Application.ServiceInterfaces;
using Reports.Core.Events;

namespace Reports.Application.Commands;

public class GenerateReportCommandHandler
{
    public static async Task LoadAsync(GenerateReportCommand command, IQuerySession session,
        CancellationToken cancellationToken)
    {
        var time = new DateTime(command.Year, command.Month, 1);
        var report = await session.QueryAsync(new GetCurrentReportQuery(time), cancellationToken);
        Guard.IsFalse(report.Any(), "Report already exists for this month");
    }

    public static async Task<ReportGenerated> Handle(GenerateReportCommand command, IReportGenerator reportGenerator,
        IDocumentSession session, CancellationToken cancellationToken)
    {
        var report = reportGenerator.GenerateReport(command.Year, command.Month);

        session.Store(report);
        await session.SaveChangesAsync(cancellationToken);

        return report.Adapt<ReportGenerated>();
    }
}