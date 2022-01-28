using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Order.API.DTOs;
using Order.API.Models;
using Shared.Events;
using System.Linq;
using System.Threading.Tasks;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _context;

        private readonly IPublishEndpoint _publishEndpoint;

        public OrdersController(AppDbContext context, IPublishEndpoint publishEndpoint)
        {
            _context = context;
            _publishEndpoint = publishEndpoint;
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderCreateDto orderCreate)
        {
            var newOrder = new Models.Order
            {
                BuyerId = orderCreate.BuyerId,
                Status = OrderStatus.Suspend,
                Address = new Address
                {
                    District = orderCreate.Address.District,
                    Line = orderCreate.Address.Line,
                    Province = orderCreate.Address.Province,
                },
                CreatedDate = System.DateTime.Now
            };

            newOrder.Items = orderCreate.OrderItems.Select(x => new OrderItem()
            {
                Price = x.Price,
                Count = x.Count,
                ProductId = x.ProductId,
            }).ToList();

            await _context.AddAsync(newOrder);

            await _context.SaveChangesAsync();

            var orderCreateEvent = new OrderCreatedEvent()
            {
                BuyerId = orderCreate.BuyerId,
                OrderId = newOrder.Id,
                Payment = new Shared.Models.PaymentMessage()
                {
                    CardName = orderCreate.Payment.CardName,
                    CardNumber = orderCreate.Payment.CardNumber,
                    Expiration = orderCreate.Payment.Expiration,
                    CVV = orderCreate.Payment.CVV,
                    TotalPrice = orderCreate.OrderItems.Sum(x => x.Price * x.Count),
                }
            };

            orderCreateEvent.OrderItems = orderCreate.OrderItems.Select(x => new Shared.Models.OrderItemMessage
            {
                Count = x.Count,
                ProductId = x.ProductId,
            }).ToList();

            await _publishEndpoint.Publish(orderCreateEvent);

            return Ok();
        }

    }
}
