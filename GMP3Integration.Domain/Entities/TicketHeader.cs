using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP3Integration.Domain.Entities
{
    public class TicketHeader
    {
        public ulong TransactionHandle { get; set; }
        public string HeaderText { get; set; }
    }
}
