using EventStore.ClientAPI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EventSourcing.WebApi.EventStores
{
    public static class EventStoreExtensions
    {
        public static IServiceCollection AddEventStore(this IServiceCollection services, IConfiguration configuration) {

            var connection = EventStoreConnection.Create(connectionString: configuration.GetConnectionString("EventStore"));

            connection.ConnectAsync().Wait();

            services.AddSingleton(connection);

            using var logFactory = LoggerFactory.Create(b =>
            {
                b.SetMinimumLevel(LogLevel.Information);
                b.AddConsole();
            });

            var logger = logFactory.CreateLogger("Startup");

            connection.Connected += (sender, args) => {
                logger.LogInformation("EventStore connection established");
            };

            connection.ErrorOccurred += (sender, args) => {
                logger.LogError(args.Exception.Message);
            };


            return services;
        
        }
    }
}
