using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Settings
{
    public class RabbitMQSettings
    {
        public const string ORDER_SAGA = "order-saga-queue";
        public const string PAYMENT_STOCK_RESERVED_REQUESTEVENT_QUEUENAME = "payment-stock-reserved-request-queue";
        public const string ORDER_STOCK_NOTRESERVED_REQUESTEVENT_QUEUENAME = "order-stock-notreserved-request-queue";
        public const string ORDER_FINAL_REQUEST_QUEUENAME = "order-final-request-queue";
        public const string ORDER_FAILED_EVENT_QUEUENAME = "order-failed-event-queue";
        public const string STOCK_ROLLBACK_EVENT_QUEUENAME = "stock-rollback-event-queue";
        public const string STOCK_ORDERCREATEDEVENT_QUEUENAME = "stock-order-created-queue";

    }
}
