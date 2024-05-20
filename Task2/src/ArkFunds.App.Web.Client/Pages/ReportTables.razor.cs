using ArkFunds.App.Web.Client.Api;
using Microsoft.AspNetCore.Components;

namespace ArkFunds.App.Web.Client.Pages;

public partial class ReportTables
{
    [Parameter]
    public Report Report { get; set; }
}