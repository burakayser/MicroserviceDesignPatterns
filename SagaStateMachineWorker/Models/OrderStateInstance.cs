using Automatonymous;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SagaStateMachineWorker.Models
{
    public class OrderStateInstance : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; }
        public string BuyerId { get; set; }
        public int OrderId { get; set; }
        
        public PaymentOwnedInstance Payment { get; set; }

        public DateTime CreateDate { get; set; }

        public override string ToString()
        {
            var props = GetType().GetProperties();

            var sb = new StringBuilder();

            props.ToList().ForEach(p => {
                sb.Append($"{p.Name} : {p.GetValue(this,null)}");
            });

            return sb.ToString();
        }

    }
}
