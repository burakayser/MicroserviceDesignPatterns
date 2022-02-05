using MassTransit;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events.Stock
{
    public interface IStockReservedRequestEvent : CorrelatedBy<Guid>
    {
        string BuyerId { get; set; }

        PaymentMessage Payment { get; set; }

        List<OrderItemMessage> OrderItems { get; set; }
    }
}
