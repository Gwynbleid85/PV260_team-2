using Reports.Core;

namespace WebApp.Components.Pages;

public partial class History
{
    private HttpClient _client = new HttpClient() { BaseAddress = new Uri("https://localhost:7108") };
    public List<string> history = ["January 2024", "December 2023", "November 2023"];
    protected override async Task OnInitializedAsync()
    {
        try
        {
            var response = await _client.GetAsync("reports/history/");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseBody);
           // history = responseBody;// TODO needs parsing but no data to try
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine("\nException Caught!");
            Console.WriteLine("Message :{0} ", e.Message);
        }
    }
}
