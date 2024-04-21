using ArkFunds.App.Web.Api;
using Microsoft.AspNetCore.Components;

namespace ArkFunds.App.Web.Components;

public partial class ReportTables
{
    [Parameter]
    public Report Report { get; set; }
}