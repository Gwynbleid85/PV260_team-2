using ArkFunds.App.Web.Api;
using Microsoft.AspNetCore.Components;

namespace ArkFunds.App.Web.Pages;

public partial class ReportTables
{
    [Parameter]
    public Report Report { get; set; }
}