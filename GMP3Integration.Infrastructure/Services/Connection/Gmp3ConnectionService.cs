using GMP3Integration.Infrastructure.Interop;
using Microsoft.Extensions.Logging;
using System;

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
            
            // 1) Önce Echo dene
            var rc = Gmp3NativeMethods.Echo(iface, echoTimeoutMs);
            _log.LogInformation("ECHO({iface}) rc=0x{rc:X}", iface, rc);
            
            if (rc == Gmp3NativeMethods.TRAN_RESULT_OK)
            {
                _log.LogInformation("🎉 Echo OK! Stabilize edildi!");
                return rc;
            }
            
            // 2) Echo başarısızsa pairing dene
            if (rc == Gmp3NativeMethods.DLL_RETCODE_HANDSHAKE || rc == Gmp3NativeMethods.DLL_RETCODE_PAIRING_REQUIRED)
            {
                _log.LogInformation("🔧 Echo başarısız, pairing deneniyor...");
                // Pairing işlemi burada yapılacak
                return rc;
            }
            
            // 3) Diğer hatalar için son rc'yi döndür
            _log.LogWarning("⚠️ Echo stabilize başarısız! rc=0x{rc:X}", rc);
            return rc;
        }

        /// <summary>
        /// Echo OK bekleme işlemi
        /// </summary>
        public int WaitForEchoOk(string iface, int timeoutMs)
        {
            _log.LogInformation("🔧 Echo OK bekleniyor - {timeoutMs}ms...");
            
            var deadline = DateTime.UtcNow.AddMilliseconds(timeoutMs);
            
            while (DateTime.UtcNow < deadline)
            {
                // Küçük gecikme
                System.Threading.Thread.Sleep(1000);
                
                // Echo dene
                var rc = Gmp3NativeMethods.Echo(iface, 5000);
                _log.LogInformation("ECHO(wait {iface}) rc=0x{rc:X}", iface, rc);
                
                if (rc == Gmp3NativeMethods.TRAN_RESULT_OK)
                {
                    _log.LogInformation("🎉 Echo OK! Handshake tamamlandı!");
                    return rc;
                }

                // Handshake devam ediyorsa bekle
                if (rc == Gmp3NativeMethods.DLL_RETCODE_HANDSHAKE)
                {
                    continue; // Sadece bekle
                }

                // Diğer hatalar için son rc'yi döndür
                return rc;
            }
            
            _log.LogWarning("⏰ Echo OK timeout - {timeoutMs}ms sonra");
            return Gmp3NativeMethods.DLL_RETCODE_HANDSHAKE;
        }
    }
}
