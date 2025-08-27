using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP3Integration.Infrastructure.Interop
{
    internal static class InteropDiagnostics
    {
        private static bool _loggedOnce;
        private static bool? _hasIfaceOpen;
        private static bool? _hasEcho;

        private static readonly string[] EchoExports = { "FP3_Echo", "FP3_ECHO" };

        internal static void DetectAndLogExports(ILogger log)
        {
            if (_loggedOnce) return;
            _loggedOnce = true;

            IntPtr h;
            bool ok = DynLoader.TryLoad("GMPSmartDLL.dll", out h);
            log.LogInformation("GMPSmartDLL load: {ok}", ok);
            if (!ok)
            {
                log.LogError("GMPSmartDLL.dll yüklenemedi (PATH/native dizini).");
                return;
            }

            bool anyEcho = false;
            for (int i = 0; i < EchoExports.Length; i++)
            {
                bool exist = DynLoader.TryGetExport(h, EchoExports[i]);
                log.LogInformation("Export exists? {name} = {exist}", EchoExports[i], exist);
                anyEcho |= exist;
            }
            _hasEcho = anyEcho;

            DynLoader.Free(h);
        }

        internal static bool HasEchoExports()
        {
            if (_hasEcho.HasValue) return _hasEcho.Value;
            IntPtr h;
            if (!DynLoader.TryLoad("GMPSmartDLL.dll", out h)) { _hasEcho = false; return false; }
            bool any = false;
            for (int i = 0; i < EchoExports.Length; i++)
                any |= DynLoader.TryGetExport(h, EchoExports[i]);
            DynLoader.Free(h);
            _hasEcho = any;
            return any;
        }
    }
}
