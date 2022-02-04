using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events.OrderCreated
{
    public class OrderCreatedEvent : IOrderCreatedEvent
    {
        public List<OrderItemMessage> OrderItems { get; set; } = new List<OrderItemMessage> { };
    }
}
