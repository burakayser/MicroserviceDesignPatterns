using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Settings
{
    public class RabbitMQSettings
    {
        public const string STOCK_ORDERCREATEDEVENT_QUEUENAME = "stock-order-created-queue";
        public const string STOCK_RESERVEDEVENT_QUEUENAME = "stock-reserved-queue";
    }
}
