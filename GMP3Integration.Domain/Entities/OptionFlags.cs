using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP3Integration.Domain.Entities
{
    public class OptionFlags
    {
        public ulong TransactionHandle { get; set; }
        public int Flags { get; set; }
    }
}
