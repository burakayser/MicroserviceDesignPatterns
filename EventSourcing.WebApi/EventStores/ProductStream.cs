using EventSourcing.Shared.Events;
using EventSourcing.WebApi.Dtos;
using EventStore.ClientAPI;
using System;

namespace EventSourcing.WebApi.EventStores
{
    public class ProductStream : AbstractStream
    {
        public static string StreamName => "ProductStream";
        public static string GroupName => "replay";

        public ProductStream(IEventStoreConnection eventStoreConnection) : base(StreamName, eventStoreConnection)
        {



        }


        public void Created(CreateProductDto createProductDto)
        {
            Events.AddLast(new ProductCreatedEvent()
            {
                Id = System.Guid.NewGuid(),
                Name = createProductDto.Name,
                Price = createProductDto.Price,
                Stock = createProductDto.Stock,
                UserId = createProductDto.UserId
            });
        }

        public void NameChanged(ChangeProductNameDto changeProductNameDto)
        {
            Events.AddLast(new ProductNameChangeEvent()
            {
                Id = changeProductNameDto.Id,
                ChangedName = changeProductNameDto.Name
            });
        }

        public void PriceChanged(ChangeProductPriceDto changeProductPriceDto)
        {
            Events.AddLast(new ProductPriceChangeEvent()
            {
                Id = changeProductPriceDto.Id,
                ChangedPrice = changeProductPriceDto.Price
            });
        }

        public void Deleted(Guid id)
        {
            Events.AddLast(new ProductDeletedEvent() { Id = id });
        }

    }
}
