using MassTransit;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events.Payment
{
    public interface IPaymentFailedEvent : CorrelatedBy<Guid>
    {
        string Reason { get; set; }

        List<OrderItemMessage> OrderItems { get; set; }

    }
}
