using GMP3Integration.Application.DTOs.DepertmenConfiguration;
using GMP3Integration.Infrastructure.Services;
using GMP3Integration.Infrastructure.Interop.Native.Constants;
using GMP3Integration.Infrastructure.Interop.Native.Enums;
using GMP3Integration.Infrastructure.Interop.Native.Structs;
using GMP3Integration.Infrastructure.Interop.Native.PInvoke;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace GMP3Integration.Infrastructure.Interop
{
    /// <summary>
    /// GMP3 Native Methods - Wrapper class for all native DLL interactions
    /// This class provides high-level wrapper methods around the native P/Invoke calls
    /// </summary>
    internal class Gmp3NativeMethods
    {
        // === RETCODES === (Legacy compatibility - now using Gmp3Constants)
        public const int TRAN_RESULT_OK = Gmp3Constants.TRAN_RESULT_OK;
        public const int DLL_RETCODE_INVALID_INTERFACE = Gmp3Constants.DLL_RETCODE_INVALID_INTERFACE_FORMAT;
        public const int DLL_RETCODE_HANDSHAKE = Gmp3Constants.DLL_RETCODE_HANDSHAKE;
        public const int DLL_RETCODE_PAIRING_REQUIRED = Gmp3Constants.DLL_RETCODE_PAIRING_REQUIRED;
        public const int DLL_RETCODE_PORT_NOT_OPEN = Gmp3Constants.DLL_RETCODE_PORT_NOT_OPEN;
        public const int DLL_RETCODE_JSON_INVALID_INTERFACE = Gmp3Constants.DLL_RETCODE_JSON_INVALID_INTERFACE;
        public const int DLL_RETCODE_JSON_FUNCTION_ERROR = Gmp3Constants.DLL_RETCODE_JSON_FUNCTION_ERROR;
        public const int DLL_RETCODE_CREATE_INTERFACE_SUCCESS = Gmp3Constants.DLL_RETCODE_CREATE_INTERFACE_SUCCESS;
        public const int DLL_RETCODE_INTERFACE_NOT_SUPPORTED = Gmp3Constants.DLL_RETCODE_INTERFACE_NOT_SUPPORTED;
        public const int APP_ERR_ALREADY_DONE = Gmp3Constants.APP_ERR_ALREADY_DONE;
        
        // Additional legacy constants
        public const int DLL_RETCODE_FUNC_NOT_FOUND = unchecked((int)0xF0FE);
        public const int DLL_RETCODE_TIMEOUT = unchecked((int)0xF00B);
        public const int DLL_RETCODE_ACK_NOT_RECEIVED = unchecked((int)0xF00A);
        public const int DLL_RETCODE_RECV_BUSY = unchecked((int)0xF00E);

        // === JSON-BASED P/INVOKE METHODS (Emulator Style - ACİL DÜZELTİLDİ) ===
        [DllImport("GMPSmartDLL.dll", EntryPoint = "Json_FP3_CreateInterface", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint Json_FP3_CreateInterface(uint hInt, byte[] szJsonXmlData_Out, int JsonXmlDataLen_Out);

        [DllImport("GMPSmartDLL.dll", EntryPoint = "Json_FP3_Echo", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint Json_FP3_Echo(uint hInt, byte[] szEcho_Out, int EchoLen_Out, int TimeoutInMiliseconds);

        [DllImport("GMPSmartDLL.dll", EntryPoint = "Json_FP3_StartPairingInit", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint Json_FP3_StartPairingInit(uint hInt, byte[] szPairing, byte[] szPairingResp, int PairingRespLen);

        // === WRAPPER METHODS ===

        /// <summary>
        /// JSON-based CreateInterface (emulator style) - Handle döndürür!
        /// </summary>
        internal static int CreateInterface(string iface, ref uint handle)
        {
            // IMMEDIATE DEBUG - EN BAŞTA!
            try {
                var debugPath = Path.Combine(Directory.GetCurrentDirectory(), "debug_handle.log");
                File.AppendAllText(debugPath, 
                    $"{DateTime.Now:HH:mm:ss.fff} 🎯 CREATE ENTRY: CreateInterface({iface}) - Path: {debugPath}\r\n");
            } catch (Exception ex) {
                // Debug exception'ı da yakala
                try {
                    File.AppendAllText("debug_exception.log", 
                        $"{DateTime.Now:HH:mm:ss.fff} ❌ DEBUG EXCEPTION: {ex.Message}\r\n");
                } catch { }
            }
            
            try 
            {
                // Gerçek CreateInterface çağrısı - handle döndürür!
                var result = Gmp3InterfaceMethods.CreateInterface(iface, ref handle);
                
                // Handle'ı global olarak kaydet (emulator gibi)
                if (result == TRAN_RESULT_OK || result == DLL_RETCODE_CREATE_INTERFACE_SUCCESS)
                {
                    _currentInterfaceHandle = handle;
                    _currentInterfaceString = iface;
                    
                    try {
                        File.AppendAllText("debug_handle.log", 
                            $"{DateTime.Now:HH:mm:ss.fff} ✅ CreateInterface SUCCESS! iface={iface}, handle={handle}, rc=0x{result:X}\r\n");
                    } catch { }
                }
                
                return result;
            } 
            catch 
            { 
                return DLL_RETCODE_INVALID_INTERFACE;
            }
        }
        
        // Interface handle tracking (emulator style)
        private static uint _currentInterfaceHandle = 0x26DF345A; // Emulator'dan alınan handle  
        private static string _currentInterfaceString = "COM1";
        
        /// <summary>
        /// Get current interface handle for string interface
        /// </summary>
        internal static uint GetInterfaceHandle(string iface)
        {
            if (_currentInterfaceString == iface)
                return _currentInterfaceHandle;
                
            // Try to create interface and get handle
            uint newHandle = 0;
            var rc = CreateInterface(iface, ref newHandle);
            if (rc == TRAN_RESULT_OK || rc == DLL_RETCODE_CREATE_INTERFACE_SUCCESS)
                return _currentInterfaceHandle;
                
            return 0;
        }

        /// <summary>
        /// Wrapper for Close with fallback to legacy methods
        /// </summary>
        internal static int Close(string iface, int timeout)
        {
            try { return Gmp3InterfaceMethods.Close(iface, timeout); } catch { }
            return DLL_RETCODE_INVALID_INTERFACE;
        }

        /// <summary>
        /// Echo method (emulator style) - Handle-based!
        /// </summary>
        internal static int Echo(string iface, ref ST_ECHO pStEcho, int timeout)
        {
            try 
            {
                // EMULATOR PATTERN: Handle-based çağrı
                uint handle = _currentInterfaceHandle > 0 ? _currentInterfaceHandle : GetInterfaceHandle(iface);
                
                try {
                    File.AppendAllText("debug_handle.log", 
                        $"{DateTime.Now:HH:mm:ss.fff} 🔥 ECHO: Handle={handle} for {iface}\r\n");
                } catch { }
                
                // Primary: Handle-based Echo (emulator pattern)
                var result = Gmp3InterfaceMethods.Echo(handle, ref pStEcho, timeout);
                
                if (result == TRAN_RESULT_OK || result == DLL_RETCODE_HANDSHAKE)
                {
                    try {
                        File.AppendAllText("debug_handle.log", 
                            $"{DateTime.Now:HH:mm:ss.fff} ✅ ECHO SUCCESS with handle! rc=0x{result:X}\r\n");
                    } catch { }
                    return result;
                }
                
                // FALLBACK: String-based Echo
                return Gmp3InterfaceMethods.Echo_StringBased(iface, ref pStEcho, timeout); 
            } 
            catch (Exception ex) 
            {
                try {
                    File.AppendAllText("debug_handle.log", 
                        $"{DateTime.Now:HH:mm:ss.fff} ❌ ECHO EXCEPTION: {ex.Message}\r\n");
                } catch { }
                return DLL_RETCODE_INVALID_INTERFACE;
            }
        }

        /// <summary>
        /// StartPairingInit method (HANDLE-BASED EMULATOR STYLE) - UNIQUE WRAPPER
        /// </summary>
        internal static int StartPairingInit_EmulatorWrapper(string iface, ref Native.Structs.ST_GMP_PAIR pairing, bool isEmulatorPattern = true)
        {
            // WRAPPER ÇALIŞIYOR! Debug log'u log dosyasına yaz
            var logPath = Path.Combine(Directory.GetCurrentDirectory(), "debug_handle.log");
            try {
                File.AppendAllText(logPath, 
                    $"{DateTime.Now:HH:mm:ss.fff} 🚀 WRAPPER ENTRY: StartPairingInit_EmulatorWrapper({iface})\r\n");
                System.Diagnostics.Debug.WriteLine($"🚀 WRAPPER ENTRY: StartPairingInit_EmulatorWrapper({iface})");
            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine($"❌ DEBUG LOG EXCEPTION: {ex.Message}");
            }
            
            try 
            {
                                                    // EMULATOR STYLE: Handle-based yaklaşım (en önemli!)
                // EMULATOR PATTERN: GetInterfaceHandleByID kullan!
                uint handle = 0;
                var ifaceBytes = System.Text.Encoding.ASCII.GetBytes(iface + "\0");
                
                try {
                    File.AppendAllText(logPath, 
                        $"{DateTime.Now:HH:mm:ss.fff} 🔍 Trying FP3_GetInterfaceHandleByID for {iface}\r\n");
                        
                    var result = Gmp3InterfaceMethods.FP3_GetInterfaceHandleByID(ref handle, ifaceBytes);
                    File.AppendAllText(logPath, 
                        $"{DateTime.Now:HH:mm:ss.fff} 🔍 FP3_GetInterfaceHandleByID result: 0x{result:X}, handle: {handle}\r\n");
                        
                    // SUCCESS kontrolü - 0x0000 döndürse bile handle 0 olabilir
                    if (handle == 0 || result != TRAN_RESULT_OK) {
                        // CreateInterface ile yeni handle oluştur
                        uint newHandle = 0;
                        var createResult = CreateInterface(iface, ref newHandle);
                        File.AppendAllText(logPath, 
                            $"{DateTime.Now:HH:mm:ss.fff} 🆕 CreateInterface result: 0x{createResult:X}, newHandle: {newHandle}\r\n");
                            
                        if (newHandle > 0) {
                            handle = newHandle;
                            _currentInterfaceHandle = newHandle; // Cache et
                        } else {
                            handle = GetInterfaceHandle(iface); // Final fallback
                            File.AppendAllText(logPath, 
                                $"{DateTime.Now:HH:mm:ss.fff} 🔄 Final fallback handle: {handle}\r\n");
                        }
                    }
                } catch (Exception ex) {
                    File.AppendAllText(logPath, 
                        $"{DateTime.Now:HH:mm:ss.fff} ❌ GetInterfaceHandleByID exception: {ex.Message}\r\n");
                    handle = GetInterfaceHandle(iface); // Fallback
                }
                
                Debug.WriteLine($"🔗 Final handle for {iface}: {handle}");
                    
                    try {
                        File.AppendAllText("debug_handle.log", 
                            $"{DateTime.Now:HH:mm:ss.fff} 🔗 WRAPPER: Handle generated for {iface}: {handle}\r\n");
                    } catch { }
                
                try {
                    File.AppendAllText("debug_handle.log", 
                        $"{DateTime.Now:HH:mm:ss.fff} 🧮 Checking handle > 0: {handle} > 0 = {handle > 0}\r\n");
                } catch { }
                
                if (handle > 0)
                {
                    try {
                        File.AppendAllText("debug_handle.log", 
                            $"{DateTime.Now:HH:mm:ss.fff} 🎯 EMULATOR PATTERN: Using handle={handle}\r\n");
                    } catch { }
                    
                    var pairingResp = new ST_GMP_PAIR_RESP();
                    
                    // PRIMARY: EMULATOR JSON PATTERN - JSON-based StartPairingInit!
                    Debug.WriteLine($"🎯 EMULATOR JSON: Json_FP3_StartPairingInit(handle={handle})...");
                    
                    // JSON Serialize - System.Text.Json ile (runtime uyumluluğu için)
                    var pairingJson = System.Text.Json.JsonSerializer.Serialize(pairing);
                    var pairingBytes = System.Text.Encoding.ASCII.GetBytes(pairingJson + "\0"); // Null terminated!
                    var responseBytes = new byte[4096]; // Response buffer
                    
                    File.AppendAllText(logPath, 
                        $"{DateTime.Now:HH:mm:ss.fff} 📝 JSON Serialized: {pairingJson.Substring(0, Math.Min(100, pairingJson.Length))}...\r\n");
                    
                    var result = Gmp3InterfaceMethods.Json_FP3_StartPairingInit(handle, pairingBytes, responseBytes, responseBytes.Length);
                    Debug.WriteLine($"EMULATOR JSON StartPairingInit({handle}) rc=0x{result:X}");
                    
                    try {
                        File.AppendAllText(logPath, 
                            $"{DateTime.Now:HH:mm:ss.fff} 🎯 JSON EMULATOR RESULT: 0x{result:X}\r\n");
                            
                        // JSON Response Parse et (emulator gibi)
                        if (result == TRAN_RESULT_OK && responseBytes.Length > 0) {
                            var responseJson = System.Text.Encoding.ASCII.GetString(responseBytes).TrimEnd('\0');
                            if (!string.IsNullOrEmpty(responseJson)) {
                                try {
                                    // System.Text.Json ile parse et - DOĞRU TYPE!
                                    pairingResp = System.Text.Json.JsonSerializer.Deserialize<ST_GMP_PAIR_RESP>(responseJson);
                                    File.AppendAllText(logPath, 
                                        $"{DateTime.Now:HH:mm:ss.fff} ✅ JSON Response parsed: {responseJson.Substring(0, Math.Min(100, responseJson.Length))}...\r\n");
                                } catch (Exception parseEx) {
                                    File.AppendAllText(logPath, 
                                        $"{DateTime.Now:HH:mm:ss.fff} ❌ JSON Parse error: {parseEx.Message}\r\n");
                                }
                            }
                        }
                    } catch (Exception ex) {
                        File.AppendAllText(logPath, 
                            $"{DateTime.Now:HH:mm:ss.fff} ❌ JSON Exception: {ex.Message}\r\n");
                    }
                    
                    // EMULATOR BAŞARI KODU: 0x0000 (0)
                    if (result == TRAN_RESULT_OK) // 0x0000 = SUCCESS (emulator'dan öğrenilen!)
                    {
                        try {
                            File.AppendAllText("debug_handle.log", 
                                $"{DateTime.Now:HH:mm:ss.fff} 🎉 EMULATOR SUCCESS! StartPairingInit rc=0x{result:X}\r\n");
                        } catch { }
                        return result;
                    }
                    
                    // FALLBACK: 0xF032 da kabul et (eski test)
                    if (result == 0xF032) 
                    {
                        try {
                            File.AppendAllText("debug_handle.log", 
                                $"{DateTime.Now:HH:mm:ss.fff} 🎯 OLD SUCCESS! StartPairingInit rc=0x{result:X}\r\n");
                        } catch { }
                        return result;
                    }
                    
                    // FALLBACK: Simple old method test
                    try {
                        File.AppendAllText("debug_handle.log", 
                            $"{DateTime.Now:HH:mm:ss.fff} ⚠️ EMULATOR pattern failed, trying fallback...\r\n");
                    } catch { }
                    
                    var result1 = Gmp3InterfaceMethods.StartPairingInit_Handle_Old(handle, ref pairing, ref pairingResp, 10000);
                    if (result1 == 0xF032 || result1 == TRAN_RESULT_OK) return result1;
                }
                else
                {
                    try {
                        File.AppendAllText("debug_handle.log", 
                            $"{DateTime.Now:HH:mm:ss.fff} ❌ Handle <= 0, falling back to string methods...\r\n");
                    } catch { }
                    Debug.WriteLine($"⚠️ Handle generation failed for {iface}, falling back to string methods");
                }
                
                // FALLBACK: String-based methods
                
                // TEST: Original String Cdecl
                var resultStringCdecl = Gmp3InterfaceMethods.StartPairingInit_StringBased(iface, ref pairing);
                if (resultStringCdecl != DLL_RETCODE_INVALID_INTERFACE) return resultStringCdecl;
                
                
                
                return DLL_RETCODE_INVALID_INTERFACE;
            } 
            catch (Exception ex) 
            {
                try {
                    File.AppendAllText("debug_handle.log", 
                        $"{DateTime.Now:HH:mm:ss.fff} ❌ EXCEPTION in StartPairingInit: {ex.Message}\r\n");
                } catch { }
                return DLL_RETCODE_INVALID_INTERFACE;
            }
        }

        // === LEGACY COMPATIBILITY METHODS ===
        // These are kept for backward compatibility but should not be used for new code

        

        internal static int Ping(string iface, int timeout)
        {
            try { return Gmp3InterfaceMethods.Ping(iface); } catch { }
            return DLL_RETCODE_INVALID_INTERFACE;
        }

        internal static int StartPairingApprove(string iface, ref Native.Structs.ST_GMP_PAIR_RESP pairingResp, int timeout)
        {
            try { return Gmp3InterfaceMethods.StartPairingApprove(iface, ref pairingResp, timeout); } catch { }
            return DLL_RETCODE_INVALID_INTERFACE;
        }

        internal static int StartPairingInitWithPairing(string iface, ref Native.Structs.ST_GMP_PAIR pairing, ref Native.Structs.ST_GMP_PAIR_RESP pairingResp, int timeout)
        {
            try { return Gmp3InterfaceMethods.StartPairingInitWithPairing(iface, ref pairing, ref pairingResp, timeout); } catch { }
            return DLL_RETCODE_INVALID_INTERFACE;
        }



        internal static int StartPairingInitWithPairing_All(string iface, ref Native.Structs.ST_GMP_PAIR pairing, ref Native.Structs.ST_GMP_PAIR_RESP pairingResp, int timeout)
        {
            try { return Gmp3InterfaceMethods.StartPairingInitWithPairing_All(iface, ref pairing, ref pairingResp, timeout); } catch { }
            return DLL_RETCODE_INVALID_INTERFACE;
        }

        /// <summary>
        /// Get departments from GMP3 device
        /// </summary>
        internal static int FP3_GetDepartments(string iface, ulong tranHandle, ref ST_DEPARTMENT[] departments, ref int count, int timeout)
        {
            try { return Gmp3TransactionMethods.FP3_GetDepartments(iface, tranHandle, ref departments, ref count, timeout); } catch { }
            return DLL_RETCODE_INVALID_INTERFACE;
        }

        /// <summary>
        /// Get currency from GMP3 device
        /// </summary>
        internal static int FP3_GetCurrency(string iface, ulong tranHandle, ref ST_EXCHANGE exchange, int timeout)
        {
            try { return Gmp3TransactionMethods.FP3_GetCurrency(iface, tranHandle, ref exchange, timeout); } catch { }
            return DLL_RETCODE_INVALID_INTERFACE;
        }

        /// <summary>
        /// Wrapper for FP3_Start with fallback to legacy methods
        /// </summary>
        internal static int FP3_Start(string iface, ref ulong tranHandle, byte[] uniqueId, int timeout)
        {
            try { return Gmp3TransactionMethods.FP3_Start(iface, ref tranHandle, uniqueId, timeout); } catch { }
            return DLL_RETCODE_INVALID_INTERFACE;
        }

        /// <summary>
        /// Wrapper for FP3_GetCurrentHandle - EMULATOR PATTERN! (HANDLE-BASED)
        /// </summary>
        internal static int FP3_GetCurrentHandle(uint interfaceHandle, ref ulong tranHandle, byte[] uniqueId, int maxLengthOfUniqueId, int timeout)
        {
            try { return Gmp3TransactionMethods.FP3_GetCurrentHandle(interfaceHandle, ref tranHandle, uniqueId, maxLengthOfUniqueId, timeout); } catch { }
            return DLL_RETCODE_INVALID_INTERFACE;
        }

        /// <summary>
        /// Start transaction - HANDLE-BASED (Emulator Pattern!)
        /// </summary>
        internal static int FP3_Start_Handle(uint interfaceHandle, ref ulong tranHandle, byte[] uniqueId, int timeout)
        {
            try 
            { 
                // Ensure uniqueId is valid
                if (uniqueId == null || uniqueId.Length == 0)
                    uniqueId = new byte[24]; // Empty 24-byte array
                    
                // Convert uint to IntPtr for P/Invoke
                var handlePtr = new IntPtr(interfaceHandle);
                    
                File.AppendAllText("debug_handle.log", 
                    $"{DateTime.Now:HH:mm:ss.fff} 🚀 FP3_Start_Handle: handle={interfaceHandle} -> IntPtr={handlePtr}, timeout={timeout}, uniqueId.Length={uniqueId.Length}\r\n");
                    
                // Emulator'daki gibi parameters
                byte isBackground = 0; // Not background  
                var uniqueIdSign = new byte[0]; // Empty signature
                var userData = new byte[0]; // Empty user data
                
                var result = Gmp3TransactionMethods.FP3_Start_Handle(interfaceHandle, ref tranHandle, isBackground, 
                    uniqueId, uniqueId.Length, uniqueIdSign, uniqueIdSign.Length, userData, userData.Length, timeout);
                
                File.AppendAllText("debug_handle.log", 
                    $"{DateTime.Now:HH:mm:ss.fff} ✅ FP3_Start_Handle RESULT: 0x{result:X}, tranHandle=0x{tranHandle:X}\r\n");
                
                return result;
            } 
            catch (Exception ex)
            { 
                File.AppendAllText("debug_handle.log", 
                    $"{DateTime.Now:HH:mm:ss.fff} ❌ FP3_Start_Handle EXCEPTION: {ex.Message}\r\n");
                return DLL_RETCODE_INVALID_INTERFACE;
            }
        }


        /// <summary>
        /// Legacy Echo method without timeout for backward compatibility
        /// </summary>
        internal static int Echo(string iface)
        {
            var echo = new ST_ECHO();
            return Echo(iface, ref echo, Gmp3Constants.DEFAULT_TIMEOUT);
        }

        /// <summary>
        /// Send ticket header - CORRECT SIGNATURE (Simple TicketType only!)
        /// </summary>
        internal static uint FP3_TicketHeader_Simple(uint interfaceHandle, ulong tranHandle, TTicketType ticketType, int timeout)
        {
            try { return Gmp3TransactionMethods.FP3_TicketHeader(interfaceHandle, tranHandle, ticketType, timeout); } catch { }
            return DLL_RETCODE_INVALID_INTERFACE;
        }

        /// <summary>
        /// Close transaction - CORRECT SIGNATURE (Handle-based)
        /// </summary>
        internal static uint FP3_Close_Handle(uint interfaceHandle, ulong tranHandle, int timeout)
        {
            try 
            { 
                File.AppendAllText("debug_handle.log",
                    $"{DateTime.Now:HH:mm:ss.fff} 🔴 FP3_Close_Handle: iface=0x{interfaceHandle:X}, tran=0x{tranHandle:X}\r\n");
                
                var result = Gmp3TransactionMethods.FP3_Close(interfaceHandle, tranHandle, timeout);
                
                File.AppendAllText("debug_handle.log",
                    $"{DateTime.Now:HH:mm:ss.fff} ✅ FP3_Close_Handle RESULT: 0x{result:X}\r\n");
                
                return result;
            } 
            catch (Exception ex)
            {
                File.AppendAllText("debug_handle.log",
                    $"{DateTime.Now:HH:mm:ss.fff} ❌ FP3_Close_Handle EXCEPTION: {ex.Message}\r\n");
                return DLL_RETCODE_INVALID_INTERFACE;
            }
        }

        /// <summary>
        /// Test if DLL is working properly
        /// </summary>
        internal static int TestDll()
        {
            try
            {
                // Try to call a simple function to test DLL loading
                // Use a simple interface string that should work
                uint testHandle = 0;
                var result = Gmp3InterfaceMethods.CreateInterface("TCPIP", ref testHandle);
                
                // 0xF02A (61482) is actually CREATE_INTERFACE_SUCCESS, not an error
                if (result == DLL_RETCODE_CREATE_INTERFACE_SUCCESS || result == TRAN_RESULT_OK)
                {
                    return TRAN_RESULT_OK; // DLL is working
                }
                
                return result;
            }
            catch (Exception ex)
            {
                return DLL_RETCODE_FUNC_NOT_FOUND;
            }
        }
    }
}
