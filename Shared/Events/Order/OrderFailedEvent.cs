using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events.Order
{
    public class OrderFailedEvent : IOrderFailedEvent
    {
        public int OrderId { get; set; }

        public string Reason { get; set; }
    }
}
