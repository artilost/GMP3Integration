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
        internal static int StartPairingInit_EmulatorWrapper(string iface, ref Native.Structs.ST_GMP_PAIR pairing)
        {
            // IMMEDIATE DEBUG - EN BAŞTA! - API LOG KULLAN!
            try {
                // API log'a yaz (Console çalışmıyor)
                System.Diagnostics.Debug.WriteLine($"🚀 WRAPPER ENTRY: StartPairingInit({iface})");
                File.AppendAllText("debug_handle.log", 
                    $"{DateTime.Now:HH:mm:ss.fff} 🚀 WRAPPER ENTRY: StartPairingInit({iface})\r\n");
            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine($"❌ DEBUG EXCEPTION: {ex.Message}");
            }
            
            try 
            {
                                                    // EMULATOR STYLE: Handle-based yaklaşım (en önemli!)
                // Artık gerçek handle'ı kullan (_currentInterfaceHandle CreateInterface'de set edildi)
                uint handle = _currentInterfaceHandle > 0 ? _currentInterfaceHandle : GetInterfaceHandle(iface);
                Debug.WriteLine($"🔗 Handle for {iface}: {handle} (current={_currentInterfaceHandle})");
                    
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
                    
                    // PRIMARY: EMULATOR PATTERN - Handle-based StartPairingInit
                    Debug.WriteLine($"🎯 EMULATOR: StartPairingInit(handle={handle})...");
                    var result = Gmp3InterfaceMethods.StartPairingInit(handle, ref pairing, ref pairingResp);
                    Debug.WriteLine($"EMULATOR StartPairingInit({handle}) rc=0x{result:X}");
                    
                    try {
                        File.AppendAllText("debug_handle.log", 
                            $"{DateTime.Now:HH:mm:ss.fff} 🎯 EMULATOR RESULT: 0x{result:X}\r\n");
                    } catch { }
                    
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
                    
                    // FALLBACK: Eski test methodları (debug için)
                    try {
                        File.AppendAllText("debug_handle.log", 
                            $"{DateTime.Now:HH:mm:ss.fff} ⚠️ EMULATOR pattern failed, trying OLD methods...\r\n");
                    } catch { }
                    
                    // TEST 1: OLD STYLE (0xF032 veren)
                    Debug.WriteLine($"🧪 FALLBACK: OLD STYLE pairing (handle={handle})...");
                    var result1 = Gmp3InterfaceMethods.StartPairingInit_Handle_Old(handle, ref pairing, ref pairingResp, 10000);
                    Debug.WriteLine($"StartPairingInit_Handle_Old({handle}) rc=0x{result1:X}");
                    if (result1 == 0xF032 || result1 == TRAN_RESULT_OK) return result1;
                    
                    // TEST 2: NEW STYLE 1
                    Debug.WriteLine($"🧪 FALLBACK: NEW STYLE 1 pairing (handle={handle})...");
                    var result2 = Gmp3InterfaceMethods.StartPairingInit_Handle_NewStyle1(handle, ref pairing, 10000);
                    Debug.WriteLine($"StartPairingInit_Handle_NewStyle1({handle}) rc=0x{result2:X}");
                    if (result2 == TRAN_RESULT_OK) return result2;
                    
                    // TEST 3: NEW STYLE 2
                    Debug.WriteLine($"🧪 FALLBACK: NEW STYLE 2 pairing (handle={handle})...");
                    var result3 = Gmp3InterfaceMethods.StartPairingInit_Handle_NewStyle2(handle, pairing, ref pairingResp, 10000);
                    Debug.WriteLine($"StartPairingInit_Handle_NewStyle2({handle}) rc=0x{result3:X}");
                    if (result3 == TRAN_RESULT_OK) return result3;
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
                
                // TEST: String StdCall convention
                var resultStringStdCall = Gmp3InterfaceMethods.StartPairingInit_StdCall(iface, ref pairing);
                if (resultStringStdCall != DLL_RETCODE_INVALID_INTERFACE) return resultStringStdCall;
                
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

        internal static int EchoSimple(string iface)
        {
            try { return Gmp3InterfaceMethods.EchoSimple(iface); } catch { }
            return DLL_RETCODE_INVALID_INTERFACE;
        }

        internal static int EchoWithTimeout(string iface, int timeout)
        {
            try { return Gmp3InterfaceMethods.EchoWithTimeout(iface, timeout); } catch { }
            return DLL_RETCODE_INVALID_INTERFACE;
        }

        internal static int EchoBasic(string iface)
        {
            try { return Gmp3InterfaceMethods.EchoBasic(iface); } catch { }
            return DLL_RETCODE_INVALID_INTERFACE;
        }

        internal static int EchoGmp3(string iface)
        {
            try { return Gmp3InterfaceMethods.EchoGmp3(iface); } catch { }
            return DLL_RETCODE_INVALID_INTERFACE;
        }

        internal static int EchoTest(string iface)
        {
            try { return Gmp3InterfaceMethods.EchoTest(iface); } catch { }
            return DLL_RETCODE_INVALID_INTERFACE;
        }

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

        internal static int StartPairingInit_All(string iface, ref Native.Structs.ST_GMP_PAIR pairing, int timeout)
        {
            try { return Gmp3InterfaceMethods.StartPairingInit_All(iface, ref pairing, timeout); } catch { }
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
        /// Wrapper for FP3_Close with fallback to legacy methods
        /// </summary>
        internal static int FP3_Close(string iface, ulong tranHandle, int timeout)
        {
            try { return Gmp3TransactionMethods.FP3_Close(iface, tranHandle, timeout); } catch { }
            return DLL_RETCODE_INVALID_INTERFACE;
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
