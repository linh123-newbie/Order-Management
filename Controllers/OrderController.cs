using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
// using OrderManagement.Data;
using OrderManagement.Models;
using OrderManagement.Models.DTOs;
using AutoMapper;

namespace OrderManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        public OrderController(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<List<Order>>> Get()
        {
            return Ok(await _dbContext.Orders.ToListAsync());
        }
        [HttpGet("revenue-reports")]
        public async Task<IActionResult> GetRevenueReports()
        {
            var orders = await _dbContext.Orders.AsNoTracking()
                .Where(o => o.Status == "Đã giao")
                .Select(o => new { o.CreatedAt, o.Quantity, o.Price })
                .ToListAsync();

            var reports = orders
                .GroupBy(o => o.CreatedAt)
                .OrderByDescending(group => group.Key)
                .Select(group => new
                {
                    CreatedAt = group.Key,
                    DerivedOrders = group.Count(),
                    TotalRevenue = group.Sum(o => o.Quantity * o.Price)
                });

            return Ok(reports);
        }
        [HttpPost]
        public async Task<ActionResult<List<Order>>> Add(OrderDto order)
        {
            if (order == null) return BadRequest("Order data is null.");
            if (string.IsNullOrWhiteSpace(order.CustomerName) || string.IsNullOrWhiteSpace(order.ProductName))
            {
                return BadRequest("Customer name and product name cannot be empty.");
            }
            if (order.Quantity <= 0 || order.Price <= 0)
            {
                return BadRequest("Quantity and price must be greater than zero.");
            }

            var orderEntity = _mapper.Map<Order>(order);  

            var data = new Order
            {
                CustomerName = orderEntity.CustomerName,
                ProductName = orderEntity.ProductName,
                Quantity = orderEntity.Quantity,
                Price = orderEntity.Price,
                Status = "Mới",
                CreatedAt = DateOnly.FromDateTime(DateTime.Now)
            };
            _dbContext.Orders.Add(data);

            await _dbContext.SaveChangesAsync();
            return Ok(await _dbContext.Orders.ToListAsync());
        }
        [HttpPut("{orderId}")]
        public async Task<ActionResult<List<Order>>> Update(int orderId, OrderDto order)
        {
            var data = await _dbContext.Orders.Where(c => c.OrderId == orderId).FirstOrDefaultAsync();

            if (data == null) return NotFound();
            if (data.Status != "Mới") return BadRequest("Only new orders can be edited.");
            if (order == null) return BadRequest("Order data is null.");
            if (string.IsNullOrWhiteSpace(order.CustomerName) || string.IsNullOrWhiteSpace(order.ProductName))
            {
                return BadRequest("Customer name and product name cannot be empty.");
            }
            if (order.Quantity <= 0 || order.Price <= 0)
            {
                return BadRequest("Quantity and price must be greater than zero.");
            }

            var orderEntity = _mapper.Map<Order>(order);

            data.CustomerName = orderEntity.CustomerName;
            data.ProductName = orderEntity.ProductName;
            data.Quantity = orderEntity.Quantity;
            data.Price = orderEntity.Price;   
      
            _dbContext.Orders.Update(data);

            await _dbContext.SaveChangesAsync();
            return Ok(await _dbContext.Orders.ToListAsync());
        }
        [HttpPut("{orderId}/status")]
        public async Task<ActionResult<List<Order>>> UpdateStatus(int orderId, string status)
        {
            if (status != "Đã giao" && status != "Hủy" && status != "Huỷ")
                return BadRequest("Invalid order status.");

            var data = await _dbContext.Orders.Where(c => c.OrderId == orderId).FirstOrDefaultAsync();

            if (data == null) return NotFound();
            if (data.Status != "Mới") return BadRequest("Only new orders can change status.");

            data.Status = status;

            _dbContext.Orders.Update(data);

            await _dbContext.SaveChangesAsync();
            return Ok(await _dbContext.Orders.ToListAsync());
        }


        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] List<int> ids)
        {

            foreach (int id in ids)
            {
                var data = await _dbContext.Orders.Where(c => c.OrderId == id).FirstOrDefaultAsync();
                if (data == null) return NotFound();

                _dbContext.Orders.Remove(data);
            }

            await _dbContext.SaveChangesAsync();
            return NoContent();
        }

    }
}
