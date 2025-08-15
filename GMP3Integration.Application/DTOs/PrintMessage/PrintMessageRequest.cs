using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP3Integration.Application.DTOs.PrintMessage
{
    /// <summary>
    /// Mesaj yazdırma isteği için gerekli bilgiler.
    /// </summary>
    public class PrintMessageRequest
    {
        public ulong TransactionHandle { get; set; }
        public string MessageText { get; set; }
    }
}
