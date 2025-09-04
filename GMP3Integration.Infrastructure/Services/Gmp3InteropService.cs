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
using GMP3Integration.Application.DTOs.CloseTransaction;
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
using GMP3Integration.Infrastructure.Interop.Native.PInvoke;
using GMP3Integration.Infrastructure.Interop.Native.Enums;

namespace GMP3Integration.Infrastructure.Services
{
    public class Gmp3InteropService : IGmp3Service
    {
        private readonly ILogger<Gmp3InteropService> _log;

        private readonly string _xmlPath;
        private readonly Gmp3PairingService _pairingService;
        private readonly Gmp3ConnectionService _connectionService;
        
        // Session state tracking
        private static uint _currentInterfaceHandle = 0;
        private static ulong _currentTransactionHandle = 0;
        private static string _currentInterface = "";
        
        // Clear session state when transaction ends
        private static void ClearSessionState()
        {
            _currentInterfaceHandle = 0;
            _currentTransactionHandle = 0;
            _currentInterface = "";
        }

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
                    
                    // Emulator style: GERÇEK Handle döndürür!
                    uint interfaceHandle = 0;
                    var rc = Gmp3NativeMethods.CreateInterface(ifaceInput, ref interfaceHandle);
                    
                    // CRITICAL: Gerçek handle'ı GetInterfaceHandleByID ile al!
                    if (rc == Gmp3NativeMethods.TRAN_RESULT_OK || rc == Gmp3NativeMethods.DLL_RETCODE_CREATE_INTERFACE_SUCCESS)
                    {
                        var ifaceBytes = System.Text.Encoding.ASCII.GetBytes(ifaceInput + "\0");
                        var handleResult = Gmp3InterfaceMethods.FP3_GetInterfaceHandleByID(ref interfaceHandle, ifaceBytes);
                        _log.LogInformation("GetInterfaceHandleByID({iface}) result=0x{result:X}, realHandle={realHandle}", ifaceInput, handleResult, interfaceHandle);
                    }
                    
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
                        
                        // EMULATOR PATTERN: Clean Echo call
                        _log.LogInformation("🔧 Echo (Handshake) deneniyor...");
                        var echo = new ST_ECHO();
                        var bestEchoRc = Gmp3NativeMethods.Echo(ifaceInput, ref echo, 10000);
                        _log.LogInformation("FP3_Echo({iface}) rc=0x{rc:X}", ifaceInput, bestEchoRc);
                        
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
                        int pairingRc = -1;
                        try 
                        {
                            _log.LogInformation("🚀 DoQuickPairing çağrılıyor...");
                            pairingRc = _pairingService.DoQuickPairing(ifaceInput);
                            _log.LogInformation("✅ DoQuickPairing tamamlandı: rc=0x{rc:X}", pairingRc);
                        }
                        catch (Exception ex)
                        {
                            _log.LogError(ex, "❌ DoQuickPairing EXCEPTION: {message}", ex.Message);
                            pairingRc = Gmp3NativeMethods.DLL_RETCODE_INVALID_INTERFACE;
                        }
                        _log.LogInformation("FP3_StartPairingInit({iface}) rc=0x{rc:X}", ifaceInput, pairingRc);
                        
                        if (pairingRc == Gmp3NativeMethods.TRAN_RESULT_OK)
                        {
                            _log.LogInformation("🎉 Pairing başarılı! Emulator sequence devam ediyor...");
                            
                            // EMULATOR SEQUENCE: Skip GetDepartments & GetCurrency for now
                            // Emulator uses JSON-based + handle-based versions, we need to implement those first
                            _log.LogInformation("⏭️ EMULATOR: GetDepartments & GetCurrency skip (JSON-based versions needed)");
                            
                            // Focus on FP3_GetCurrentHandle which is the key for transaction handle
                            
                            // 3. FP3_Start - EMERGENCY FALLBACK: String-based ile stable transaction başlat
                            ulong transactionHandle = 0;
                            var uniqueId = new byte[24]; // Empty unique ID for ECR to generate
                            
                            // CORRECT EMULATOR PATTERN: GetCurrentHandle is for CHECK, then FP3_Start for CREATE
                            _log.LogInformation("🔍 EMULATOR: FP3_GetCurrentHandle ile mevcut transaction kontrol...");
                            
                            // Check existing transaction first (like emulator)
                            int checkRc = -1;
                            try 
                            {
                                checkRc = Gmp3NativeMethods.FP3_GetCurrentHandle(interfaceHandle, ref transactionHandle, uniqueId, uniqueId.Length, 10000);
                                _log.LogInformation("🔍 FP3_GetCurrentHandle CHECK: rc=0x{rc:X}, tranHandle=0x{handle:X}", checkRc, transactionHandle);
                                
                                // If we have an existing transaction, try to use it (0x90D means active transaction)
                                if (checkRc == 0x90D && transactionHandle != 0)
                                {
                                    _log.LogWarning("⚠️ Existing active transaction found! Handle: 0x{handle:X} - CLOSING IT to ensure clean state", transactionHandle);
                                    
                                    // FORCE CLEAN: Close existing transaction
                                    var closeResult = Gmp3NativeMethods.FP3_Close_Handle(interfaceHandle, transactionHandle, 10000);
                                    _log.LogInformation("🔄 FP3_Close existing transaction: rc=0x{rc:X}", closeResult);
                                    
                                    // Reset for new transaction creation
                                    transactionHandle = 0;
                                    
                                    _log.LogInformation("🆕 Creating fresh transaction after cleanup...");
                                }
                            }
                            catch (Exception ex) 
                            {
                                _log.LogError("❌ FP3_GetCurrentHandle CHECK EXCEPTION: {msg}", ex.Message);
                            }
                            
                            // Now CREATE transaction with FP3_Start (like emulator does after check)
                            _log.LogInformation("🚀 EMULATOR: FP3_Start ile yeni transaction yaratılıyor...");
                            transactionHandle = 0; // Reset for new transaction
                            int startRc = -1;
                            try 
                            {
                                // Try handle-based FP3_Start like emulator
                                startRc = Gmp3NativeMethods.FP3_Start_Handle(interfaceHandle, ref transactionHandle, uniqueId, 10000);
                                _log.LogInformation("✅ FP3_Start RESULT: rc=0x{rc:X}, tranHandle=0x{handle:X}", startRc, transactionHandle);
            }
            catch (Exception ex)
            {
                                _log.LogError("❌ FP3_Start EXCEPTION: {msg}", ex.Message);
                                startRc = Gmp3NativeMethods.DLL_RETCODE_INVALID_INTERFACE;
                            }
                            
                            // 0xF032 might be SUCCESS (old style warning) like in emulator
                            if (startRc == Gmp3NativeMethods.TRAN_RESULT_OK || startRc == 0xF032)
                            {
                                _log.LogInformation("🎉 Transaction başarıyla başlatıldı! Handle: 0x{handle:X}", transactionHandle);
                                
                                // Save session state for TicketHeader and other operations
                                _currentInterfaceHandle = interfaceHandle;
                                _currentTransactionHandle = transactionHandle;
                                _currentInterface = ifaceInput;
                                
                                _log.LogInformation("💾 Session state saved - Interface: {iface}, IHandle: 0x{ihandle:X}, THandle: 0x{thandle:X}", 
                                    _currentInterface, _currentInterfaceHandle, _currentTransactionHandle);
                                
                                return new StartTransactionResponse 
                                { 
                                    Success = true, 
                                    TransactionHandle = transactionHandle, // GERÇEK transaction handle!
                                    Rc = startRc,
                                    Message = "GMP3 transaction başarıyla başlatıldı - Ready for operations!",
                                    Interface = ifaceInput,
                                    InterfaceUsed = ifaceInput,
                                    ExistingOpenTicket = false
                                };
                            }
                            else
                            {
                                // Handle specific error cases
                                if (startRc == Gmp3NativeMethods.APP_ERR_ALREADY_DONE) // 0x820
                                {
                                    _log.LogWarning("⚠️ APP_ERR_ALREADY_DONE (0x820) - Transaction already exists! Trying to get current handle...");
                                    
                                    // Try to get the existing transaction handle
                                    try 
                                    {
                                        ulong existingHandle = 0;
                                        var getResult = Gmp3NativeMethods.FP3_GetCurrentHandle(interfaceHandle, ref existingHandle, uniqueId, uniqueId.Length, 10000);
                                        
                                        if (getResult == 0x90D && existingHandle != 0)
                                        {
                                            _log.LogInformation("✅ Found existing transaction! Handle: 0x{handle:X}", existingHandle);
                                            
                                            // Save session state and return existing transaction
                                            _currentInterfaceHandle = interfaceHandle;
                                            _currentTransactionHandle = existingHandle;
                                            _currentInterface = ifaceInput;
                                            
                                            return new StartTransactionResponse 
                                            { 
                                                Success = true, 
                                                TransactionHandle = existingHandle,
                                                Rc = getResult,
                                                Message = "Using existing transaction (recovered from APP_ERR_ALREADY_DONE)",
                                                Interface = ifaceInput,
                                                InterfaceUsed = ifaceInput,
                                                ExistingOpenTicket = true
                                            };
                }
            }
            catch (Exception ex)
            {
                                        _log.LogError("❌ Recovery attempt failed: {msg}", ex.Message);
                                    }
                                }
                                
                                _log.LogError("❌ FP3_Start başarısız! rc=0x{rc:X}", startRc);
                                return new StartTransactionResponse 
                                { 
                                    Success = false, 
                                    TransactionHandle = 0,
                                    Rc = startRc,
                                    Message = $"Transaction başlatılamadı - FP3_Start error: 0x{startRc:X}",
                                    Interface = ifaceInput,
                                    InterfaceUsed = ifaceInput,
                                    ExistingOpenTicket = false
                                };
                            }
                        }
                        else
                        {
                            _log.LogWarning("⚠️ Pairing başarısız: rc=0x{rc:X}", pairingRc);
                        }
                    }
                }
                
                _log.LogError("❌ Hiçbir interface başarılı olmadı!");
                return new StartTransactionResponse 
                { 
                    Success = false, 
                    TransactionHandle = 0,
                    Rc = -1,
                    ErrorMessage = "Hiçbir interface başarılı olmadı",
                    ExistingOpenTicket = false,
                    Interface = variants.FirstOrDefault()
                };
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "❌ StartTransaction hatası: {message}", ex.Message);
                return new StartTransactionResponse 
                { 
                    Success = false, 
                    TransactionHandle = 0,
                    Rc = -2,
                    ErrorMessage = ex.Message,
                    ExistingOpenTicket = false,
                    Interface = request?.CurrentInterface
                };
            }
        }

        // Diğer interface metodları için stub implementasyonlar
        public async Task<SetOptionFlagsResponse> SetOptionFlagsAsync(SetOptionFlagsRequest request)
        {
                return new SetOptionFlagsResponse { Success = true };
            }

        public async Task<SendTicketHeaderResponse> SendTicketHeaderAsync(SendTicketHeaderRequest request)
        {
            try
            {
                _log.LogInformation("🎫 SendTicketHeader başlatılıyor - Handle: 0x{handle:X}, TicketType: {type}", 
                    request.TransactionHandle, request.TicketType);

                // Ticket struct oluştur - MAP TO CORRECT ENUM VALUES!
                TTicketType correctTicketType;
                switch (request.TicketType)
                {
                    case 0: // SALE -> TProcessSale
                        correctTicketType = TTicketType.TProcessSale; // 1 (Fiscal Ticket)
                        break;
                    case 1: // REFUND -> TRefund  
                        correctTicketType = TTicketType.TRefund; // 15 (Non_Fiscal Ticket)
                        break;
                    default:
                        correctTicketType = TTicketType.TProcessSale; // Default to TProcessSale
                        break;
                }
                
                // DEBUG: Force SALE for now to test
                _log.LogInformation("🔧 Original TicketType: {orig} -> Mapped: {mapped}({val})", 
                    request.TicketType, correctTicketType, (int)correctTicketType);
                
                // TODO: Maybe we need FP3_GetTicketHeader first for merchant info?
                // UInt32 FP3_GetTicketHeader(UInt32 hInt, ushort IndexOfHeader, ref ST_TICKET_HEADER pStTicketHeader, ref ushort pNumberOfSpaceTotal, int TimeoutInMiliseconds)
                
                // SIMPLE APPROACH: Just pass TicketType enum directly!
                _log.LogInformation("🎫 SIMPLE CALL - TicketType: {type}({typeVal})", 
                    correctTicketType, (int)correctTicketType);

                // CORRECT HANDLE USAGE: Use saved session state!
                if (_currentInterfaceHandle == 0 || _currentTransactionHandle == 0)
                {
                    _log.LogError("❌ No active session! InterfaceHandle: 0x{ih:X}, TransactionHandle: 0x{th:X}", 
                        _currentInterfaceHandle, _currentTransactionHandle);
                    return new SendTicketHeaderResponse { Success = false };
                }
                
                _log.LogInformation("🔧 FP3_TicketHeader_Simple çağrılıyor - interfaceHandle: 0x{handle:X}", _currentInterfaceHandle);
                
                _log.LogInformation("🔧 FP3_TicketHeader parametreleri - interface: 0x{iface:X}, tran: 0x{tran:X}, type: {type}", 
                    _currentInterfaceHandle, _currentTransactionHandle, correctTicketType);
                
                var result = Gmp3NativeMethods.FP3_TicketHeader_Simple(
                    _currentInterfaceHandle,    // Correct interface handle
                    _currentTransactionHandle,  // Correct transaction handle
                    correctTicketType,          // Just the enum type!
                    10000);

                _log.LogInformation("🎫 FP3_TicketHeader sonucu - rc: 0x{rc:X}", result);

                if (result == 0) // Success
                {
                    return new SendTicketHeaderResponse 
                    { 
                        Success = true
                    };
                }
                else
                {
                    return new SendTicketHeaderResponse 
                    { 
                        Success = false
                    };
                }
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "❌ SendTicketHeader hatası");
                return new SendTicketHeaderResponse 
                { 
                    Success = false
                };
            }
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

        public async Task<CloseTransactionResponse> CloseTransactionAsync(CloseTransactionRequest request)
        {
            try
            {
                _log.LogInformation("🔴 CloseTransaction başlatılıyor...");
                
                // Check if we have active session
                if (_currentInterfaceHandle == 0 || _currentTransactionHandle == 0)
                {
                    _log.LogWarning("⚠️ Aktif transaction yok - Interface: 0x{iface:X}, Transaction: 0x{tran:X}", 
                        _currentInterfaceHandle, _currentTransactionHandle);
                    
                    return new CloseTransactionResponse
                    {
                        Success = false,
                        ResultCode = -1,
                        Message = "No active transaction to close"
                    };
                }

                _log.LogInformation("🔴 Aktif transaction kapatılıyor - Interface: 0x{iface:X}, Transaction: 0x{tran:X}", 
                    _currentInterfaceHandle, _currentTransactionHandle);
                
                // Close transaction using FP3_Close
                var closeResult = Gmp3NativeMethods.FP3_Close_Handle(_currentInterfaceHandle, _currentTransactionHandle, 10000);
                _log.LogInformation("🔴 FP3_Close sonucu: 0x{rc:X}", closeResult);
                
                // Clear session state after closing
                ClearSessionState();
                _log.LogInformation("✅ Session state temizlendi");
                
                if (closeResult == 0) // Success
                {
                    return new CloseTransactionResponse
                    {
                        Success = true,
                        ResultCode = 0,
                        Message = "Transaction closed successfully"
                    };
                }
                else
                {
                    return new CloseTransactionResponse
                    {
                        Success = false,
                        ResultCode = (int)closeResult,
                        Message = $"Close failed with code: 0x{closeResult:X}"
                    };
                }
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "❌ CloseTransaction hatası");
                return new CloseTransactionResponse
                {
                    Success = false,
                    ResultCode = -1,
                    Message = ex.Message
                };
            }
        }

        public async Task<ForceResetResponse> ForceResetAsync(ForceResetRequest request)
        {
            try
            {
                _log.LogInformation("🔄 ForceReset başlatılıyor - Session state ve DLL transaction'ları temizleniyor...");
                
                // FIRST: Close any active DLL transaction if we have handles
                if (_currentInterfaceHandle != 0 && _currentTransactionHandle != 0)
                {
                    _log.LogInformation("🔴 Aktif DLL transaction kapatılıyor - Interface: 0x{iface:X}, Transaction: 0x{tran:X}", 
                        _currentInterfaceHandle, _currentTransactionHandle);
                    
                    var closeResult = Gmp3NativeMethods.FP3_Close_Handle(_currentInterfaceHandle, _currentTransactionHandle, 10000);
                    _log.LogInformation("🔴 FP3_Close sonucu: 0x{rc:X}", closeResult);
                }
                else
                {
                    _log.LogInformation("ℹ️ Aktif transaction handle yok - sadece session state temizleniyor");
                }
                
                // SECOND: Clear our static session state
                ClearSessionState();
                
                _log.LogInformation("✅ Session state ve DLL transaction'ları temizlendi");
                
                return new ForceResetResponse 
                { 
                    Reset = true,
                    ResultCode = 0,
                    Message = "Interface session state cleared successfully"
                };
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "❌ ForceReset hatası");
                return new ForceResetResponse 
                { 
                    Reset = false,
                    ResultCode = -1,
                    Message = ex.Message
                };
            }
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

    }
}
