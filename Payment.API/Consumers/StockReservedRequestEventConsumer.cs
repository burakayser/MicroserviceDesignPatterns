using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Events;
using Shared.Events.Payment;
using Shared.Events.Stock;
using System.Threading.Tasks;

namespace Payment.API.Consumers
{
    public class StockReservedRequestEventConsumer : IConsumer<IStockReservedRequestEvent>
    {
        private readonly ILogger<StockReservedRequestEventConsumer> _logger;

        private readonly IPublishEndpoint _endpoint;

        public StockReservedRequestEventConsumer(ILogger<StockReservedRequestEventConsumer> logger, IPublishEndpoint endpoint)
        {
            _logger = logger;
            _endpoint = endpoint;
        }

        public async Task Consume(ConsumeContext<IStockReservedRequestEvent> context)
        {
            var balance = 3000m;

            if (balance <= context.Message.Payment.TotalPrice)
            {
                _logger.LogInformation($"{context.Message.Payment.TotalPrice} TL was NOT withdrawn from credit card for User id = {context.Message.BuyerId}");

                await _endpoint.Publish(new PaymentFailedEvent(context.Message.CorrelationId)
                {
                    Reason = "Not enough balance",
                    OrderItems = context.Message.OrderItems
                });

                return;
            }

            _logger.LogInformation($"{context.Message.Payment.TotalPrice} TL was withdrawn from credit card for User id = {context.Message.BuyerId}");

            await _endpoint.Publish(new PaymentCompletedEvent(context.Message.CorrelationId) { });

        }

       
    }
}
