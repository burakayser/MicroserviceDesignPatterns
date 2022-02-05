using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Events.Stock;
using Stock.API.Models;
using System.Threading.Tasks;

namespace Stock.API.Consumers
{
    public class StockRollbackEventConsumer : IConsumer<StockRollbackEvent>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<StockRollbackEventConsumer> _logger;

        public StockRollbackEventConsumer(AppDbContext context, ILogger<StockRollbackEventConsumer> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<StockRollbackEvent> context)
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
