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

        // Emulator'da kullanılan ek alanlar
        [JsonPropertyName("flags")]
        public uint Flags { get; set; }
        
        [JsonPropertyName("dateOfPayment")]
        public uint DateOfPayment { get; set; }
        
        [JsonPropertyName("orgAmount")]
        public uint OrgAmount { get; set; }
        
        [JsonPropertyName("orgAmountCurrencyCode")]
        public ushort OrgAmountCurrencyCode { get; set; }
        
        [JsonPropertyName("cashBackAmountInTL")]
        public uint CashBackAmountInTL { get; set; }
        
        [JsonPropertyName("bankBkmId")]
        public ushort BankBkmId { get; set; }
        
        [JsonPropertyName("terminalId")]
        public string TerminalId { get; set; }
        
        [JsonPropertyName("merchantId")]
        public string MerchantId { get; set; }
        
        [JsonPropertyName("batchNo")]
        public uint BatchNo { get; set; }
        
        [JsonPropertyName("stan")]
        public uint Stan { get; set; }
        
        [JsonPropertyName("authorizeCode")]
        public string AuthorizeCode { get; set; }
        
        [JsonPropertyName("transFlag")]
        public uint TransFlag { get; set; }
        
        // Emulator'da kullanılan bankName alanı
        [JsonPropertyName("bankName")]
        public string BankName { get; set; }

        public PaymentRequest()
        {
            TypeOfPayment = "";
            SubtypeOfPayment = "";
            BankPaymentUniqueId = "";
            PayAmountCurrencyCode = 949; // Default Turkish Lira
            OrgAmountCurrencyCode = 949; // Default Turkish Lira
            TerminalId = "";
            MerchantId = "";
            AuthorizeCode = "";
            BankName = "";
        }
    }
}
