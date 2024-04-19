using Microsoft.AspNetCore.Components;
using WebApp.Api;

namespace WebApp.Components;

public partial class ReportTables
{
    [Parameter]
    public Report Report { get; set; }
}