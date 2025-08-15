using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP3Integration.Application.DTOs.Refund
{
    public class RefundRequest
    {
        public ulong TransactionHandle { get; set; }
        public decimal Amount { get; set; }
    }
}
