using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Models;
using OrderManagement.Models.DTOs;
namespace OrderManagement.Controllers;

public class ItemController : Controller
{
    private readonly AppDbContext _context;
    private readonly HttpClient _client;
    Uri baseAddress = new Uri("http://localhost:5051/api/Order");
    public ItemController(AppDbContext context, HttpClient client)
    {
        _context = context;
        _client = client;
    }
    [HttpGet]
    public async Task<IActionResult> Overview()
    {
        using var response = await _client.GetAsync(baseAddress);

        if (!response.IsSuccessStatusCode)
        {
            return StatusCode((int)response.StatusCode, "Cannot load orders.");
        }
        var orders = await response.Content.ReadFromJsonAsync<List<Order>>() ?? new List<Order>();
        return View("~/Views/Items/Overview.cshtml", orders);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(OrderDto order)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Invalid order data.");
        }
        if (string.IsNullOrWhiteSpace(order.CustomerName) || string.IsNullOrWhiteSpace(order.ProductName))
        {
            return BadRequest("Customer name and product name cannot be empty.");
        }
        if (order.Quantity <= 0 || order.Price <= 0)
        {
            return BadRequest("Quantity and price must be greater than zero.");
        }

        var response = await _client.PostAsJsonAsync(baseAddress, order);

        if (!response.IsSuccessStatusCode)
        {
            return StatusCode((int)response.StatusCode, "Cannot add order.");
        }

        TempData["OrderSuccess"] = "Add successful.";
        return Ok();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int id, OrderDto order)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Invalid order data.");
        }

        if (order == null)
        {
            return BadRequest("Invalid order data.");
        }
        if (string.IsNullOrWhiteSpace(order.CustomerName) || string.IsNullOrWhiteSpace(order.ProductName))
        {
            return BadRequest("Customer name and product name cannot be empty.");
        }
        if (order.Quantity <= 0 || order.Price <= 0)
        {
            return BadRequest("Quantity and price must be greater than zero.");
        }

        var response = await _client.PutAsJsonAsync($"{baseAddress}/{id}", order);

        if (!response.IsSuccessStatusCode)
        {
            return StatusCode((int)response.StatusCode, "Cannot update order.");
        }

        TempData["OrderSuccess"] = "Update successful.";
        return Ok();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatus(int id, string status)
    {
        if (!ModelState.IsValid || id <= 0 || string.IsNullOrWhiteSpace(status))
        {
            return BadRequest("Invalid order status.");
        }

        using var response = await _client.PutAsync($"{baseAddress}/{id}/status?status={Uri.EscapeDataString(status)}", null);

        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            return StatusCode((int)response.StatusCode,
                response.StatusCode == System.Net.HttpStatusCode.BadRequest && !string.IsNullOrWhiteSpace(errorMessage)
                    ? errorMessage : "Cannot update order status.");
        }

        return Ok();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(List<int> ids)
    {
        if (ids == null || ids.Count == 0)
        {
            return RedirectToAction("Home", "Home");
        }

        var query = string.Join("&", ids.Select(id => $"ids={id}"));

        using var response = await _client.DeleteAsync($"{baseAddress}?{query}");

        if (!response.IsSuccessStatusCode)
        {
            return StatusCode((int)response.StatusCode, "Cannot delete orders.");
        }

        TempData["OrderSuccess"] = "Delete successful.";
        return RedirectToAction("Home", "Home");
    }

}
