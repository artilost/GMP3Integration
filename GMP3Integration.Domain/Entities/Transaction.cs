using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP3Integration.Domain.Entities
{
    public class Transaction
    {
        public string InterfaceName { get; set; }
        public ulong TransactionHandle { get; set; }
        public DateTime StartedAt { get; set; }
        public bool IsClosed { get; set; }
    }
}
