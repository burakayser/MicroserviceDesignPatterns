using Automatonymous;
using Shared.Events.OrderCreated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SagaStateMachineWorker.Models
{
    public class OrderStateMachine : MassTransitStateMachine<OrderStateInstance>
    {
        public Event<IOrderCreatedRequestEvent> OrderCreatedRequestEvent { get; set; }

        public State OrderCreated { get; private set; }


        public OrderStateMachine()
        {
            InstanceState(x => x.CurrentState);

            Event(() => OrderCreatedRequestEvent, y => y.CorrelateBy<int>(x => x.OrderId, z => z.Message.OrderId).SelectId(context => Guid.NewGuid()));

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
                .Publish(context => new OrderCreatedEvent() { OrderItems = context.Data.OrderItems })
                .TransitionTo(OrderCreated)
                .Then(context => { Console.WriteLine($"OrderCreatedRequestEvent After: { context.Instance }"); })
                );




        }


    }
}
