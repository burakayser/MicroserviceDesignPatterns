using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events.Stock
{
    public class StockReservedRequestEvent : IStockReservedRequestEvent
    {
        public StockReservedRequestEvent(Guid correlationId)
        {
            CorrelationId = correlationId;
        }

        public Guid CorrelationId { get; }

        public List<OrderItemMessage> OrderItems { get; set; }

        public PaymentMessage Payment { get; set; }
        public string BuyerId { get; set; }
    }
}
