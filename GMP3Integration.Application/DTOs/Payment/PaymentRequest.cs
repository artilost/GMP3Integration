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
        // Doküman Sayfa 18 - FP3_Payment için gerekli alanlar (9 alan)
        [JsonPropertyName("typeOfPayment")]
        public uint TypeOfPayment { get; set; }
        
        [JsonPropertyName("subtypeOfPayment")]
        public uint SubtypeOfPayment { get; set; }
        
        [JsonPropertyName("payAmount")]
        public uint PayAmount { get; set; }             // TL*100
        
        [JsonPropertyName("payAmountCurrencyCode")]
        public ushort PayAmountCurrencyCode { get; set; } // 949
        
        [JsonPropertyName("bankBkmId")]
        public ushort BankBkmId { get; set; }
        
        [JsonPropertyName("bankPaymentUniqueId")]
        public string BankPaymentUniqueId { get; set; }   // opsiyonel
        
        [JsonPropertyName("payAmountBonus")]
        public uint PayAmountBonus { get; set; }
        
        [JsonPropertyName("numberOfinstallments")]
        public ushort NumberOfinstallments { get; set; }
        
        [JsonPropertyName("transactionFlag")]
        public uint TransactionFlag { get; set; }

        public PaymentRequest()
        {
            TypeOfPayment = 0;
            SubtypeOfPayment = 0;
            BankPaymentUniqueId = "";
            PayAmountCurrencyCode = 949; // Default Turkish Lira
            BankBkmId = 0; // Auto select
            TransactionFlag = 0;
            PayAmountBonus = 0;
            NumberOfinstallments = 0;
        }
    }
}
