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
        public static extern int CreateInterface(string iface, ref uint handle);  // Handle döndürüyor!

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "FP3_Close")]
        public static extern int Close(string iface, int timeout);

        // EMULATOR PATTERN: Handle-based Echo!
        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "FP3_Echo")]
        public static extern int Echo(uint handle, ref ST_ECHO pStEcho, int TimeoutInMiliseconds);  // Handle-based!
        
        // LEGACY: String-based fallback
        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "FP3_Echo")]
        public static extern int Echo_StringBased(string iface, ref ST_ECHO pStEcho, int TimeoutInMiliseconds);  // String-based!

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

        // Pairing Methods - String-based (ECHO GİBİ DENEME!)
        // EMULATOR PATTERN: Handle-based!
        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "FP3_StartPairingInit")]
        public static extern int StartPairingInit(uint handle, ref ST_GMP_PAIR pairing, ref ST_GMP_PAIR_RESP pairingResp);
        
        // LEGACY: String-based fallback
        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "FP3_StartPairingInit")]
        public static extern int StartPairingInit_StringBased(string iface, ref ST_GMP_PAIR pairing);  // String-based!
        
        // ALTERNATIVE: Farklı calling convention dene
        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "FP3_StartPairingInit")]
        public static extern int StartPairingInit_StdCall(string iface, ref ST_GMP_PAIR pairing);
        
        // HANDLE-BASED: uint handle ile pairing (emulator style)
        // OLD STYLE (0xF032 veriyor):
        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "GMP_StartPairingInit")]
        public static extern int StartPairingInit_Handle_Old(uint hInt, ref ST_GMP_PAIR pairing, ref ST_GMP_PAIR_RESP pairingResp, int timeout);
        
        // NEW STYLE 1: Response parameter olmadan (3 parametre)
        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "GMP_StartPairingInit")]
        public static extern int StartPairingInit_Handle_NewStyle1(uint hInt, ref ST_GMP_PAIR pairing, int timeout);
        
        // NEW STYLE 2: Struct by value
        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "GMP_StartPairingInit")]
        public static extern int StartPairingInit_Handle_NewStyle2(uint hInt, ST_GMP_PAIR pairing, ref ST_GMP_PAIR_RESP pairingResp, int timeout);
        
        // ESKİ İSİM BACKWARD COMPATIBILITY İÇİN
        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "GMP_StartPairingInit")]
        public static extern int StartPairingInit_Handle(uint hInt, ref ST_GMP_PAIR pairing, ref ST_GMP_PAIR_RESP pairingResp, int timeout);
        
        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "GMP_StartPairingInit")]
        public static extern int StartPairingInit_Handle_StdCall(uint hInt, ref ST_GMP_PAIR pairing, ref ST_GMP_PAIR_RESP pairingResp, int timeout);

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
        /// IMPROVED: Proper handle generation for pairing
        /// </summary>
        public static uint GetInterfaceHandle(string iface)
        {
            // Try to parse as uint first (direct handle)
            if (uint.TryParse(iface, out uint directHandle))
            {
                return directHandle;
            }

            // EMULATOR PATTERN: Gerçek handle'ı kullan!
            // Bu method artık sadece fallback için - gerçek handle CreateInterface'den gelir
            try
            {
                // CreateInterface çağrılmış olmalı ve handle kaydedilmiş olmalı
                // Eğer kaydedilmemişse, interface string'den deterministic hash üret
                uint hash = (uint)iface.GetHashCode();
                if (hash == 0) hash = 1; // Ensure non-zero handle
                return hash & 0x7FFFFFFF; // Ensure positive
            }
            catch
            {
                // If anything fails, still try to generate handle
                return 1; // Fallback to handle 1
            }
        }
    }
}
