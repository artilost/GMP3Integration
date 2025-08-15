using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP3Integration.Application.DTOs.TicketHeader
{
    public class SendTicketHeaderRequest
    {
        public ulong TransactionHandle { get; set; }
        public int TicketType { get; set; }
    }
}
