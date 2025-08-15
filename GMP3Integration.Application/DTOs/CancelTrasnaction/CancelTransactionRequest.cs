using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP3Integration.Application.DTOs.CanselTransaction
{
    public class CancelTransactionRequest
    {
        public ulong TransactionHandle { get; set; }
    }
}
