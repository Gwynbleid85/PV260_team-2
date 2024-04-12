namespace Reports.Core.Events;

public record ReportGenerated(Guid Id, int Year, int Month);