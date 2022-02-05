using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events.Stock
{
    public interface IStockNotReservedEvent : CorrelatedBy<Guid>
    {
        public string Reason { get; set; }
    }
}
