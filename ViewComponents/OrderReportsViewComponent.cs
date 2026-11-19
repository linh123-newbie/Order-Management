using Microsoft.AspNetCore.Mvc;
using OrderManagement.Models.DTOs;

namespace OrderManagement.ViewComponents;

public class OrderReportsViewComponent : ViewComponent
{
    private readonly HttpClient _client;
    Uri baseAddress = new Uri("http://localhost:5051/api/Order");

    public OrderReportsViewComponent(HttpClient client)
    {
        _client = client;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        try
        {
            using var response = await _client.GetAsync($"{baseAddress}/revenue-reports");
            if (!response.IsSuccessStatusCode)
            {
                return View("~/Views/Shared/Components/OrderReports/Error.cshtml");
            }

            var revenueReports = await response.Content.ReadFromJsonAsync<List<RevenueReportDto>>() ?? new List<RevenueReportDto>();
            return View("~/Views/Items/RevenueReports.cshtml", revenueReports);
        }
        catch (HttpRequestException)
        {
            return View("~/Views/Shared/Components/OrderReports/Error.cshtml");
        }
    }
}
