using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GMP3Integration.Infrastructure.Interop
{
    internal static class NativeExportsResolver
    {
        private static bool _inited;
        private static IntPtr _hDll;

        // ---- Delegates ----
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        private delegate int EchoA_Delegate([MarshalAs(UnmanagedType.LPStr)] string iface, int timeoutMs);
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        private delegate int EchoW_Delegate([MarshalAs(UnmanagedType.LPWStr)] string iface, int timeoutMs);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int EchoH_Delegate(IntPtr hInt, int timeoutMs);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        private delegate int PingA_Delegate([MarshalAs(UnmanagedType.LPStr)] string iface, int timeoutMs);
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        private delegate int PingW_Delegate([MarshalAs(UnmanagedType.LPWStr)] string iface, int timeoutMs);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int PingH_Delegate(IntPtr hInt, int timeoutMs);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        private delegate int InterfaceOpenA_Delegate([MarshalAs(UnmanagedType.LPStr)] string iface, out IntPtr hInt);
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        private delegate int InterfaceOpenW_Delegate([MarshalAs(UnmanagedType.LPWStr)] string iface, out IntPtr hInt);
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        private delegate int OpenA_Delegate([MarshalAs(UnmanagedType.LPStr)] string iface, out IntPtr hInt);
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        private delegate int OpenW_Delegate([MarshalAs(UnmanagedType.LPWStr)] string iface, out IntPtr hInt);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int CloseH_Delegate(IntPtr hInt);

        private static EchoA_Delegate _echoA;
        private static EchoW_Delegate _echoW;
        private static EchoH_Delegate _echoH;

        private static PingA_Delegate _pingA;
        private static PingW_Delegate _pingW;
        private static PingH_Delegate _pingH;

        private static InterfaceOpenA_Delegate _ifaceOpenA;
        private static InterfaceOpenW_Delegate _ifaceOpenW;
        private static OpenA_Delegate _openA;
        private static OpenW_Delegate _openW;

        private static CloseH_Delegate _closeH;

        public static void Init(ILogger log)
        {
            if (_inited) return;
            _inited = true;

            // GMPSmartDLL.dll zaten PATH'e eklenmiş olmalı (serviste yapılıyor).
            _hDll = LoadLibraryW("GMPSmartDLL.dll");
            if (_hDll == IntPtr.Zero)
            {
                log?.LogWarning("GMPSmartDLL.dll LoadLibraryW FAILED");
                return;
            }

            // --- Resolve helpers ---
            T Resolve<T>(string name) where T : class
            {
                var p = GetProcAddress(_hDll, name);
                if (p == IntPtr.Zero) return null;
                return Marshal.GetDelegateForFunctionPointer(p, typeof(T)) as T;
            }

            // ECHO
            _echoA = Resolve<EchoA_Delegate>("FP3_EchoA");
            _echoW = Resolve<EchoW_Delegate>("FP3_EchoW");
            _echoH = Resolve<EchoH_Delegate>("FP3_Echo"); // çoğunlukla HANDLE tabanlı

            // PING
            _pingA = Resolve<PingA_Delegate>("FP3_PingA");
            _pingW = Resolve<PingW_Delegate>("FP3_PingW");
            _pingH = Resolve<PingH_Delegate>("FP3_Ping");

            // OPEN (çeşitli isimler)
            //_ifaceOpenA = Resolve<InterfaceOpenA_Delegate>("FP3_InterfaceOpenA");
            //_ifaceOpenW = Resolve<InterfaceOpenW_Delegate>("FP3_InterfaceOpenW");
            _openA = Resolve<OpenA_Delegate>("FP3_OpenA");
            _openW = Resolve<OpenW_Delegate>("FP3_OpenW");

            // CLOSE
            _closeH = Resolve<CloseH_Delegate>("FP3_Close");

            log?.LogInformation("Native resolve: EchoA={ea} EchoW={ew} EchoH={eh} | PingA={pa} PingW={pw} PingH={ph} | IfaceOpenA={ioa} IfaceOpenW={iow} OpenA={oa} OpenW={ow} | CloseH={ch}",
                _echoA != null, _echoW != null, _echoH != null,
                _pingA != null, _pingW != null, _pingH != null,
                _ifaceOpenA != null, _ifaceOpenW != null, _openA != null, _openW != null,
                _closeH != null);
        }

        public static bool HasEchoByString => _echoA != null || _echoW != null;
        public static bool HasPingByString => _pingA != null || _pingW != null;
        public static bool HasEchoByHandle => _echoH != null;
        public static bool HasPingByHandle => _pingH != null;
        public static bool HasOpenByString => (_ifaceOpenA != null || _ifaceOpenW != null || _openA != null || _openW != null);
        public static bool HasCloseByHandle => _closeH != null;

        public static int EchoByString(string iface, int timeoutMs)
        {
            if (_echoA != null) return _echoA(iface, timeoutMs);
            if (_echoW != null) return _echoW(iface, timeoutMs);
            return Gmp3NativeMethods.DLL_RETCODE_INVALID_INTERFACE; // ulaşmaması lazım
        }

        public static int PingByString(string iface, int timeoutMs)
        {
            if (_pingA != null) return _pingA(iface, timeoutMs);
            if (_pingW != null) return _pingW(iface, timeoutMs);
            return Gmp3NativeMethods.DLL_RETCODE_INVALID_INTERFACE;
        }

        public static int OpenByString(string iface, out IntPtr hInt)
        {
            hInt = IntPtr.Zero;
            if (_ifaceOpenA != null) return _ifaceOpenA(iface, out hInt);
            if (_ifaceOpenW != null) return _ifaceOpenW(iface, out hInt);
            if (_openA != null) return _openA(iface, out hInt);
            if (_openW != null) return _openW(iface, out hInt);
            return Gmp3NativeMethods.DLL_RETCODE_INVALID_INTERFACE;
        }

        public static int EchoByHandle(IntPtr hInt, int timeoutMs)
        {
            if (_echoH == null) return Gmp3NativeMethods.DLL_RETCODE_INVALID_INTERFACE;
            return _echoH(hInt, timeoutMs);
        }

        public static int PingByHandle(IntPtr hInt, int timeoutMs)
        {
            if (_pingH == null) return Gmp3NativeMethods.DLL_RETCODE_INVALID_INTERFACE;
            return _pingH(hInt, timeoutMs);
        }

        public static int CloseHandle(IntPtr hInt)
        {
            if (_closeH == null) return 0; // yoksa sessiz geç
            return _closeH(hInt);
        }

        // --- kernel32 ---
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern IntPtr LoadLibraryW(string lpLibFileName);

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);
    }
}
