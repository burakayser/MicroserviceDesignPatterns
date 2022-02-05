using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events.Payment
{
    public class PaymentFailedEvent : IPaymentFailedEvent
    {
        public Guid CorrelationId { get; }

        public PaymentFailedEvent(Guid correlationId)
        {
            CorrelationId = correlationId;
        }

        public string Reason { get; set; }

        public List<OrderItemMessage> OrderItems { get; set; } = new List<OrderItemMessage> { };

    }
}
