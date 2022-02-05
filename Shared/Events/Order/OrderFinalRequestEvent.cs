using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events.Order
{
    public class OrderFinalRequestEvent : IOrderFinalRequestEvent
    {
        public int OrderId { get; set; }

        public Guid CorrelationId { get; set; }

        public OrderFinalRequestEvent(Guid correlationId)
        {
            CorrelationId = correlationId;
        }
    }
}
