using Marten;

namespace Reports.Infrastructure;

public class ReportsStore(StoreOptions options) : DocumentStore(options)
{
}