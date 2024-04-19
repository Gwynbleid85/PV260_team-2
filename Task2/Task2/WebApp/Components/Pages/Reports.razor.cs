using Microsoft.AspNetCore.Components;
using Reports.Core;
using Reports.Infrastructure;
using Wolverine;

namespace WebApp.Components.Pages;

public partial class Reports
{

    private HttpClient _client = new HttpClient() { BaseAddress = new Uri("https://localhost:7108") };
    private Report report;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            var response = await _client.GetAsync("reports/current/");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseBody);
            //report = responseBody; TODO needs parsing but API is not working
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine("\nException Caught!");
            Console.WriteLine("Message :{0} ", e.Message);
            report = new Report
            {
                Id = Guid.NewGuid(),
                Year = 2024,
                Month = 4,
                NewPositions = GenerateHoldingsList(4),
                IncreaedPositions = GenerateHoldingsList(4),
                ReducedPositions = GenerateHoldingsList(4)
            };
        }
    }


    public static List<Holdings> GenerateHoldingsList(int count)
    {
        var holdingsList = new List<Holdings>();
        Random random = new Random();

        for (int i = 0; i < count; i++)
        {
            holdingsList.Add(new Holdings()
            {
                Date = DateOnly.FromDateTime(new DateTime(2024, 4, random.Next(1, 30))), // Generating random Date within April 2024
                Fund = "Dummy Fund",
                Company = "Dummy Company",
                Ticker = "DUM",
                Cusip = "123456789",
                Shares = random.Next(1000, 10000),
                MarketValue = random.NextDouble() * 1000000,
                Weight = random.NextDouble() * 100
            });
        }

        return holdingsList;
    }
}
