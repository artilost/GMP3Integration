using GMP3Integration.Application.DTOs.ITransactionWorkflowService.Inputs;
using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace GMP3Integration.Application.DTOs.ITransactionWorkflowService
{
    public class CompleteSaleRequest
    {
        [JsonPropertyName("currentInterface")] public string CurrentInterface { get; set; }

        // OptionFlags (dokümanla uyumlu)
        public int ActiveFlags { get; set; }
        public int FlagsToBeSet { get; set; }

        // TicketHeader
        public int TicketType { get; set; }

        // ✳️ Artık handle’sız workflow tipleri:
        public List<WorkflowItem> Items { get; set; } = new List<WorkflowItem>();
        public WorkflowPayment Payment { get; set; }
        public List<WorkflowMessage> Messages { get; set; } = new List<WorkflowMessage>();
    }
}
