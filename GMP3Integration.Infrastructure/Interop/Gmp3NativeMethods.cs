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
using Newtonsoft.Json;

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

        // === JSON-BASED P/INVOKE METHODS (Emulator Style) ===
        [DllImport("GMPSmartDLL.dll", EntryPoint = "Json_FP3_CreateInterface", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint Json_FP3_CreateInterface(ref uint phInt, byte[] szID, byte IsDefault, byte[] szJsonXmlData);

        [DllImport("GMPSmartDLL.dll", EntryPoint = "Json_FP3_Echo", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint Json_FP3_Echo(uint hInt, byte[] szEcho_Out, int EchoLen_Out, int TimeoutInMiliseconds);

        [DllImport("GMPSmartDLL.dll", EntryPoint = "Json_FP3_StartPairingInit", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint Json_FP3_StartPairingInit(uint hInt, byte[] szPairing, byte[] szPairingResp, int PairingRespLen);

        // === WRAPPER METHODS ===

        /// <summary>
        /// Wrapper for CreateInterface (string-based, emulator style)
        /// </summary>
        internal static int CreateInterface(string iface)
        {
            try { return Gmp3InterfaceMethods.CreateInterface(iface); } catch { }
            return DLL_RETCODE_INVALID_INTERFACE;
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
        /// JSON-based Echo method (emulator style)
        /// </summary>
        internal static int Echo(string iface, ref ST_ECHO pStEcho, int timeout)
        {
            try 
            {
                // JSON kullanmadan basit Echo yap (EchoSimple gibi)
                // Çünkü EchoSimple zaten çalışıyor (0xF032 - UNKNOWN_ERROR)
                return EchoSimple(iface);
            } 
            catch (Exception ex) 
            {
                return DLL_RETCODE_INVALID_INTERFACE;
            }
        }

        /// <summary>
        /// JSON-based StartPairingInit method (emulator style)
        /// </summary>
        internal static int StartPairingInit(string iface, ref ST_GMP_PAIR pairing)
        {
            try 
            {
                // JSON kullanmadan basit pairing yap
                // Emülatördeki gibi hardcoded değerlerle
                return Gmp3InterfaceMethods.StartPairingInit(iface, ref pairing);
            } 
            catch (Exception ex) 
            {
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
                var result = Gmp3InterfaceMethods.CreateInterface("TCPIP");
                
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
