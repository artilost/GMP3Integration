using System.Runtime.InteropServices;
using GMP3Integration.Infrastructure.Interop.Native.Constants;
using GMP3Integration.Infrastructure.Interop.Native.Structs;

namespace GMP3Integration.Infrastructure.Interop.Native.PInvoke
{
    /// <summary>
    /// GMP3 Interface and Pairing-related P/Invoke methods
    /// </summary>
    public static class Gmp3InterfaceMethods
    {
        // Interface Management Methods
        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "FP3_CreateInterface")]
        public static extern int CreateInterface(string iface);  // Handle yok!

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "FP3_Close")]
        public static extern int Close(string iface, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "FP3_Echo")]
        public static extern int Echo(string iface, ref ST_ECHO pStEcho, int TimeoutInMiliseconds);  // String-based!

        // Basit Echo method'u (string ile) - Test için!
        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "FP3_Echo")]
        public static extern int EchoSimple(string iface);  // String ile!

        // Alternatif Echo method'u (string + timeout ile) - Test için!
        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "FP3_Echo")]
        public static extern int EchoWithTimeout(string iface, int timeout);  // String + timeout!

        // TEST: Farklı function isimleri dene!
        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "Echo")]
        public static extern int EchoBasic(string iface);  // "Echo" + string!

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "GMP3_Echo")]
        public static extern int EchoGmp3(string iface);  // "GMP3_Echo" + string!

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "EchoTest")]
        public static extern int EchoTest(string iface);  // "EchoTest" + string!

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "FP3_Ping")]
        public static extern int Ping(string iface);  // String-based!

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "FP3_Busy")]
        public static extern int Busy(string iface, int timeout);

        // Pairing Methods - String-based (Echo ile tutarlı!)
        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "FP3_StartPairingInit")]
        public static extern int StartPairingInit(string iface, ref ST_GMP_PAIR pairing);  // String-based!

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "FP3_StartPairingApprove")]
        public static extern int StartPairingApprove(string iface, ref ST_GMP_PAIR_RESP pairingResp, int timeout);  // String-based!

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "FP3_StartPairingInitWithPairing")]
        public static extern int StartPairingInitWithPairing(string iface, ref ST_GMP_PAIR pairing, ref ST_GMP_PAIR_RESP pairingResp, int timeout);  // String-based!

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "FP3_StartPairingInit_All")]
        public static extern int StartPairingInit_All(string iface, ref ST_GMP_PAIR pairing, int timeout);  // String-based!

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "FP3_StartPairingInitWithPairing_All")]
        public static extern int StartPairingInitWithPairing_All(string iface, ref ST_GMP_PAIR pairing, ref ST_GMP_PAIR_RESP pairingResp, int timeout);  // String-based!

        // JSON-based Methods
        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int JsonGmp3Methods_FP3_StartPairingInit(string iface, string jsonRequest, ref string jsonResponse, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int JsonGmp3Methods_FP3_StartPairingApprove(string iface, string jsonRequest, ref string jsonResponse, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int JsonGmp3Methods_FP3_Echo(string iface, string jsonRequest, ref string jsonResponse, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int JsonGmp3Methods_FP3_Start(string iface, string jsonRequest, ref string jsonResponse, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int JsonGmp3Methods_FP3_Close(string iface, string jsonRequest, ref string jsonResponse, int timeout);

        /// <summary>
        /// Get interface handle from string interface name
        /// For JSON-based methods that require uint handles
        /// </summary>
        public static uint GetInterfaceHandle(string iface)
        {
            // Try to parse as uint first (direct handle)
            if (uint.TryParse(iface, out uint directHandle))
            {
                return directHandle;
            }

            // For string-based interfaces, we need to create/get a handle
            // This is a simplified approach - in practice, you might need to maintain a mapping
            try
            {
                // Try to create interface and get handle
                int result = CreateInterface(iface);
                if (result == Gmp3Constants.DLL_RETCODE_CREATE_INTERFACE_SUCCESS)
                {
                    // For now, return a default handle - in practice you'd need to track this
                    return 1; // Default handle
                }
            }
            catch
            {
                // Ignore errors
            }

            return 0; // No handle available
        }
    }
}
