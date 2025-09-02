using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GMP3Integration.Infrastructure.Interop
{
    public static class NativeAudit
    {
        public static void Run(ILogger logger)
        {
            try
            {
                var asms = AppDomain.CurrentDomain.GetAssemblies();
                foreach (var asm in asms)
                {
                    Type[] types;
                    try { types = asm.GetTypes(); } catch (ReflectionTypeLoadException ex) { types = ex.Types.Where(t => t != null).ToArray(); }

                    foreach (var t in types)
                    {
                        if (t == null) continue;
                        foreach (var m in t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance))
                        {
                            var attr = (DllImportAttribute[])m.GetCustomAttributes(typeof(DllImportAttribute), inherit: false);
                            if (attr == null || attr.Length == 0) continue;

                            foreach (var a in attr)
                            {
                                var ep = (a.EntryPoint ?? "").Trim();
                                if (string.Equals(ep, "FP3_Close", StringComparison.OrdinalIgnoreCase) ||
                                    string.Equals(ep, "FP3_INTERFACE_CLOSE", StringComparison.OrdinalIgnoreCase) ||
                                    string.Equals(ep, "FP3_InterfaceClose", StringComparison.OrdinalIgnoreCase))
                                {
                                    logger.LogWarning("P/Invoke CLOSE found: {DeclaringType}.{Method}  (EntryPoint={EntryPoint}, CharSet={CharSet}, CallConv={Conv}, Assembly={Asm})",
                                        m.DeclaringType?.FullName, m.Name, ep, a.CharSet, a.CallingConvention, asm.FullName);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "NativeAudit.Run failed.");
            }
        }
    }
}
