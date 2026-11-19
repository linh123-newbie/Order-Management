using System;

namespace OrderManagement.Models.DTOs;

public class RevenueReportDto
{
    public DateOnly? CreatedAt { get; set; }
    public int DerivedOrders { get; set; }
    public decimal TotalRevenue { get; set; }
}
