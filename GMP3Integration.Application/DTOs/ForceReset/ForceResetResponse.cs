using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP3Integration.Application.DTOs.ForceReset
{
    public class ForceResetResponse
    {
        public bool Reset { get; set; }
        public int ResultCode { get; set; }
        public string Message { get; set; }
        public int? TransactionHandle { get; set; }
    }
}
