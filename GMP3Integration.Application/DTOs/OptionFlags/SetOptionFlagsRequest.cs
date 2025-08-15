using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP3Integration.Application.DTOs.OptionFlags
{
    public class SetOptionFlagsRequest
    {
        public ulong TransactionHandle { get; set; }
        public int ActiveFlags { get; set; }
        public int FlagsToBeSet { get; set; }
    }
}
