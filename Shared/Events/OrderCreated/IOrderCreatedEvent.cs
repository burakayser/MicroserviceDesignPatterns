using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events.OrderCreated
{
    public interface IOrderCreatedEvent
    {
        List<OrderItemMessage> OrderItems { get; set; }
    }
}
