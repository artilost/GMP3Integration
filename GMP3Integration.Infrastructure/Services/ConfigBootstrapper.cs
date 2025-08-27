using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP3Integration.Infrastructure.Services
{
    internal class ConfigBootstrapper
    {
        // DLL'in beklediği muhtemel dosya adları
        private static readonly string[] XmlNames = { "GMP.XML", "GMP.xml", "GMPSmartDLL.xml", "GMPConfig.xml" };

        internal static void EnsureXmlAliases(ILogger log, string nativeDir)
        {
            if (string.IsNullOrWhiteSpace(nativeDir) || !Directory.Exists(nativeDir)) return;

            // Çalışma klasörünü DLL klasörüne sabitle (bazı sürümlerde zorunlu)
            try
            {
                Directory.SetCurrentDirectory(nativeDir);
                log.LogInformation("CurrentDirectory forced to native: {dir}", nativeDir);
            }
            catch (Exception ex)
            {
                log.LogWarning(ex, "SetCurrentDirectory failed");
            }

            // Var olan tek bir kaynak dosyayı, eksik isimlere kopyala
            string src = null;
            foreach (var n in XmlNames)
            {
                var p = Path.Combine(nativeDir, n);
                if (File.Exists(p)) { src = p; break; }
            }

            if (src == null)
            {
                log.LogWarning("No GMP XML found in {dir}. Expected one of: {names}", nativeDir, string.Join(", ", XmlNames));
                return;
            }

            foreach (var n in XmlNames)
            {
                var dst = Path.Combine(nativeDir, n);
                if (!File.Exists(dst))
                {
                    try { File.Copy(src, dst, overwrite: false); log.LogInformation("XML alias created: {dst}", dst); }
                    catch (IOException) { /* eşzamanlı deneme */ }
                    catch (Exception ex) { log.LogWarning(ex, "XML alias create failed: {dst}", dst); }
                }
            }
        }
    }
}
