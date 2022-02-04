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



        public const string STOCK_ORDERCREATEDEVENT_QUEUENAME = "stock-order-created-queue";
        public const string STOCK_RESERVEDEVENT_QUEUENAME = "stock-reserved-queue";
        public const string STOCK_NOTRESERVEDEVENT_QUEUENAME = "stock-notreserved-queue";
        public const string STOCK_PAYMENT_FAILED_QUEUENAME = "stock-payment-failed-queue";
        public const string ORDER_PAYMENT_COMPLETED_QUEUENAME = "order-payment-completed-queue";
        public const string ORDER_PAYMENT_FAILED_QUEUENAME = "order-payment-failed-queue";

    }
}
