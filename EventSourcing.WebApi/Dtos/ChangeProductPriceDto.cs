using System;

namespace EventSourcing.WebApi.Dtos
{
    public class ChangeProductPriceDto
    {
        public Guid Id { get; set; }
        public int UserId { get; set; }

        public decimal Price { get; set; }
    }
}
