using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP3Integration.Domain.Entities
{
    public class Payment
    {
        public ulong TransactionHandle { get; set; }
        public string PaymentType { get; set; }     // Örn. “CreditCard”, “Cash”
        public string SubType { get; set; }         // Örn. “Regular”, “Installment”
        public decimal Amount { get; set; }         // Ödenecek tutar (kuruşlu)
    }
}
