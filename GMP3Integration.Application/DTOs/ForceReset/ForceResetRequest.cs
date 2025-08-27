using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP3Integration.Application.DTOs.ForceReset
{
    public class ForceResetRequest
    {
        public string CurrentInterface { get; set; }  // "LAN1" veya "LAN:IP,PORT" / "TCP:IP:PORT"
        public int TimeoutSeconds { get; set; } = 8;   // kısa tutalım
    }
}
