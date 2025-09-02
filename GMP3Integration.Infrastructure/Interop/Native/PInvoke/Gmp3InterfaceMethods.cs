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
        public static extern int CreateInterface(string iface);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "FP3_Close")]
        public static extern int Close(string iface, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "FP3_Echo")]
        public static extern int Echo(string iface);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "FP3_Ping")]
        public static extern int Ping(string iface);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "FP3_Busy")]
        public static extern int Busy(string iface, int timeout);

        // Pairing Methods
        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "FP3_StartPairingInit")]
        public static extern int StartPairingInit(string iface, ref ST_GMP_PAIR pairing, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "FP3_StartPairingApprove")]
        public static extern int StartPairingApprove(string iface, ref ST_GMP_PAIR_RESP pairingResp, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "FP3_StartPairingInitWithPairing")]
        public static extern int StartPairingInitWithPairing(string iface, ref ST_GMP_PAIR pairing, ref ST_GMP_PAIR_RESP pairingResp, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "FP3_StartPairingInit_All")]
        public static extern int StartPairingInit_All(string iface, ref ST_GMP_PAIR pairing, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "FP3_StartPairingInitWithPairing_All")]
        public static extern int StartPairingInitWithPairing_All(string iface, ref ST_GMP_PAIR pairing, ref ST_GMP_PAIR_RESP pairingResp, int timeout);

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
    }
}
