using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SagaStateMachineWorker.Models
{
    [Owned]
    public class PaymentOwnedInstance
    {
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string Expiration { get; set; }
        public string CVV { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }

        public override string ToString()
        {
            var props = GetType().GetProperties();

            var sb = new StringBuilder();

            props.ToList().ForEach(p => {

                var value = p.GetValue(this, null);

                sb.AppendLine($"{p.Name} : {p.GetValue(this, null)}");
            });

            return sb.ToString();
        }
    }
    
}
