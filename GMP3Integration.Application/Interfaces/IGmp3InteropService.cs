using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP3Integration.Application.Interfaces
{
    /// <summary>
    /// Native katmanı saran en alt seviye servis arayüzü.
    /// Uygulaması Infrastructure'da: Gmp3InteropService
    /// </summary>
    public interface IGmp3InteropService
    {
        int Echo(string interfaceId);                 // FP3_Echo
        int Ping(string interfaceId);                 // FP3_Ping
        int StartPairingInit_All(string interfaceId); // StartPairingInit_All
    }
}
