using GMP3Integration.Infrastructure.Interop;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP3Integration.Infrastructure.Services
{
    internal static class Gmp3EchoRunner
    {
        internal static int TryAll(string iface, ILogger log = null)
        {
            try
            {
                var rc = Gmp3NativeMethods.Iface_AnsiCdecl_x64.Echo(iface);
                log?.LogInformation("Echo_Ansi_Cdecl({iface}) rc=0x{rc:X}", iface, rc);
                if (rc != 0xF034) return rc;
            }
            catch { }

            try
            {
                var rc = Gmp3NativeMethods.Iface_AnsiStd_x64.Echo(iface);
                log?.LogInformation("Echo_Ansi_Std({iface}) rc=0x{rc:X}", iface, rc);
                if (rc != 0xF034) return rc;
            }
            catch { }

            try
            {
                var rc = Gmp3NativeMethods.Iface_UniStd_x64.Echo(iface);
                log?.LogInformation("Echo_Uni_Std({iface}) rc=0x{rc:X}", iface, rc);
                return rc;
            }
            catch { }

            return 0xF034;
        }
    }
}
