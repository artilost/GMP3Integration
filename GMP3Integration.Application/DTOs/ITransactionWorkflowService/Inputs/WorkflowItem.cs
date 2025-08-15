using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GMP3Integration.Application.DTOs.ITransactionWorkflowService.Inputs
{
    public class WorkflowItem
    {
        [JsonPropertyName("type")] public int Type { get; set; }          // genelde 1
        [JsonPropertyName("subType")] public int SubType { get; set; }       // genelde 0
        [JsonPropertyName("deptIndex")] public int DeptIndex { get; set; }     // 0-based
        [JsonPropertyName("amount")] public int Amount { get; set; }        // TL * 100 (kuruş)
        [JsonPropertyName("currencyCode")] public int CurrencyCode { get; set; }  // 949 = TRY
        [JsonPropertyName("count")] public int Count { get; set; }         // adet
        [JsonPropertyName("unitType")] public int UnitType { get; set; }      // sayılabilir = 1

        // Kimlik/opsiyonel ama dokümanda normal alanlar:
        [JsonPropertyName("itemCode")] public string ItemCode { get; set; }
        [JsonPropertyName("name")] public string Name { get; set; }
        [JsonPropertyName("barcode")] public string Barcode { get; set; }
        [JsonPropertyName("flag")] public int? Flag { get; set; }         // verilmezse 0 geçeriz
    }
}
