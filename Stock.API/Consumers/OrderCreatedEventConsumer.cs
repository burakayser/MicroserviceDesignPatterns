using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Events.Order;
using Shared.Events.Stock;
using Shared.Settings;
using Stock.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stock.API.Consumers
{
    public class OrderCreatedEventConsumer : IConsumer<IOrderCreatedEvent>
    {
        private readonly AppDbContext _appDbContext;
        private readonly ILogger<OrderCreatedEventConsumer> _logger;
        private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly IPublishEndpoint _publishEndpoint;

        public OrderCreatedEventConsumer(AppDbContext appDbContext, ILogger<OrderCreatedEventConsumer> logger, ISendEndpointProvider sendEndpointProvider, IPublishEndpoint publishEndpoint)
        {
            _appDbContext = appDbContext;
            _logger = logger;
            _sendEndpointProvider = sendEndpointProvider;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<IOrderCreatedEvent> context)
        {
            var stockResult = new List<bool>();


            foreach (var item in context.Message.OrderItems)
            {
                stockResult.Add(await _appDbContext.Stocks.AnyAsync(x => x.ProductId == item.ProductId && x.Count > item.Count));
            }

            if (stockResult.All(x => x.Equals(true)))
            {

                foreach (var item in context.Message.OrderItems)
                {
                    var stock = await _appDbContext.Stocks.FirstOrDefaultAsync(x => x.ProductId == item.ProductId);

                    if (stock == null)
                    {
                        continue;
                    }

                    stock.Count -= item.Count;

                    await _appDbContext.SaveChangesAsync();

                }

                _logger.LogInformation($"Stock was reserved for CorrelationId : {context.Message.CorrelationId}");

                //var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{RabbitMQSettings.STOCK_RESERVEDEVENT_QUEUENAME}"));

                IStockReservedEvent stockReservedEvent = new StockReservedEvent(context.Message.CorrelationId)
                {
                    OrderItems = context.Message.OrderItems,
                };

                //await sendEndpoint.Send(stockReservedEvent);

                await _publishEndpoint.Publish(stockReservedEvent);

            }

            else
            {
                await _publishEndpoint.Publish(new StockNotReservedEvent(context.Message.CorrelationId)
                {
                    Reason = "Not enough stock"
                });

                _logger.LogInformation($"Stock not enough for CorrelationId: {context.Message.CorrelationId}");

            }
        }
    }
}
