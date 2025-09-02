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

        // === LEGACY COMPATIBILITY ===
        // These are kept for backward compatibility with existing code
        internal static class Iface_AnsiCdecl_x64
        {
            [DllImport("GMPSmartDLL.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "FP3_CreateInterface")]
            internal static extern int CreateInterface(string currentInterface);

            [DllImport("GMPSmartDLL.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "FP3_Close")]
            internal static extern int Close(string currentInterface, int timeoutMs);

            [DllImport("GMPSmartDLL.dll", EntryPoint = "FP3_Echo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = true)]
            internal static extern int Echo(string iface);
        }

        internal static class Iface_AnsiStd_x64
        {
            [DllImport("GMPSmartDLL.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall, EntryPoint = "FP3_CreateInterface")]
            internal static extern int CreateInterface(string currentInterface);

            [DllImport("GMPSmartDLL.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall, EntryPoint = "FP3_Close")]
            internal static extern int Close(string currentInterface, int timeoutMs);

            [DllImport("GMPSmartDLL.dll", EntryPoint = "FP3_Echo", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, ExactSpelling = true)]
            internal static extern int Echo(string iface);
        }

        internal static class Iface_UniStd_x64
        {
            [DllImport("GMPSmartDLL.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "FP3_CreateInterface")]
            internal static extern int CreateInterface(string currentInterface);

            [DllImport("GMPSmartDLL.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "FP3_Close")]
            internal static extern int Close(string currentInterface, int timeoutMs);

            [DllImport("GMPSmartDLL.dll", EntryPoint = "FP3_Echo", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true)]
            internal static extern int Echo(string iface);
        }

        // JSON Methods for backward compatibility
        internal static class JsonGmp3Methods
        {
            [DllImport("GMPSmartDLL.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall, EntryPoint = "JsonGmp3Methods_FP3_CreateInterface")]
            internal static extern int CreateInterface_All(string iface);

            [DllImport("GMPSmartDLL.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall, EntryPoint = "JsonGmp3Methods_FP3_StartPairingInit")]
            internal static extern int StartPairingInit(string iface, string jsonRequest, ref string jsonResponse, int timeout);

            [DllImport("GMPSmartDLL.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall, EntryPoint = "JsonGmp3Methods_FP3_Echo")]
            internal static extern int Echo(string iface, string jsonRequest, ref string jsonResponse, int timeout);
        }

        // === WRAPPER METHODS ===

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
        /// Wrapper for FP3_Close without transaction handle (legacy)
        /// </summary>
        internal static int FP3_Close(string iface, int timeout)
        {
            try { return Gmp3TransactionMethods.FP3_Close(iface, 0, timeout); } catch { }
            return DLL_RETCODE_INVALID_INTERFACE;
        }

        /// <summary>
        /// Wrapper for CreateInterface with fallback to legacy methods
        /// </summary>
        internal static int CreateInterface(string iface, int timeout)
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
        /// Wrapper for Echo with fallback to legacy methods
        /// </summary>
        internal static int Echo(string iface, int timeout)
        {
            try { return Gmp3InterfaceMethods.Echo(iface); } catch { }
            return DLL_RETCODE_INVALID_INTERFACE;
        }

        /// <summary>
        /// Wrapper for StartPairingInit with fallback to legacy methods
        /// </summary>
        internal static int StartPairingInit(string iface, ref Native.Structs.ST_GMP_PAIR pairing, int timeout)
        {
            try { return Gmp3InterfaceMethods.StartPairingInit(iface, ref pairing, timeout); } catch { }
            return DLL_RETCODE_INVALID_INTERFACE;
        }

        /// <summary>
        /// Wrapper for StartPairingInitWithPairing_All with fallback to legacy methods
        /// </summary>
        internal static int StartPairingInitWithPairing_All(string iface, ref Native.Structs.ST_GMP_PAIR pairing, ref Native.Structs.ST_GMP_PAIR_RESP pairingResp, int timeout)
        {
            try { return Gmp3InterfaceMethods.StartPairingInitWithPairing_All(iface, ref pairing, ref pairingResp, timeout); } catch { }
            return DLL_RETCODE_INVALID_INTERFACE;
        }

        /// <summary>
        /// Wrapper for StartPairingInit_All with fallback to legacy methods
        /// </summary>
        internal static int StartPairingInit_All(string iface, ref Native.Structs.ST_GMP_PAIR pairing, int timeout)
        {
            try { return Gmp3InterfaceMethods.StartPairingInit_All(iface, ref pairing, timeout); } catch { }
            return DLL_RETCODE_INVALID_INTERFACE;
        }

        /// <summary>
        /// Wrapper for Ping with fallback to legacy methods
        /// </summary>
        internal static int Ping(string iface, int timeout)
        {
            try { return Gmp3InterfaceMethods.Ping(iface); } catch { }
            return DLL_RETCODE_INVALID_INTERFACE;
        }

        /// <summary>
        /// Legacy CreateInterface method without timeout for backward compatibility
        /// </summary>
        internal static int CreateInterface(string iface)
        {
            return CreateInterface(iface, Gmp3Constants.DEFAULT_TIMEOUT);
        }

        /// <summary>
        /// Legacy Echo method without timeout for backward compatibility
        /// </summary>
        internal static int Echo(string iface)
        {
            return Echo(iface, Gmp3Constants.DEFAULT_TIMEOUT);
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
