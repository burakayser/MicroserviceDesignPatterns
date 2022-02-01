using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Events;
using System.Threading.Tasks;

namespace Payment.API.Consumers
{
    public class StockReservedEventConsumer : IConsumer<StockReservedEvent>
    {
        private readonly ILogger<StockReservedEventConsumer> _logger;

        private readonly IPublishEndpoint _endpoint;

        public StockReservedEventConsumer(ILogger<StockReservedEventConsumer> logger, IPublishEndpoint endpoint)
        {
            _logger = logger;
            _endpoint = endpoint;
        }

        public async Task Consume(ConsumeContext<StockReservedEvent> context)
        {
            var balance = 3000m;

            if (balance <= context.Message.Payment.TotalPrice)
            {
                _logger.LogInformation($"{context.Message.Payment.TotalPrice} TL was NOT withdrawn from credit card for User id = {context.Message.BuyerId}");

                await _endpoint.Publish(new PaymentFailedEvent() { 
                    BuyerId = context.Message.BuyerId, OrderId = context.Message.OrderId, Message = "Not enough balance",
                    OrderItems = context.Message.OrderItems
                });

                return;
            }

            _logger.LogInformation($"{context.Message.Payment.TotalPrice} TL was withdrawn from credit card for User id = {context.Message.BuyerId}");

            await _endpoint.Publish(new PaymentCompletedEvent { BuyerId = context.Message.BuyerId, OrderId = context.Message.OrderId });

        }
    }
}
