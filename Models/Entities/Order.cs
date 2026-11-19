using System;

namespace OrderManagement.Models;

public  class Order
{
    public int OrderId { get; set; }
    public string? CustomerName { get; set; }
    public string? ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Total => Quantity * Price;
    public string? Status { get; set; }
    public DateOnly? CreatedAt { get; set; }
}
