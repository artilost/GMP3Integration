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
                    
                    // Klasik CreateInterface dene (handle-based çalışmıyor)
                    var rc = Gmp3NativeMethods.CreateInterface(ifaceInput, 10000);
                    _log.LogInformation("CreateInterface({iface}) rc=0x{rc:X}", ifaceInput, rc);
                    
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
                        rc = Gmp3NativeMethods.JsonGmp3Methods.CreateInterface_All(ifaceInput);
                        _log.LogInformation("Klasik CreateInterface({iface}) rc=0x{rc:X}", ifaceInput, rc);
                        
                        if (rc == Gmp3NativeMethods.DLL_RETCODE_INVALID_INTERFACE)
                        {
                            continue;
                        }
                    }
                    
                    // Emülatör gibi: CreateInterface başarılıysa emülatör sırasını uygula
                    if (rc == Gmp3NativeMethods.TRAN_RESULT_OK || rc == Gmp3NativeMethods.DLL_RETCODE_CREATE_INTERFACE_SUCCESS)
                    {
                        _log.LogInformation("✅ CreateInterface başarılı: {iface}", ifaceInput);
                        
                        // Echo başarısız oluyor, direkt pairing dene
                        _log.LogInformation("🔧 Echo başarısız, direkt pairing deneniyor...");
                        var pairingRc = _pairingService.DoQuickPairing(ifaceInput);
                        
                        if (pairingRc == Gmp3NativeMethods.TRAN_RESULT_OK || pairingRc == Gmp3NativeMethods.DLL_RETCODE_HANDSHAKE)
                        {
                            // Pairing başarılı, handshake tamamlanana kadar bekle
                            _log.LogInformation("🔧 Pairing başarılı, handshake tamamlanana kadar bekleniyor...");
                            
                            // Handshake tamamlanana kadar Echo kontrol et
                            var echoRc = _connectionService.WaitForEchoOk(ifaceInput, 15000); // 15 saniye bekle
                            
                            if (echoRc == Gmp3NativeMethods.TRAN_RESULT_OK)
                            {
                                _log.LogInformation("🎉 Handshake tamamlandı! Echo OK!");
                                
                                // Transaction handle al
                                ulong tranHandle = 0;
                                var startRc = Gmp3NativeMethods.FP3_Start(ifaceInput, ref tranHandle, new byte[24], 10000);
                                _log.LogInformation("FP3_Start({iface}) rc=0x{rc:X}, handle=0x{handle:X}", ifaceInput, startRc, tranHandle);
                                
                                if (startRc == Gmp3NativeMethods.TRAN_RESULT_OK)
                                {
                                    _log.LogInformation("🎉 Transaction başarıyla başlatıldı! Handle=0x{handle:X}", tranHandle);
                                    
                                    // Interface'i kapat
                                    var closeRc = Gmp3NativeMethods.FP3_Close(ifaceInput, tranHandle, 10000);
                                    _log.LogInformation("FP3_Close({iface}) rc=0x{rc:X}", ifaceInput, closeRc);
                                    
                                    return new StartTransactionResponse 
                                    { 
                                        Success = true, 
                                        TransactionHandle = tranHandle,
                                        Interface = ifaceInput
                                    };
                                }
                                else
                                {
                                    _log.LogWarning("⚠️ FP3_Start başarısız! rc=0x{rc:X}", startRc);
                                }
                            }
                            else
                            {
                                _log.LogWarning("⚠️ Echo OK timeout! rc=0x{rc:X}", echoRc);
                            }
                        }
                        else
                        {
                            _log.LogWarning("⚠️ Pairing başarısız! rc=0x{rc:X}", pairingRc);
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
