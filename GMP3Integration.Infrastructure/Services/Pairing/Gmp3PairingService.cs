using GMP3Integration.Infrastructure.Interop;
using GMP3Integration.Infrastructure.Interop.Native.Structs;
using Microsoft.Extensions.Logging;
using System;

namespace GMP3Integration.Infrastructure.Services.Pairing
{
    /// <summary>
    /// GMP3 Pairing işlemleri için ayrı service
    /// </summary>
    public class Gmp3PairingService
    {
        private readonly ILogger _log;

        public Gmp3PairingService(ILogger log)
        {
            _log = log;
        }

        /// <summary>
        /// Emülatör pairing işlemi (JSON tabanlı - artık kullanılmıyor)
        /// </summary>
        public int DoEmulatorPairing(string iface)
        {
            _log.LogInformation("🔧 Emülatör pairing başlatılıyor...");
            
            // JSON tabanlı pairing artık kullanılmıyor, klasik pairing kullan
            return DoQuickPairing(iface);
        }

        /// <summary>
        /// Hızlı pairing işlemi
        /// </summary>
        public int DoQuickPairing(string iface)
        {
            _log.LogInformation("🔧 Hızlı pairing başlatılıyor...");
            
            // Emülatördeki gibi pairing bilgileri oluştur (hardcoded values)
            var pairing = new ST_GMP_PAIR();
            pairing.szExternalDeviceBrand = "WORLDLINE";
            pairing.szExternalDeviceModel = "IWE280";
            pairing.szExternalDeviceSerialNumber = "12344567";
            pairing.szEcrSerialNumber = "JHWE20000079";
            pairing.szProcOrderNumber = "000001";
            pairing.szProcDate = DateTime.Now.ToString("ddMMyy");
            pairing.szProcTime = DateTime.Now.ToString("HHmmss");
            
            _log.LogInformation("🔧 Pairing bilgileri: Brand={brand}, Model={model}, Serial={serial}", 
                pairing.szExternalDeviceBrand, pairing.szExternalDeviceModel, pairing.szExternalDeviceSerialNumber);
            
            // Emülatördeki gibi sadece StartPairingInit çağır (JSON-based)
            var rcInit = Gmp3NativeMethods.StartPairingInit(iface, ref pairing);
            _log.LogInformation("StartPairingInit({iface}) rc=0x{rc:X}", iface, rcInit);
            
            // Emülatördeki gibi response'u kontrol et
            if (rcInit == Gmp3NativeMethods.TRAN_RESULT_OK)
            {
                _log.LogInformation("🎉 Pairing başarılı! rc=0x{rc:X}", rcInit);
            }
            else
            {
                _log.LogWarning("⚠️ Pairing başarısız! rc=0x{rc:X}", rcInit);
            }
            
            return rcInit;
        }

        /// <summary>
        /// Sert pairing ve Echo OK bekleme
        /// </summary>
        public int HardPairAndWaitEchoOk(string iface, int echoTimeoutMs, int pairingTimeoutMs, int waitUntilOkMs)
        {
            // Emülatördeki gibi pairing bilgileri oluştur
            _log.LogInformation("🔧 Emülatördeki gibi pairing bilgileri oluşturuluyor...");
            
            var pairing = new ST_GMP_PAIR();
            // Emülatördeki gibi pairing bilgileri (hardcoded values)
            pairing.szExternalDeviceBrand = "WORLDLINE";
            pairing.szExternalDeviceModel = "IWE280";
            pairing.szExternalDeviceSerialNumber = "12344567";
            pairing.szEcrSerialNumber = "JHWE20000079";
            pairing.szProcOrderNumber = "000001";
            pairing.szProcDate = DateTime.Now.ToString("ddMMyy");
            pairing.szProcTime = DateTime.Now.ToString("HHmmss");
            
            _log.LogInformation("🔧 Pairing bilgileri: Brand={brand}, Model={model}, Serial={serial}", 
                pairing.szExternalDeviceBrand, pairing.szExternalDeviceModel, pairing.szExternalDeviceSerialNumber);
            
            // 1) Önce klasik pairing dene (JSON çalışmıyor)
            _log.LogInformation("🔧 FP3_StartPairingInit klasik yöntem ile deneniyor...");
            var pairing2 = new ST_GMP_PAIR();
            pairing2.szExternalDeviceBrand = "WORLDLINE";
            pairing2.szExternalDeviceModel = "IWE280";
            pairing2.szExternalDeviceSerialNumber = "12344567";
            pairing2.szEcrSerialNumber = "JHWE20000079";
            pairing2.szProcOrderNumber = "000001";
            pairing2.szProcDate = DateTime.Now.ToString("ddMMyy");
            pairing2.szProcTime = DateTime.Now.ToString("HHmmss");
            var rcInit = Gmp3NativeMethods.StartPairingInit_All(iface, ref pairing2, pairingTimeoutMs);  // String!
            _log.LogWarning("StartPairingInit({iface}) rc=0x{rc:X}", iface, rcInit);

            // 4) Bekle → Echo/Ping stabilize olana dek
            var deadline = DateTime.UtcNow.AddMilliseconds(waitUntilOkMs);
            int last = Gmp3NativeMethods.DLL_RETCODE_HANDSHAKE;
            _log.LogInformation("🔧 Pairing döngüsü başladı - {waitUntilOkMs}ms bekleniyor...");
            
            while (DateTime.UtcNow < deadline)
            {
                // Önce küçük bir gecikme
                System.Threading.Thread.Sleep(1000);

                // Klasik Echo dene (handle ile)
                var echo = new ST_ECHO();
                last = Gmp3NativeMethods.Echo(iface, ref echo, echoTimeoutMs);
                _log.LogInformation("ECHO(wait handle=0x{handle:X}) rc=0x{rc:X}", iface, last);
                if (last == Gmp3NativeMethods.TRAN_RESULT_OK) 
                {
                    _log.LogInformation("🎉 Pairing başarılı! Echo OK!");
                    return last;
                }
                // Ping dene (handle ile)
                last = Gmp3NativeMethods.Ping(iface, echoTimeoutMs);
                _log.LogInformation("PING(wait handle=0x{handle:X}) rc=0x{rc:X}", iface, last);
                if (last == Gmp3NativeMethods.TRAN_RESULT_OK) 
                {
                    _log.LogInformation("🎉 Pairing başarılı! Ping OK!");
                    return last;
                }

                // Handshake (0xF035) devam ediyorsa döngü sürsün
                continue;
            }

            _log.LogWarning("⏰ Pairing timeout oldu - {waitUntilOkMs}ms sonra son rc=0x{last:X}", waitUntilOkMs, last);
            return last; // genellikle 0xF035 olur → üst akış varyant değiştirir
        }
    }
}
