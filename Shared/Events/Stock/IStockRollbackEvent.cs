using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events.Stock
{
    public interface IStockRollbackEvent
    {
        public int OrderId { get; set; }
        public List<OrderItemMessage> OrderItems { get; set; }
    }
}
