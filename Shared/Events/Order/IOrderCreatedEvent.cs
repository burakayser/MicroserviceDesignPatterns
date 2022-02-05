using MassTransit;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events.Order
{
    public interface IOrderCreatedEvent : CorrelatedBy<Guid>
    {
        string BuyerId { get; set; }
        List<OrderItemMessage> OrderItems { get; set; }
    }
}
