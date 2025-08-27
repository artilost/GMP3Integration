using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GMP3Integration.Infrastructure.Interop
{
    internal static class DynLoader
    {
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool FreeLibrary(IntPtr hModule);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true)]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        internal static bool TryLoad(string dll, out IntPtr handle)
        {
            handle = LoadLibrary(dll);
            return handle != IntPtr.Zero;
        }

        internal static bool TryGetExport(IntPtr handle, string name)
        {
            return handle != IntPtr.Zero && GetProcAddress(handle, name) != IntPtr.Zero;
        }

        internal static void Free(IntPtr handle)
        {
            if (handle != IntPtr.Zero)
            {
                try { FreeLibrary(handle); } catch { /* yut */ }
            }
        }
    }
}
