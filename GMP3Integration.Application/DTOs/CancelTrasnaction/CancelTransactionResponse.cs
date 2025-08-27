using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP3Integration.Application.DTOs.CancelTansaction
{
    public class CancelTransactionResponse
    {
        public bool Success { get; set; }
        public int ResultCode { get; set; }
        public string Message { get; set; }
    }
}
