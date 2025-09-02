using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP3Integration.Application.DTOs.CanselTransaction
{
    public class CancelTransactionRequest
    {
        public long TransactionHandle { get; set; }
        public string Reason { get; set; }
    }
}
