using GMP3Integration.Application.DTOs;
using GMP3Integration.Application.DTOs.CancelTansaction;
using GMP3Integration.Application.DTOs.CanselTransaction;
using GMP3Integration.Application.DTOs.CloseTransaction;
using GMP3Integration.Application.DTOs.DepertmenConfiguration;
using GMP3Integration.Application.DTOs.ForceReset;
using GMP3Integration.Application.DTOs.ItemSale;
using GMP3Integration.Application.DTOs.OptionFlags;
using GMP3Integration.Application.DTOs.Payment;
using GMP3Integration.Application.DTOs.PrintBeforeMf;
using GMP3Integration.Application.DTOs.PrintMessage;
using GMP3Integration.Application.DTOs.PrintMf;
using GMP3Integration.Application.DTOs.PrintTotalsAndPayments;
using GMP3Integration.Application.DTOs.Refund;
using GMP3Integration.Application.DTOs.TaxRates;
using GMP3Integration.Application.DTOs.TicketHeader;
using GMP3Integration.Application.Interfaces;
using GMP3Integration.Application.Options;
using GMP3Integration.Infrastructure.Interop;
using GMP3Integration.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GMP3Integration.Infrastructure.Services
{
    public class Gmp3InteropService : IGmp3Service
    {

        private readonly IConfiguration _cfg;
        private readonly ILogger<Gmp3InteropService> _log;

        public Gmp3InteropService(IConfiguration cfg, ILogger<Gmp3InteropService> log)
        {
            _cfg = cfg;
            _log = log;
        }
        private const int RC_OK = 0x0000;
        private const int RC_INVALID_INTERFACE = 0xF034;
        private const int RC_HANDSHAKE = 0xF035;
        private const int RC_PAIRING_REQUIRED = 0xF020;
        public Task<StartTransactionResponse> StartTransactionAsync(StartTransactionRequest req)
        {
            var res = new StartTransactionResponse();



            string ifaceInput = req != null ? req.CurrentInterface : null;
            if (string.IsNullOrWhiteSpace(ifaceInput))
                throw new ArgumentException("CurrentInterface boş olamaz. Örn: TCP:192.168.137.99:7500");
            ifaceInput = ifaceInput.Trim();

            int echoTo = ReadInt("Gmp3:EchoTimeoutMs", 3000);
            int startTo = ReadInt("Gmp3:StartTimeoutMs", 5000);
            int pairTo = ReadInt("Gmp3:PairingTimeoutMs", 10000);
            int pairWait = ReadInt("Gmp3:PairingWaitMs", 60000);
            bool useIfaceOpen = ReadBool("Gmp3:UseInterfaceOpen", false); // default: BYPASS

            TryAppendNativePath();
            Interop.NativeExportsResolver.Init(_log);
            string nativeDir = GetNativeDirFromPathVar();
            _log.LogInformation("Native path: {dir}", nativeDir);
            _log.LogInformation("Proc64={p64} OS64={os64}", Environment.Is64BitProcess, Environment.Is64BitOperatingSystem);
            _log.LogInformation("CurrentDirectory: {dir}", Directory.GetCurrentDirectory());
            ConfigBootstrapper.EnsureXmlAliases(_log, nativeDir);

            InteropDiagnostics.DetectAndLogExports(_log);

            // Varyant setini üret
            var variants = InterfaceHelper.BuildVariants(ifaceInput);
            _log.LogInformation("IFACE VARIANTS ({n}): {v}", variants.Count, string.Join(" | ", variants));

            int lastRc = Gmp3NativeMethods.DLL_RETCODE_UNKNOWN_ECHO;

            // Tüm varyantları sırayla dene
            for (int i = 0; i < variants.Count; i++)
            {
                var iface = variants[i];
                _log.LogInformation(">> TRY iface='{iface}'", iface);

                // Ham TCP probe
                string host; int port;
                if (NetProbe.TryParseIface(iface, out host, out port))
                {
                    string perr;
                    bool ok = NetProbe.CanConnect(host, port, 2500, out perr);
                    _log.LogInformation("TCP probe {host}:{port} -> {ok} ({perr})", host, port, ok, perr ?? "OK");
                    if (!ok) { lastRc = Gmp3NativeMethods.DLL_RETCODE_PORT_NOT_OPEN; continue; }
                }

                // 1) ECHO stabilize
                // Debug: iface'in ham hali (görünmez karakter yakalamak için)
                _log.LogInformation("iface raw '{iface}' len={len} hex={hex}",
                    iface, iface.Length, BitConverter.ToString(Encoding.UTF8.GetBytes(iface)));

                // 1) ECHO stabilize
                var rc = EchoWithAnsiFallback(iface, echoTo);
                _log.LogInformation("ECHO({iface}) final rc=0x{rc:X}", iface, rc);

                //int rc = EchoStabilize(iface, echoTo);
                // _log.LogInformation("ECHO({iface}) final rc=0x{rc:X}", iface, rc);

                // 0xF034 (INVALID_INTERFACE) ise doğrudan bir sonraki varyanta geç
                if (rc == Gmp3NativeMethods.DLL_RETCODE_INVALID_INTERFACE)
                {
                    lastRc = rc;
                    _log.LogWarning("iface='{iface}' → 0xF034 (invalid interface). Next variant.", iface);
                    // close dene
                    try { Gmp3NativeMethods.InterfaceClose_All(iface, echoTo); } catch { }
                    continue;
                }

                // Pairing gerekliyse/unknown ise sert pairing
                if (rc == Gmp3NativeMethods.DLL_RETCODE_PAIRING_REQUIRED ||
                    rc == Gmp3NativeMethods.DLL_RETCODE_UNKNOWN_ECHO)
                {
                    rc = HardPairAndWaitEchoOk(iface, echoTo, pairTo, pairWait);
                    _log.LogInformation("HardPairAndWaitEchoOk({iface}) rc=0x{rc:X}", iface, rc);
                }

                if (rc != Gmp3NativeMethods.TRAN_RESULT_OK)
                {
                    lastRc = rc;
                    _log.LogWarning("ECHO fail iface='{iface}' rc=0x{rc:X} -> next variant", iface, rc);
                    try { Gmp3NativeMethods.InterfaceClose_All(iface, echoTo); } catch { }
                    continue;
                }



                // 2) START
                ulong handle = 0UL;
                //byte[] unique = new byte[16];

                byte[] unique = UniqueId16FromMachine();

                rc = Gmp3NativeMethods.Start(iface, ref handle, unique, startTo);
                _log.LogInformation("START({iface}) rc=0x{rc:X}, handle={h}", iface, rc, handle);

                if (rc == Gmp3NativeMethods.DLL_RETCODE_PAIRING_REQUIRED)
                {
                    int prc = HardPairAndWaitEchoOk(iface, echoTo, pairTo, pairWait);
                    _log.LogInformation("Start->Pair({iface}) rc=0x{rc:X}", iface, prc);
                    if (prc == Gmp3NativeMethods.TRAN_RESULT_OK)
                    {
                        handle = 0UL;
                        rc = Gmp3NativeMethods.Start(iface, ref handle, unique, startTo);
                        _log.LogInformation("START-AGAIN({iface}) rc=0x{rc:X}, handle={h}", iface, rc, handle);
                    }
                }

                if (rc == Gmp3NativeMethods.TRAN_RESULT_OK)
                {
                    try { Gmp3NativeMethods.InterfaceClose_All(iface, echoTo); } catch { }
                    return Task.FromResult(new StartTransactionResponse
                    {
                        Success = true,
                        TransactionHandle = handle,
                        Rc = rc,
                        Message = "OK",
                        ExistingOpenTicket = false
                    });
                }

                if (rc == Gmp3NativeMethods.APP_ERR_ALREADY_DONE)
                {
                    int grc = Gmp3NativeMethods.GetTicketShallow(iface, handle, echoTo);
                    try { Gmp3NativeMethods.InterfaceClose_All(iface, echoTo); } catch { }
                    return Task.FromResult(new StartTransactionResponse
                    {
                        Success = false,
                        TransactionHandle = handle,
                        Rc = rc,
                        ExistingOpenTicket = (grc == Gmp3NativeMethods.TRAN_RESULT_OK),
                        Message = "Açık fiş mevcut (APP_ERR_ALREADY_DONE)."
                    });
                }

                lastRc = rc;
                try { Gmp3NativeMethods.InterfaceClose_All(iface, echoTo); } catch { }
                _log.LogWarning("START fail iface='{iface}' rc=0x{rc:X} -> next variant", iface, rc);
            }

            // Tüm varyantlar bitti
            res.Success = false;
            res.TransactionHandle = 0;
            res.Rc = lastRc;
            res.Message = MapMessage("Tüm iface varyantları denendi", lastRc);
            return Task.FromResult(res);
        }
        private int EchoWithAnsiFallback(string iface, int echoTo)
        {
            // 1) Mevcut resolver ile dene
            var rc = Gmp3NativeMethods.Echo(iface, echoTo);
            if (rc != Gmp3NativeMethods.DLL_RETCODE_INVALID_INTERFACE)
                return rc;

            return Gmp3NativeMethods.DLL_RETCODE_INVALID_INTERFACE;
        }


        private int EchoFallbacks(string iface, int echoTo)
        {
            try
            {
                // İlk deneme zaten 0xF034 geldiği için farklı formatlarla yeniden deneyeceğiz
                string host = null;
                int port = 0;

                // iface içinden host/port ayıkla (mevcut yardımcıyı kullan)
                if (!NetProbe.TryParseIface(iface, out host, out port))
                {
                    // basit fallback: TCP:host:port gibi formatlardan host'u çıkar
                    var s = iface;
                    var p = s.IndexOf(':');
                    if (p >= 0) s = s.Substring(p + 1);
                    p = s.IndexOf(':');
                    if (p >= 0) s = s.Substring(0, p);
                    p = s.IndexOf(',');
                    if (p >= 0) s = s.Substring(0, p);
                    host = s.Trim();
                }

                if (string.IsNullOrWhiteSpace(host))
                    return Gmp3NativeMethods.DLL_RETCODE_INVALID_INTERFACE;

                // 1) ETHERNET:host (portsuz)
                int tryRc = Gmp3NativeMethods.Echo($"ETHERNET:{host}", echoTo);
                if (tryRc == Gmp3NativeMethods.TRAN_RESULT_OK || tryRc == Gmp3NativeMethods.DLL_RETCODE_HANDSHAKE)
                    return tryRc;

                // 2) ETHERNET:host,port (port biliniyorsa)
                if (port > 0)
                {
                    tryRc = Gmp3NativeMethods.Echo($"ETHERNET:{host},{port}", echoTo);
                    if (tryRc == Gmp3NativeMethods.TRAN_RESULT_OK || tryRc == Gmp3NativeMethods.DLL_RETCODE_HANDSHAKE)
                        return tryRc;
                }

                // 3) TCP:ip,port <-> TCP:ip:port swap
                var swapped1 = iface.Replace(':', ',');    // TCP:ip,port
                var swapped2 = iface.Contains(',')
                    ? iface.Replace(",", ":")              // TCP:ip:port
                    : iface;

                tryRc = Gmp3NativeMethods.Echo(swapped1, echoTo);
                if (tryRc == Gmp3NativeMethods.TRAN_RESULT_OK || tryRc == Gmp3NativeMethods.DLL_RETCODE_HANDSHAKE)
                    return tryRc;

                tryRc = Gmp3NativeMethods.Echo(swapped2, echoTo);
                if (tryRc == Gmp3NativeMethods.TRAN_RESULT_OK || tryRc == Gmp3NativeMethods.DLL_RETCODE_HANDSHAKE)
                    return tryRc;

                // 4) IP:host,port
                if (port > 0)
                {
                    tryRc = Gmp3NativeMethods.Echo($"IP:{host},{port}", echoTo);
                    if (tryRc == Gmp3NativeMethods.TRAN_RESULT_OK || tryRc == Gmp3NativeMethods.DLL_RETCODE_HANDSHAKE)
                        return tryRc;
                }

                // 5) TCPIP,host,port  (bazı build’ler)
                if (port > 0)
                {
                    tryRc = Gmp3NativeMethods.Echo($"TCPIP,{host},{port}", echoTo);
                    if (tryRc == Gmp3NativeMethods.TRAN_RESULT_OK || tryRc == Gmp3NativeMethods.DLL_RETCODE_HANDSHAKE)
                        return tryRc;
                }

                // 6) TCP:host (portsuz)
                tryRc = Gmp3NativeMethods.Echo($"TCP:{host}", echoTo);
                if (tryRc == Gmp3NativeMethods.TRAN_RESULT_OK || tryRc == Gmp3NativeMethods.DLL_RETCODE_HANDSHAKE)
                    return tryRc;

                // Hiçbiri tutmazsa 0xF034’e sadık kal
                return Gmp3NativeMethods.DLL_RETCODE_INVALID_INTERFACE;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "EchoFallbacks hata");
                return Gmp3NativeMethods.DLL_RETCODE_INVALID_INTERFACE;
            }
        }


        // ---- helpers ----
        private int EchoStabilize(string iface, int to)
        {
            int rc = RC_HANDSHAKE;
            try { rc = Gmp3NativeMethods.Echo(iface, to); } catch { }
            if (rc == RC_OK || rc == RC_INVALID_INTERFACE || rc == RC_HANDSHAKE || rc == RC_PAIRING_REQUIRED)
                return rc;


            // bazı DLL’lerde Echo yerine Ping geri dönebilir
            try { rc = Gmp3NativeMethods.Ping(iface, to); } catch { }
            return rc;
        }

        private int HardPairAndWaitEchoOk(string iface, int echoTimeoutMs, int pairingTimeoutMs, int waitUntilOkMs)
        {
            // 1) INIT
            var rcInit = Gmp3NativeMethods.StartPairingInit_All(iface, pairingTimeoutMs);
            _log.LogWarning("StartPairingInit_All({iface}) rc=0x{rc:X}", iface, rcInit);

            // 2) APPROVE (varsa)
            var rcApprove = Gmp3NativeMethods.StartPairingApprove_All(iface, pairingTimeoutMs);
            if (rcApprove != Gmp3NativeMethods.DLL_RETCODE_FUNC_NOT_FOUND)
                _log.LogWarning("StartPairingApprove_All({iface}) rc=0x{rc:X}", iface, rcApprove);
            else
                _log.LogInformation("StartPairingApprove not found in DLL; skipping.");


            // 4) Bekle → Echo/Ping stabilize olana dek
            var deadline = DateTime.UtcNow.AddMilliseconds(waitUntilOkMs);
            int last = Gmp3NativeMethods.DLL_RETCODE_HANDSHAKE;
            while (DateTime.UtcNow < deadline)
            {
                // Önce küçük bir gecikme
                System.Threading.Thread.Sleep(1000);

                // Bazı FW'lerde önce Ping, bazılarında Echo daha hızlı cevaplar
                last = Gmp3NativeMethods.Ping(iface, echoTimeoutMs);
                _log.LogInformation("PING(wait {iface}) rc=0x{rc:X}", iface, last);
                if (last == Gmp3NativeMethods.TRAN_RESULT_OK) return last;
                if (last == Gmp3NativeMethods.DLL_RETCODE_PAIRING_REQUIRED) goto REAPPROVE;

                last = Gmp3NativeMethods.Echo(iface, echoTimeoutMs);
                _log.LogInformation("ECHO(wait {iface}) rc=0x{rc:X}", iface, last);
                if (last == Gmp3NativeMethods.TRAN_RESULT_OK) return last;
                if (last == Gmp3NativeMethods.DLL_RETCODE_PAIRING_REQUIRED) goto REAPPROVE;

                // Handshake (0xF035) devam ediyorsa döngü sürsün
                continue;

            REAPPROVE:
                // Bazı cihazlar INIT'ten sonra tekrar approve/finalize bekler
                var rca = Gmp3NativeMethods.StartPairingApprove_All(iface, pairingTimeoutMs);
                if (rca != Gmp3NativeMethods.DLL_RETCODE_FUNC_NOT_FOUND)
                    _log.LogWarning("Re-Approve({iface}) rc=0x{rc:X}", iface, rca);

            }

            return last; // genellikle 0xF035 olur → üst akış varyant değiştirir
        }

        private static byte[] UniqueId16FromMachine()
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                var s = Environment.MachineName + "|" + Environment.UserName;
                return md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(s)); // 16 byte
            }
        }
        private int ReadInt(string key, int def)
        {
            int v; return int.TryParse(_cfg[key], out v) ? v : def;
        }
        private bool ReadBool(string key, bool def)
        {
            bool v; return bool.TryParse(_cfg[key], out v) ? v : def;
        }

        private void TryAppendNativePath()
        {
            try
            {
                string rel = _cfg["Gmp3:DllPath"];
                if (string.IsNullOrWhiteSpace(rel)) return;

                string baseDir = AppContext.BaseDirectory;
                string full = Path.GetFullPath(Path.Combine(baseDir, rel));
                if (!Directory.Exists(full)) return;

                string path = Environment.GetEnvironmentVariable("PATH");
                if (string.IsNullOrEmpty(path) || path.IndexOf(full, StringComparison.OrdinalIgnoreCase) < 0)
                {
                    Environment.SetEnvironmentVariable("PATH", (path ?? string.Empty) + Path.PathSeparator + full);
                    _log.LogInformation("Native PATH appended: {path}", full);
                }
            }
            catch (Exception ex)
            {
                _log.LogWarning(ex, "Native PATH append failed");
            }
        }

        private string GetNativeDirFromPathVar()
        {
            try
            {
                string rel = _cfg["Gmp3:DllPath"];
                if (string.IsNullOrWhiteSpace(rel)) return AppContext.BaseDirectory;
                string baseDir = AppContext.BaseDirectory;
                return Path.GetFullPath(Path.Combine(baseDir, rel));
            }
            catch { return AppContext.BaseDirectory; }
        }

        private static string MapMessage(string prefix, int rc)
        {
            string detail;
            switch (rc)
            {
                case Gmp3NativeMethods.TRAN_RESULT_OK: detail = "OK"; break;
                case Gmp3NativeMethods.DLL_RETCODE_TIMEOUT: detail = "Timeout/iletişim"; break;
                case Gmp3NativeMethods.DLL_RETCODE_ACK_NOT_RECEIVED: detail = "ACK alınamadı"; break;
                case Gmp3NativeMethods.DLL_RETCODE_RECV_BUSY: detail = "Cihaz meşgul (BUSY)"; break;
                case Gmp3NativeMethods.DLL_RETCODE_PORT_NOT_OPEN: detail = "Port/IP/XML/DLL"; break;
                case Gmp3NativeMethods.DLL_RETCODE_PAIRING_REQUIRED: detail = "Pairing gerekli"; break;
                case Gmp3NativeMethods.DLL_RETCODE_UNKNOWN_ECHO: detail = "Handshake (0xF035)"; break;
                case Gmp3NativeMethods.DLL_RETCODE_INVALID_INTERFACE: detail = "Interface/parametre geçersiz (0xF034)"; break;
                case Gmp3NativeMethods.APP_ERR_CASHIER_ENTRY_REQUIRED: detail = "Kasiyer girişi gerekli"; break;
                case Gmp3NativeMethods.APP_ERR_ALREADY_DONE: detail = "Açık işlem/fiş var"; break;
                case Gmp3NativeMethods.APP_ERR_GMP3_INVALID_HANDLE: detail = "Geçersiz handle"; break;
                case Gmp3NativeMethods.APP_ERR_GMP3_NO_HANDLE: detail = "Aktif handle yok"; break;
                case Gmp3NativeMethods.APP_ERR_GMP3_APP_CHECKSUM_MISMATCH: detail = "Checksum/Hash uyumsuzluğu"; break;
                default: detail = "Bilinmeyen rc"; break;
            }
            return prefix + " → " + detail + " (rc=0x" + rc.ToString("X") + "/" + rc + ")";
        }


        //--------------------------------
        public async Task<SetOptionFlagsResponse> SetOptionFlagsAsync(SetOptionFlagsRequest request)
        {
            try
            {
                Gmp3NativeMethods.FP3_OptionFlags_Native(
                    request.TransactionHandle,
                    request.ActiveFlags,
                    request.FlagsToBeSet
                    );
                return new SetOptionFlagsResponse { Success = true };
            }
            catch (NotImplementedException)
            {
                // placeholder davranış
                return new SetOptionFlagsResponse { Success = true };
            }
        }
        public async Task<SendTicketHeaderResponse> SendTicketHeaderAsync(SendTicketHeaderRequest request)
        {
            try
            {
                Gmp3NativeMethods.FP3_TicketHeader_Native(request.TransactionHandle, request.TicketType);
                return new SendTicketHeaderResponse { Success = true };
            }
            catch (NotImplementedException)
            {
                // Stub davranışı: her zaman başarılı dön
                return new SendTicketHeaderResponse { Success = true };
            }
        }
        public async Task<ItemSaleResponse> SaleItemAsync(ItemSaleRequest request)
        {
            try
            {
                Gmp3NativeMethods.FP3_ItemSale_Native(
                    request.TransactionHandle, request.Type, request.SubType, request.DeptIndex, request.Amount, request.CurrencyCode,
                    request.Count, request.UnitType, request.ItemCode ?? string.Empty, request.Name ?? string.Empty, request.Barcode ?? string.Empty, request.Flag);
                return new ItemSaleResponse { Success = true };
            }
            catch (NotImplementedException) { return new ItemSaleResponse { Success = true }; }
        }
        public async Task<PaymentResponse> MakePaymentAsync(PaymentRequest request)
        {

            try
            {
                Gmp3NativeMethods.FP3_Payment_Native(
                    request.TransactionHandle,
                    request.TypeOfPayment,
                    request.SubtypeOfPayment,
                    request.PayAmount,
                    request.PayAmountCurrencyCode,
                    string.IsNullOrWhiteSpace(request.BankPaymentUniqueId) ? string.Empty : request.BankPaymentUniqueId);
                return new PaymentResponse { Success = true };
            }
            catch (NotImplementedException) { return new PaymentResponse { Success = true }; }
        }
        public async Task<PrintTotalsAndPaymentsResponse> PrintTotalsAndPaymentsAsync(PrintTotalsAndPaymentsRequest request)
        {
            try
            {
                Gmp3NativeMethods.FP3_PrintTotalsAndPayments_Native(request.TransactionHandle);
                return new PrintTotalsAndPaymentsResponse { Success = true };
            }
            catch (NotImplementedException)
            {
                // Stub davranışı: her zaman başarılı dön
                return new PrintTotalsAndPaymentsResponse { Success = true };
            }
        }
        public async Task<PrintBeforeMfResponse> PrintBeforeMfAsync(PrintBeforeMfRequest request)
        {
            try
            {
                Gmp3NativeMethods.FP3_PrintBeforeMF_Native(request.TransactionHandle);
                return new PrintBeforeMfResponse { Success = true };
            }
            catch (NotImplementedException)
            {
                // Stub davranışı: placeholder olarak başarılı dön
                return new PrintBeforeMfResponse { Success = true };
            }
        }
        public async Task<PrintMfResponse> PrintMfAsync(PrintMfRequest request)
        {
            try
            {
                Gmp3NativeMethods.FP3_PrintMF_Native(request.TransactionHandle);
                return new PrintMfResponse { Success = true };
            }
            catch (NotImplementedException)
            {
                // Stub davranışı: placeholder olarak başarılı dön
                return new PrintMfResponse { Success = true };
            }
        }
        public async Task<RefundResponse> RefundAsync(RefundRequest request)
        {
            try
            {
                Gmp3NativeMethods.FP3_Refund_Native(request.TransactionHandle, request.Amount);
                return new RefundResponse { Success = true };
            }
            catch (NotImplementedException)
            {
                // Stub davranışı: her zaman başarılı dönüyoruz
                return new RefundResponse { Success = true };
            }
        }

        public async Task<PrintMessageResponse> PrintMessageAsync(PrintMessageRequest request)
        {
            try
            {
                Gmp3NativeMethods.FP3_PrintMessage_Native(request.TransactionHandle, request.MessageText);
                return new PrintMessageResponse { Success = true };

            }
            catch (NotImplementedException)
            {

                return new PrintMessageResponse { Success = true };
            }
        }
        public async Task<GetTaxRatesResponse> GetTaxRatesAsync()
        {
            // DLL gelene kadar stub veri döndürüyoruz
           return new GetTaxRatesResponse
            {
                Rates = new List<TaxRateDto>
                {
                    new TaxRateDto { Index = 0, TaxCode = "1",  Rate = 1.0m },
                    new TaxRateDto { Index = 1, TaxCode = "8",  Rate = 8.0m },
                    new TaxRateDto { Index = 2, TaxCode = "18", Rate = 18.0m }
                }
            };
        }
        public async Task<SetDepartmentsResponse> SetDepartmentsAsync(SetDepartmentsRequest request)
        {
            try
            {
                var arr = (request.Departments ?? new List<DepartmentConfigItem>()).ToArray();
                Gmp3NativeMethods.FP3_SetDepartments_Native(request.TransactionHandle, arr, arr.Length);
                return new SetDepartmentsResponse { Success = true };
            }
            catch (NotImplementedException)
            {
                // DLL yokken stub olarak başarılı sayıyoruz
                return new SetDepartmentsResponse { Success = true };
            }
        }
       

        public Task<ForceResetResponse> ForceResetAsync(ForceResetRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
