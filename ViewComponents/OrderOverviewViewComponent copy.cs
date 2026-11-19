using Microsoft.AspNetCore.Mvc;
using OrderManagement.Models;

namespace OrderManagement.ViewComponents;

public class OrderOverviewViewComponent : ViewComponent
{
    private readonly HttpClient _client;

    public OrderOverviewViewComponent(HttpClient client)
    {
        _client = client;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        using var response = await _client.GetAsync("http://localhost:5051/api/Order");
        response.EnsureSuccessStatusCode();
        var orders = await response.Content.ReadFromJsonAsync<List<Order>>() ?? new List<Order>();
        return View("~/Views/Items/Overview.cshtml", orders);
    }
}
