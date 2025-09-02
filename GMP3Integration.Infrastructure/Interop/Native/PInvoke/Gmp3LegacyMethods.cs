using System.Runtime.InteropServices;
using GMP3Integration.Infrastructure.Interop.Native.Constants;
using GMP3Integration.Infrastructure.Interop.Native.Structs;

namespace GMP3Integration.Infrastructure.Interop.Native.PInvoke
{
    /// <summary>
    /// GMP3 Legacy P/Invoke methods (UniStd, UniCdecl, etc.)
    /// </summary>
    public static class Gmp3LegacyMethods
    {
        // UniStd Methods
        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public static extern int UniStd_x64_FP3_Start(string iface, ref ulong tranHandle, byte[] uniqueId, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public static extern int UniStd_x64_FP3_Close(string iface, ulong tranHandle, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public static extern int UniStd_x64_FP3_Echo(string iface, ulong tranHandle, ref ST_ECHO echo, int timeout);

        // UniCdecl Methods
        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public static extern int UniCdecl_x64_FP3_Start(string iface, ref ulong tranHandle, byte[] uniqueId, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public static extern int UniCdecl_x64_FP3_Close(string iface, ulong tranHandle, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public static extern int UniCdecl_x64_FP3_Echo(string iface, ulong tranHandle, ref ST_ECHO echo, int timeout);

        // AnsiStd Methods
        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int AnsiStd_x64_FP3_Start(string iface, ref ulong tranHandle, byte[] uniqueId, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int AnsiStd_x64_FP3_Close(string iface, ulong tranHandle, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int AnsiStd_x64_FP3_Echo(string iface, ulong tranHandle, ref ST_ECHO echo, int timeout);

        // AnsiCdecl Methods
        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int AnsiCdecl_x64_FP3_Start(string iface, ref ulong tranHandle, byte[] uniqueId, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int AnsiCdecl_x64_FP3_Close(string iface, ulong tranHandle, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int AnsiCdecl_x64_FP3_Echo(string iface, ulong tranHandle, ref ST_ECHO echo, int timeout);

        // Interface Methods
        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public static extern int Iface_UniStd_x64_CreateInterface(string iface, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public static extern int Iface_UniStd_x64_Close(string iface, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public static extern int Iface_UniStd_x64_Echo(string iface, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public static extern int Iface_UniCdecl_x64_CreateInterface(string iface, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public static extern int Iface_UniCdecl_x64_Close(string iface, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public static extern int Iface_UniCdecl_x64_Echo(string iface, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int Iface_AnsiStd_x64_CreateInterface(string iface, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int Iface_AnsiStd_x64_Close(string iface, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int Iface_AnsiStd_x64_Echo(string iface, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int Iface_AnsiCdecl_x64_CreateInterface(string iface, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int Iface_AnsiCdecl_x64_Close(string iface, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int Iface_AnsiCdecl_x64_Echo(string iface, int timeout);
    }
}
