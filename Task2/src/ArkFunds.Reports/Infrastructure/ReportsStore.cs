using Marten;

namespace ArkFunds.Reports.Infrastructure;

public class ReportsStore(StoreOptions options) : DocumentStore(options)
{
}