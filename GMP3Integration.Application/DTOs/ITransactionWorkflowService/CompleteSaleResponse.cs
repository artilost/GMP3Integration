using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP3Integration.Application.DTOs.ITransactionWorkflowService
{
    public class CompleteSaleResponse
    {
        public ulong TransactionHandle { get; set; }
        public bool Success { get; set; }
    }
}
