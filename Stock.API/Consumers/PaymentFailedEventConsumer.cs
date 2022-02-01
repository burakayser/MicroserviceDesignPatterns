using MassTransit;
using Microsoft.Extensions.Logging;
using Stock.API.Models;
using Shared.Events;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Stock.API.Consumers
{
    public class PaymentFailedEventConsumer : IConsumer<PaymentFailedEvent>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<PaymentFailedEventConsumer> _logger;

        public PaymentFailedEventConsumer(AppDbContext context, ILogger<PaymentFailedEventConsumer> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
        {

            foreach (var item in context.Message.OrderItems)
            {
                var stock = await _context.Stocks.FirstOrDefaultAsync(x => x.ProductId == item.ProductId);
                    
                if (stock != null)
                {
                    stock.Count += item.Count;
                    await _context.SaveChangesAsync();

                    _logger.LogInformation($"Stock was released for Order Id: {context.Message.OrderId}");
                }
            } 


        }
    }
}
