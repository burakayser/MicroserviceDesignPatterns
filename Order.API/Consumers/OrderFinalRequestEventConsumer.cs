using MassTransit;
using Microsoft.Extensions.Logging;
using Order.API.Models;
using Shared.Events.Order;
using System.Threading.Tasks;

namespace Order.API.Consumers
{
    public class OrderFinalRequestEventConsumer : IConsumer<OrderFinalRequestEvent>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<OrderFinalRequestEventConsumer> _logger;

        public OrderFinalRequestEventConsumer(AppDbContext context, ILogger<OrderFinalRequestEventConsumer> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<OrderFinalRequestEvent> context)
        {
            var order = await _context.Orders.FindAsync(context.Message.OrderId);

            if (order != null)
            {
                order.Status = OrderStatus.Completed;

                await _context.SaveChangesAsync();

                _logger.LogInformation($"Order (Id={context.Message.OrderId}) status changed : {order.Status}");

            }
            else
            {
                _logger.LogInformation($"Order (Id={context.Message.OrderId}) not found");
            }


        }
    }
}
