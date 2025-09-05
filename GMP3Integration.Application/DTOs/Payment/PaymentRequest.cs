using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace GMP3Integration.Application.DTOs.Payment
{
    public class PaymentRequest
    {
        [JsonPropertyName("transactionHandle")]
        public ulong TransactionHandle { get; set; }

        // Doküman adlarıyla 1:1 - JSON camelCase mapping
        [JsonPropertyName("typeOfPayment")]
        public string TypeOfPayment { get; set; }
        
        [JsonPropertyName("subtypeOfPayment")]
        public string SubtypeOfPayment { get; set; }
        
        [JsonPropertyName("payAmount")]
        public int PayAmount { get; set; }             // TL*100
        
        [JsonPropertyName("payAmountCurrencyCode")]
        public int PayAmountCurrencyCode { get; set; } // 949
        
        [JsonPropertyName("bankPaymentUniqueId")]
        public string BankPaymentUniqueId { get; set; }   // opsiyonel

        public PaymentRequest()
        {
            TypeOfPayment = "";
            SubtypeOfPayment = "";
            BankPaymentUniqueId = "";
            PayAmountCurrencyCode = 949; // Default Turkish Lira
        }
    }
}
