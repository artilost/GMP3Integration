using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GMP3Integration.Application.DTOs.ITransactionWorkflowService.Inputs
{
    public class WorkflowPayment
    {
        [JsonPropertyName("typeOfPayment")] public string TypeOfPayment { get; set; }
        [JsonPropertyName("subtypeOfPayment")] public string SubtypeOfPayment { get; set; }
        [JsonPropertyName("payAmount")] public int PayAmount { get; set; }            // TL * 100
        [JsonPropertyName("payAmountCurrencyCode")] public int PayAmountCurrencyCode { get; set; } // 949 = TRY
        [JsonPropertyName("bankPaymentUniqueId")] public string BankPaymentUniqueId { get; set; } // opsiyonel
    }
}
