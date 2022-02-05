using Automatonymous;
using Shared.Events.Order;
using Shared.Events.Payment;
using Shared.Events.Stock;
using Shared.Settings;
using System;

namespace SagaStateMachineWorker.Models
{
    public class OrderStateMachine : MassTransitStateMachine<OrderStateInstance>
    {
        public Event<IOrderCreatedRequestEvent> OrderCreatedRequestEvent { get; set; }
        public Event<IStockReservedEvent> StockReservedEvent { get; set; }
        public Event<IStockNotReservedEvent> StockNotReservedEvent { get; set; }
        public Event<IPaymentCompletedEvent> PaymentCompletedEvent { get; set; }
        public Event<IPaymentFailedEvent> PaymentFailedEvent { get; set; }

        public State OrderCreated { get; private set; }
        public State StockReserved { get; private set; }
        public State StockNotReserved { get; private set; }
        public State PaymentCompleted { get; private set; }
        public State PaymentFailed { get; private set; }


        public OrderStateMachine()
        {
            InstanceState(x => x.CurrentState);

            Event(() => OrderCreatedRequestEvent, y => y.CorrelateBy<int>(x => x.OrderId, z => z.Message.OrderId).SelectId(context => Guid.NewGuid()));

            Event(() => StockReservedEvent, y => y.CorrelateById(z => z.Message.CorrelationId));
            
            Event(() => StockNotReservedEvent, y => y.CorrelateById(z => z.Message.CorrelationId));

            Event(() => PaymentCompletedEvent, y => y.CorrelateById(z => z.Message.CorrelationId));

            Initially(
                When(OrderCreatedRequestEvent)
                .Then(context => {
                context.Instance.BuyerId = context.Data.BuyerId;
                context.Instance.OrderId = context.Data.OrderId;
                context.Instance.CreateDate = DateTime.Now;
                context.Instance.Payment.CardName = context.Data.Payment.CardName;
                context.Instance.Payment.CardNumber = context.Data.Payment.CardNumber;
                context.Instance.Payment.CVV = context.Data.Payment.CVV;
                context.Instance.Payment.Expiration = context.Data.Payment.Expiration;
                context.Instance.Payment.TotalPrice = context.Data.Payment.TotalPrice;
                })
                .Then(context => { Console.WriteLine($"OrderCreatedRequestEvent Before: { context.Instance }"); })
                .Publish(context => new OrderCreatedEvent(context.Instance.CorrelationId) { OrderItems = context.Data.OrderItems, BuyerId = context.Data.BuyerId })
                .TransitionTo(OrderCreated)
                .Then(context => { Console.WriteLine($"OrderCreatedRequestEvent After: { context.Instance }"); })
                );

            During(OrderCreated,
                When(StockReservedEvent)
                .TransitionTo(StockReserved)
                .Send(new Uri($"queue:{RabbitMQSettings.PAYMENT_STOCK_RESERVED_REQUESTEVENT_QUEUENAME}"), context => new StockReservedRequestEvent(context.Instance.CorrelationId) {  
                    OrderItems = context.Data.OrderItems,
                    BuyerId = context.Instance.BuyerId,
                    Payment = new Shared.Models.PaymentMessage() { 
                        CardName = context.Instance.Payment.CardName,
                        CardNumber = context.Instance.Payment.CardNumber,
                        CVV = context.Instance.Payment.CVV,
                        Expiration = context.Instance.Payment.Expiration,
                        TotalPrice = context.Instance.Payment.TotalPrice,
                    }
                })
                .Then(context => { Console.WriteLine($"StockReservedRequestEvent After: { context.Instance }"); }),
                When(StockNotReservedEvent)
                .TransitionTo(StockNotReserved)
                .Publish(context => new OrderFailedEvent() { OrderId = context.Instance.OrderId, Reason = context.Data.Reason })
                .Then(context => { Console.WriteLine($"StockNOTReservedRequestEvent After: { context.Instance }"); })
                );


            During(StockReserved,
                When(PaymentCompletedEvent)
                .TransitionTo(PaymentCompleted)
                .Publish(context => new OrderFinalRequestEvent(context.Instance.CorrelationId) { OrderId = context.Instance.OrderId })
                .Then(context => { Console.WriteLine($"OrderFinalRequestEvent After: { context.Instance }"); })
                .Finalize(),
                When(PaymentFailedEvent)
                .TransitionTo(PaymentFailed)
                .Send(new Uri($"queue:{RabbitMQSettings.STOCK_ROLLBACK_EVENT_QUEUENAME}"), context =>  new StockRollbackEvent() { OrderId = context.Instance.OrderId, OrderItems = context.Data.OrderItems } )
                .Publish(context => new OrderFailedEvent() { OrderId = context.Instance.OrderId, Reason = context.Data.Reason })
                .Then(context => { Console.WriteLine($"PaymentFailedEvent After: { context.Instance }"); })
                ) ;


            SetCompletedWhenFinalized();

        }


    }
}
