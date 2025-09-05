using GMP3Integration.Infrastructure.Interop;
using GMP3Integration.Infrastructure.Interop.Native.Structs;
using GMP3Integration.Infrastructure.Session;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using GMP3Integration.Infrastructure.Services.Pairing;

namespace GMP3Integration.Infrastructure.Services.Connection
{
    /// <summary>
    /// GMP3 Bağlantı ve Echo işlemleri için ayrı service
    /// </summary>
    public class Gmp3ConnectionService
    {
        private readonly ILogger _log;

        public Gmp3ConnectionService(ILogger log)
        {
            _log = log;
        }

        /// <summary>
        /// Echo stabilize etme işlemi
        /// </summary>
        public int EchoStabilize(string iface, int echoTimeoutMs, int pairingTimeoutMs, int waitUntilOkMs)
        {
            _log.LogInformation("🔧 Echo stabilize başlatılıyor...");
            
            // 1) Echo dene (string ile)
            var echo = new ST_ECHO();
            var rc = Gmp3NativeMethods.Echo(iface, echoTimeoutMs);  // String!
            _log.LogInformation("ECHO({iface}) rc=0x{rc:X}", iface, rc);
            
            if (rc == Gmp3NativeMethods.TRAN_RESULT_OK || rc == Gmp3NativeMethods.DLL_RETCODE_HANDSHAKE)
            {
                _log.LogInformation("🎉 Echo OK! rc=0x{rc:X}", rc);
                return rc;
            }
            
            // 2) Echo başarısızsa pairing dene
            _log.LogInformation("🔧 Echo başarısız, pairing deneniyor...");
            var pairingService = new Gmp3PairingService(_log);
            var pairingRc = pairingService.DoQuickPairing(iface);
            
            if (pairingRc == Gmp3NativeMethods.TRAN_RESULT_OK)
            {
                _log.LogInformation("🎉 Pairing başarılı! rc=0x{rc:X}", pairingRc);
                
                // 3) Pairing sonrası Echo tekrar dene
                _log.LogInformation("🔧 Pairing sonrası Echo tekrar deneniyor...");
                rc = Gmp3NativeMethods.Echo(iface, echoTimeoutMs);  // String!
                _log.LogInformation("ECHO(pairing sonrası {iface}) rc=0x{rc:X}", iface, rc);
                
                if (rc == Gmp3NativeMethods.TRAN_RESULT_OK || rc == Gmp3NativeMethods.DLL_RETCODE_HANDSHAKE)
                {
                    _log.LogInformation("🎉 Echo OK! rc=0x{rc:X}", rc);
                    return rc;
                }
            }
            
            _log.LogWarning("⚠️ Echo stabilize başarısız: rc=0x{rc:X}", rc);
            return rc;
        }

        /// <summary>
        /// Echo OK bekleme işlemi
        /// </summary>
        public int WaitForEchoOk(string iface, int echoTimeoutMs, int pairingTimeoutMs, int waitUntilOkMs)
        {
            _log.LogInformation("🔧 Echo OK bekleme başlatılıyor...");
            
            var startTime = DateTime.Now;
            var timeout = TimeSpan.FromMilliseconds(waitUntilOkMs);
            
            while (DateTime.Now - startTime < timeout)
            {
                // Echo dene (string ile)
                var echo = new ST_ECHO();
                var last = Gmp3NativeMethods.Echo(iface, echoTimeoutMs);  // String!
                _log.LogInformation("ECHO(wait {iface}) rc=0x{rc:X}", iface, last);
                if (last == Gmp3NativeMethods.TRAN_RESULT_OK) 
                {
                    _log.LogInformation("🎉 Echo OK! rc=0x{rc:X}", last);
                    return last;
                }
                
                // Ping dene (string ile)
                last = Gmp3NativeMethods.Ping(iface, echoTimeoutMs);  // String!
                _log.LogInformation("PING(wait {iface}) rc=0x{rc:X}", iface, last);
                if (last == Gmp3NativeMethods.TRAN_RESULT_OK)
                {
                    _log.LogInformation("🎉 Ping OK! rc=0x{rc:X}", last);
                    return last;
                }
                
                Thread.Sleep(1000); // 1 saniye bekle
            }
            
            _log.LogWarning("⚠️ Echo OK timeout: {timeout}ms", waitUntilOkMs);
            return Gmp3NativeMethods.DLL_RETCODE_TIMEOUT;
        }
    }
}
