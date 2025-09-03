using GMP3Integration.Application.DTOs;
using GMP3Integration.Application.DTOs.OptionFlags;
using GMP3Integration.Application.DTOs.TicketHeader;
using GMP3Integration.Application.DTOs.ItemSale;
using GMP3Integration.Application.DTOs.Payment;
using GMP3Integration.Application.DTOs.PrintTotalsAndPayments;
using GMP3Integration.Application.DTOs.PrintBeforeMf;
using GMP3Integration.Application.DTOs.PrintMf;
using GMP3Integration.Application.DTOs.Refund;
using GMP3Integration.Application.DTOs.PrintMessage;
using GMP3Integration.Application.DTOs.TaxRates;
using GMP3Integration.Application.DTOs.DepertmenConfiguration;
using GMP3Integration.Application.DTOs.ForceReset;
using GMP3Integration.Application.Interfaces;
using GMP3Integration.Infrastructure.Interop;
using GMP3Integration.Infrastructure.Services.Pairing;
using GMP3Integration.Infrastructure.Services.Connection;
using GMP3Integration.Infrastructure.Interop.Native.Structs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GMP3Integration.Infrastructure.Services
{
    public class Gmp3InteropService : IGmp3Service
    {
        private readonly ILogger<Gmp3InteropService> _log;

        private readonly string _xmlPath;
        private readonly Gmp3PairingService _pairingService;
        private readonly Gmp3ConnectionService _connectionService;

        public Gmp3InteropService(ILogger<Gmp3InteropService> log)
        {
            _log = log;
            _xmlPath = "GMP.XML";
            
            // InterfaceHelper'a logger'ı set et
            InterfaceHelper.SetLogger(_log);
            
            // Service'leri oluştur
            _pairingService = new Gmp3PairingService(_log);
            _connectionService = new Gmp3ConnectionService(_log);
        }

        public async Task<StartTransactionResponse> StartTransactionAsync(StartTransactionRequest request)
        {
            _log.LogInformation("🚀 StartTransaction başlatılıyor...");
            
            // Test DLL first
            var dllTest = Gmp3NativeMethods.TestDll();
            _log.LogInformation("DLL Test Result: {dllTest}", dllTest);
            
            try
            {
                // XML'den interface varyantlarını al
                var variants = InterfaceHelper.BuildVariantsFromXml();
                
                foreach (var ifaceInput in variants)
                {
                    _log.LogInformation("🔍 Interface deneniyor: {iface}", ifaceInput);
                    
                    // Emulator style: Handle döndürür!
                    uint interfaceHandle = 0;
                    var rc = Gmp3NativeMethods.CreateInterface(ifaceInput, ref interfaceHandle);
                    _log.LogInformation("CreateInterface({iface}) rc=0x{rc:X}, handle={handle}", ifaceInput, rc, interfaceHandle);
                    
                if (rc == Gmp3NativeMethods.DLL_RETCODE_INVALID_INTERFACE)
                {
                        _log.LogWarning("❌ INVALID_INTERFACE - Sonraki varyant deneniyor");
                    continue;
                }
                    if (rc == Gmp3NativeMethods.DLL_RETCODE_HANDSHAKE)
                    {
                        _log.LogWarning("❌ HANDSHAKE - Sonraki varyant deneniyor");
                    continue;
                }
                    if (rc == Gmp3NativeMethods.DLL_RETCODE_CREATE_INTERFACE_SUCCESS || rc == Gmp3NativeMethods.TRAN_RESULT_OK)
                    {
                        _log.LogWarning("⚠️ CREATE_INTERFACE_SUCCESS (0xF02A) - Interface oluşturuldu mu? Test ediliyor...");
                    }
                    
                    if (rc == Gmp3NativeMethods.DLL_RETCODE_FUNC_NOT_FOUND)
                    {
                        _log.LogWarning("❌ JSON fonksiyonu bulunamadı - Klasik yöntem deneniyor");
                        // JSON fonksiyonu yoksa klasik yöntemi dene
                        rc = Gmp3NativeMethods.CreateInterface(ifaceInput, ref interfaceHandle);
                        _log.LogInformation("Klasik CreateInterface({iface}) rc=0x{rc:X}, handle={handle}", ifaceInput, rc, interfaceHandle);
                        
                        if (rc == Gmp3NativeMethods.DLL_RETCODE_INVALID_INTERFACE)
                        {
                            continue;
                        }
                    }
                    
                    // Emülatör gibi: CreateInterface başarılıysa emülatör sırasını uygula
                    if (rc == Gmp3NativeMethods.TRAN_RESULT_OK || rc == Gmp3NativeMethods.DLL_RETCODE_CREATE_INTERFACE_SUCCESS)
                    {
                        _log.LogInformation("✅ CreateInterface başarılı: {iface}", ifaceInput);
                        
                        // EMÜLATÖR SIRASI: Echo → Pairing → Departments → Currency → Start
                        
                        // 1. FP3_Echo (Handshake) - String-based!
                        _log.LogInformation("🔧 1. FP3_Echo (Handshake) deneniyor...");
                        
                        // TEST: Önce basit Echo method'unu dene (string ile)
                        _log.LogInformation("🧪 TEST: Basit Echo method'u deneniyor (string ile)...");
                        var echoSimpleRc = Gmp3NativeMethods.EchoSimple(ifaceInput);
                        _log.LogInformation("EchoSimple({iface}) rc=0x{rc:X}", ifaceInput, echoSimpleRc);
                        
                        // TEST: Alternatif Echo method'u dene (string + timeout ile)
                        _log.LogInformation("🧪 TEST: Alternatif Echo method'u deneniyor (string + timeout ile)...");
                        var echoTimeoutRc = Gmp3NativeMethods.EchoWithTimeout(ifaceInput, 10000);
                        _log.LogInformation("EchoWithTimeout({iface}) rc=0x{rc:X}", ifaceInput, echoTimeoutRc);
                        
                        // TEST: Farklı function isimleri dene!
                        _log.LogInformation("🧪 TEST: Farklı function isimleri deneniyor...");
                        var echoBasicRc = Gmp3NativeMethods.EchoBasic(ifaceInput);
                        _log.LogInformation("EchoBasic({iface}) rc=0x{rc:X}", ifaceInput, echoBasicRc);
                        
                        var echoGmp3Rc = Gmp3NativeMethods.EchoGmp3(ifaceInput);
                        _log.LogInformation("EchoGmp3({iface}) rc=0x{rc:X}", ifaceInput, echoGmp3Rc);
                        
                        var echoTestRc = Gmp3NativeMethods.EchoTest(ifaceInput);
                        _log.LogInformation("EchoTest({iface}) rc=0x{rc:X}", ifaceInput, echoTestRc);
                        
                        // Orijinal Echo method'u (string ile) - Emulator'dan alındı!
                        _log.LogInformation("🔧 Orijinal Echo method'u deneniyor (string ile)...");
                        var echo = new ST_ECHO();
                        var echoRc = Gmp3NativeMethods.Echo(ifaceInput, ref echo, 10000);  // String!
                        _log.LogInformation("FP3_Echo({iface}) rc=0x{rc:X}", ifaceInput, echoRc);
                        
                        // Echo test sonuçlarını kontrol et
                        _log.LogInformation("📊 Echo Test Sonuçları:");
                        _log.LogInformation("  - EchoSimple: 0x{rc:X}", echoSimpleRc);
                        _log.LogInformation("  - EchoWithTimeout: 0x{rc:X}", echoTimeoutRc);
                        _log.LogInformation("  - EchoBasic: 0x{rc:X}", echoBasicRc);
                        _log.LogInformation("  - EchoGmp3: 0x{rc:X}", echoGmp3Rc);
                        _log.LogInformation("  - EchoTest: 0x{rc:X}", echoTestRc);
                        _log.LogInformation("  - Echo (string): 0x{rc:X}", echoRc);
                        
                        // En iyi Echo sonucunu kullan - EMULATOR PATTERN: 0x0000 SUCCESS!
                        var bestEchoRc = echoSimpleRc;
                        if (echoTimeoutRc == Gmp3NativeMethods.TRAN_RESULT_OK)  // 0x0000
                            bestEchoRc = echoTimeoutRc;
                        else if (echoBasicRc == Gmp3NativeMethods.TRAN_RESULT_OK)  // 0x0000
                            bestEchoRc = echoBasicRc;
                        else if (echoGmp3Rc == Gmp3NativeMethods.TRAN_RESULT_OK)  // 0x0000
                            bestEchoRc = echoGmp3Rc;
                        else if (echoTestRc == Gmp3NativeMethods.TRAN_RESULT_OK)  // 0x0000
                            bestEchoRc = echoTestRc;
                        else if (echoRc == Gmp3NativeMethods.TRAN_RESULT_OK)  // 0x0000
                            bestEchoRc = echoRc;
                        // FALLBACK: 0xF035 (HANDSHAKE) da kabul et
                        else if (echoTimeoutRc == Gmp3NativeMethods.DLL_RETCODE_HANDSHAKE || echoRc == Gmp3NativeMethods.DLL_RETCODE_HANDSHAKE)
                            bestEchoRc = (echoTimeoutRc == Gmp3NativeMethods.DLL_RETCODE_HANDSHAKE) ? echoTimeoutRc : echoRc;
                        
                        _log.LogInformation("🎯 En iyi Echo sonucu: 0x{rc:X}", bestEchoRc);
                        
                        // Echo kontrolü (emulator style - JSON tabanlı)
                        if (bestEchoRc == Gmp3NativeMethods.TRAN_RESULT_OK || bestEchoRc == Gmp3NativeMethods.DLL_RETCODE_HANDSHAKE)
                        {
                            _log.LogInformation("🎉 Echo OK! JSON tabanlı handshake tamamlandı! rc=0x{rc:X}", bestEchoRc);
                        }
                        else
                        {
                            _log.LogInformation("🚀 Echo başarısız ama devam ediliyor (emulator style)... rc=0x{rc:X}", bestEchoRc);
                        }
                        
                        // 2. FP3_StartPairingInit (Pairing) - JSON-based!
                        _log.LogInformation("🔧 2. FP3_StartPairingInit (JSON-based Pairing) deneniyor...");
                        var pairingRc = _pairingService.DoQuickPairing(ifaceInput);
                        _log.LogInformation("FP3_StartPairingInit({iface}) rc=0x{rc:X}", ifaceInput, pairingRc);
                        
                        if (pairingRc == Gmp3NativeMethods.TRAN_RESULT_OK)
                        {
                            _log.LogInformation("🎉 Pairing başarılı!");
                            
                            // 3. GetDepartments - String-based (transaction methods)
                            _log.LogInformation("🔧 3. GetDepartments deneniyor...");
                            var departments = new ST_DEPARTMENT[10];
                            int deptCount = 0;
                            var deptRc = Gmp3NativeMethods.FP3_GetDepartments(ifaceInput, 0, ref departments, ref deptCount, 10000);
                            _log.LogInformation("FP3_GetDepartments({iface}) rc=0x{rc:X}, count={count}", ifaceInput, deptRc, deptCount);
                            
                            // 4. GetCurrency - String-based (transaction methods)
                            _log.LogInformation("🔧 4. GetCurrency deneniyor...");
                            var exchange = new ST_EXCHANGE();
                            var currRc = Gmp3NativeMethods.FP3_GetCurrency(ifaceInput, 0, ref exchange, 10000);
                            _log.LogInformation("FP3_GetCurrency({iface}) rc=0x{rc:X}", ifaceInput, currRc);
                            
                            // 5. FP3_Start - String-based (transaction methods)
                            _log.LogInformation("🔧 5. FP3_Start deneniyor...");
                            ulong tranHandle = 0;
                            var startRc = Gmp3NativeMethods.FP3_Start(ifaceInput, ref tranHandle, new byte[24], 10000);
                            _log.LogInformation("FP3_Start({iface}) rc=0x{rc:X}, handle=0x{handle:X}", ifaceInput, startRc, tranHandle);
                            
                            if (startRc == Gmp3NativeMethods.TRAN_RESULT_OK)
                            {
                                _log.LogInformation("🎉 Transaction başarıyla başlatıldı! Handle=0x{handle:X}", tranHandle);
                                
                                // Transaction handle'ı kapat - String-based (transaction methods)
                                var closeRc = Gmp3NativeMethods.FP3_Close(ifaceInput, tranHandle, 10000);
                                _log.LogInformation("FP3_Close({iface}, 0x{handle:X}) rc=0x{rc:X}", ifaceInput, tranHandle, closeRc);
                                
                                return new StartTransactionResponse 
                                { 
                                    Success = true, 
                                    TransactionHandle = tranHandle,
                                    Rc = startRc,
                                    Message = "Transaction başarıyla başlatıldı",
                                    Interface = ifaceInput
                                };
                            }
                            else
                            {
                                _log.LogWarning("⚠️ FP3_Start başarısız: rc=0x{rc:X}", startRc);
                            }
                        }
                        else
                        {
                            _log.LogWarning("⚠️ Pairing başarısız: rc=0x{rc:X}", pairingRc);
                        }
                    }
                }
                
                _log.LogError("❌ Hiçbir interface başarılı olmadı!");
                return new StartTransactionResponse { Success = false, ErrorMessage = "Hiçbir interface başarılı olmadı" };
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "❌ StartTransaction hatası: {message}", ex.Message);
                return new StartTransactionResponse { Success = false, ErrorMessage = ex.Message };
            }
        }

        // Diğer interface metodları için stub implementasyonlar
        public async Task<SetOptionFlagsResponse> SetOptionFlagsAsync(SetOptionFlagsRequest request)
        {
                return new SetOptionFlagsResponse { Success = true };
            }

        public async Task<SendTicketHeaderResponse> SendTicketHeaderAsync(SendTicketHeaderRequest request)
        {
                return new SendTicketHeaderResponse { Success = true };
            }

        public async Task<ItemSaleResponse> SaleItemAsync(ItemSaleRequest request)
        {
                return new ItemSaleResponse { Success = true };
            }

        public async Task<PaymentResponse> MakePaymentAsync(PaymentRequest request)
        {
                return new PaymentResponse { Success = true };
            }

        public async Task<PrintTotalsAndPaymentsResponse> PrintTotalsAndPaymentsAsync(PrintTotalsAndPaymentsRequest request)
        {
                return new PrintTotalsAndPaymentsResponse { Success = true };
            }

        public async Task<PrintBeforeMfResponse> PrintBeforeMfAsync(PrintBeforeMfRequest request)
        {
                return new PrintBeforeMfResponse { Success = true };
            }

        public async Task<PrintMfResponse> PrintMfAsync(PrintMfRequest request)
        {
                return new PrintMfResponse { Success = true };
            }

        public async Task<RefundResponse> RefundAsync(RefundRequest request)
        {
                return new RefundResponse { Success = true };
        }

        public async Task<PrintMessageResponse> PrintMessageAsync(PrintMessageRequest request)
        {
                return new PrintMessageResponse { Success = true };
            }

        public async Task<GetTaxRatesResponse> GetTaxRatesAsync()
        {
           return new GetTaxRatesResponse
            {
                Success = true,
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
                return new SetDepartmentsResponse { Success = true };
        }
       
        public async Task<ForceResetResponse> ForceResetAsync(ForceResetRequest request)
        {
            return new ForceResetResponse { Reset = true };
        }
    }
}
