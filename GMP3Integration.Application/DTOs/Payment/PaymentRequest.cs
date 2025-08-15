using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP3Integration.Application.DTOs.Payment
{
    public class PaymentRequest
    {
        public ulong TransactionHandle { get; set; }

        // Doküman adlarıyla 1:1
        public string TypeOfPayment { get; set; }
        public string SubtypeOfPayment { get; set; }
        public int PayAmount { get; set; }             // TL*100
        public int PayAmountCurrencyCode { get; set; } // 949
        public string BankPaymentUniqueId { get; set; }   // opsiyonel

    }
}
