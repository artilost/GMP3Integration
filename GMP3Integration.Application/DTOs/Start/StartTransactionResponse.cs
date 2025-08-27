using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP3Integration.Application.DTOs
{
    public class StartTransactionResponse
    {
        public bool Success { get; set; }
        public ulong TransactionHandle { get; set; }
        public int Rc { get; set; }
        public string Message { get; set; }
        public bool ExistingOpenTicket { get; set; }
    }
}
