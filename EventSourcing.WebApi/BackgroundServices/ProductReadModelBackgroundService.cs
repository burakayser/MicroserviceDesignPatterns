using EventSourcing.Shared.Events;
using EventSourcing.WebApi.EventStores;
using EventSourcing.WebApi.Models;
using EventStore.ClientAPI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace EventSourcing.WebApi.BackgroundServices
{
    public class ProductReadModelBackgroundService : BackgroundService
    {
        private readonly IEventStoreConnection _eventStoreConnection;
        private readonly ILogger<ProductReadModelBackgroundService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public ProductReadModelBackgroundService(IEventStoreConnection eventStoreConnection, ILogger<ProductReadModelBackgroundService> logger, IServiceProvider serviceProvider)
        {
            _eventStoreConnection = eventStoreConnection;
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }


        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _eventStoreConnection.ConnectToPersistentSubscriptionAsync(ProductStream.StreamName, ProductStream.GroupName,
                EventAppeared, autoAck: false);
        }


        private async Task EventAppeared(EventStorePersistentSubscriptionBase arg1, ResolvedEvent arg2)
        {

            var type = Type.GetType($"{Encoding.UTF8.GetString(arg2.Event.Metadata)}, EventSourcing.Shared");

            _logger.LogInformation($"The message processing.. : Type : {type}");

            var eventData = Encoding.UTF8.GetString(arg2.Event.Data);

            var @event = JsonSerializer.Deserialize(eventData, type);

            using var scope = _serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            Product product = null;

            switch (@event)
            {
                case ProductCreatedEvent productCreatedEvent:
                    product = new Product()
                    {
                        Id = productCreatedEvent.Id,
                        Name = productCreatedEvent.Name,
                        Price = productCreatedEvent.Price,
                        Stock = productCreatedEvent.Stock,
                        UserId = productCreatedEvent.UserId,
                    };
                    context.Products.Add(product);
                    break;

                case ProductDeletedEvent productDeletedEvent:
                    product = context.Products.Find(productDeletedEvent.Id);
                    if (product != null)
                    {
                        context.Products.Remove(product);
                    }
                    break;

                case ProductNameChangeEvent productNameChangeEvent:

                    product = context.Products.Find(productNameChangeEvent.Id);
                    if(product != null)
                    {
                        product.Name = productNameChangeEvent.ChangedName;
                    }

                    break;

                case ProductPriceChangeEvent productPriceChangeEvent:
                    product = context.Products.Find(productPriceChangeEvent.Id);
                    if (product != null)
                    {
                        product.Price = productPriceChangeEvent.ChangedPrice;
                    }
                    break;

            }

            await context.SaveChangesAsync();


            //Yukarıda autoAck:false yapılırsa buradan eventstore a eventın doğru işlendiğine dair bilgi verilir. Eğer bu işlemler sırasında hata oluyorsa eventstore bu eventı göndermeye devam edecektir. True olursa otomatik bu işlem yapılır.
            arg1.Acknowledge(arg2.Event.EventId);
        }
        
    }
}
