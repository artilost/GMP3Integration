using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using Microsoft.Extensions.Logging;

namespace GMP3Integration.Infrastructure.Services
{
    internal static class InterfaceHelper
    {
        private static ILogger _logger;

        public static void SetLogger(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// XML dosyasından interface bilgilerini okur ve varyantları üretir.
        /// </summary>
        public static List<string> BuildVariantsFromXml()
        {
            var interfaces = ReadInterfacesFromXml();
            var variants = new List<string>();

            foreach (var (id, ip, port) in interfaces)
            {
                _logger?.LogInformation("Interface bilgileri eklendi: ID={id}, IP={ip}, Port={port}", id, ip, port);
                
                // 1. Interface ID'sini EN ÖNCE dene (emulator'da COM1)
                variants.Add(id); // COM1
                variants.Add(id.ToUpper()); // COM1
                variants.Add(id.ToLower()); // com1
                
                // 2. COM port kontrolü - COM portları ikinci sırada dene
                if (ip.StartsWith("COM", StringComparison.OrdinalIgnoreCase))
                {
                    variants.Add(ip); // COM5
                    variants.Add(ip.ToUpper()); // COM5
                    variants.Add(ip.ToLower()); // com5
                    variants.Add($"\\\\.\\{ip}"); // \\.\COM5 (XML'deki PortName)
                    variants.Add($"\\\\.\\{ip.ToUpper()}"); // \\.\COM5
                    variants.Add($"\\\\.\\{ip.ToLower()}"); // \\.\com5
                    
                    // 3. COM port + ID kombinasyonları
                    variants.Add($"{id}:{ip}"); // COM1:COM5
                    variants.Add($"{id};{ip}"); // COM1;COM5
                    variants.Add($"{id}.{ip}"); // COM1.COM5
                }
                
                // 4. TCP/IP interface'ler için formatları kullan (en son)
                variants.Add($"{id}:{ip}:{port}");
                variants.Add($"{id};IP={ip};PORT={port}");
                variants.Add($"{ip}:{port}");
                variants.Add(ip);
                variants.Add($"TCP:{ip}:{port}");
                variants.Add($"LAN:{ip}:{port}");
                variants.Add($"ETHERNET:{ip}:{port}");
                variants.Add($"TCPIP;IP={ip};PORT={port}");
                variants.Add($"ETHERNET;IP={ip};PORT={port}");
            }

            var variantString = string.Join(" | ", variants);
            _logger?.LogInformation("IFACE VARIANTS ({count}): {variants}", variants.Count, variantString);
            
            return variants;
        }

        /// <summary>
        /// XML dosyasından interface bilgilerini okur.
        /// </summary>
        private static List<(string ID, string IP, string Port)> ReadInterfacesFromXml()
        {
            var interfaces = new List<(string ID, string IP, string Port)>();
            
            try
            {
                var xmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "native", "win-x64", "GMP.XML");
                _logger?.LogInformation("XML okuma başlıyor: {xmlPath}", xmlPath);
                
                if (!File.Exists(xmlPath))
                {
                    _logger?.LogWarning("XML dosyası bulunamadı: {xmlPath}", xmlPath);
                    return interfaces;
                }

                var doc = new XmlDocument();
                var settings = new XmlReaderSettings
                {
                    DtdProcessing = DtdProcessing.Ignore,
                    CheckCharacters = false,
                    IgnoreWhitespace = true,
                    ValidationType = ValidationType.None
                };
                
                // XML dosyasını UTF-8 olarak oku, encoding declaration'ı ignore et
                var xmlContent = File.ReadAllText(xmlPath, Encoding.UTF8);
                // XML declaration'ı kaldır veya UTF-8 olarak değiştir
                xmlContent = xmlContent.Replace("encoding=\"iso-8859-9\"", "encoding=\"UTF-8\"");
                if (xmlContent.Contains("<?xml"))
                {
                    var lines = xmlContent.Split('\n');
                    if (lines[0].Contains("encoding="))
                    {
                        lines[0] = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
                        xmlContent = string.Join("\n", lines);
                    }
                }
                
                using (var stringReader = new StringReader(xmlContent))
                using (var reader = XmlReader.Create(stringReader, settings))
                {
                    doc.Load(reader);
                }
                _logger.LogInformation("XML dosyası yüklendi");

                var interfaceNodes = doc.SelectNodes("//INTERFACE");
                _logger.LogInformation("Interface node sayısı: {count}", interfaceNodes?.Count ?? 0);
                
                if (interfaceNodes != null)
                {
                    foreach (XmlNode node in interfaceNodes)
                    {
                        var id = node.Attributes?["ID"]?.Value;
                        var ip = node.SelectSingleNode("IP")?.InnerText;
                        var port = node.SelectSingleNode("Port")?.InnerText;
                        var portName = node.SelectSingleNode("PortName")?.InnerText;
                        
                        _logger.LogInformation("Interface bulundu: ID={id}, IP={ip}, Port={port}, PortName={portName}", id, ip, port, portName);
                        
                        if (!string.IsNullOrEmpty(id))
                        {
                            // TCP/IP interface
                            if (!string.IsNullOrEmpty(ip) && !string.IsNullOrEmpty(port))
                            {
                                interfaces.Add((id, ip, port));
                                _logger.LogInformation("TCP/IP Interface bilgileri eklendi: ID={id}, IP={ip}, Port={port}", id, ip, port);
                            }
                            
                            // COM interface
                            if (!string.IsNullOrEmpty(portName) && portName.StartsWith("\\\\.\\COM"))
                            {
                                var comPort = portName.Replace("\\\\.\\", "");
                                interfaces.Add((id, comPort, "0")); // COM port için port=0
                                _logger.LogInformation("COM Interface bilgileri eklendi: ID={id}, PortName={portName}, COM={comPort}", id, portName, comPort);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "XML okuma hatası");
            }
            
            return interfaces;
        }

        /// <summary>
        /// Verilen iface string için olası tüm varyantları üretir.
        /// PAX A910SF için TCP/IP ve COM formatlarını destekler.
        /// </summary>
        internal static List<string> BuildVariants(string iface)
        {
            var list = new List<string>();
            if (string.IsNullOrWhiteSpace(iface)) return list;

            iface = iface.Trim();
            list.Add(iface); // orijinal

            // COM port kontrolü (COM1, COM2, etc.)
            if (iface.StartsWith("COM", StringComparison.OrdinalIgnoreCase))
            {
                // COM port formatları
                list.Add(iface.ToUpper()); // COM1, COM2, etc.
                list.Add(iface.ToLower()); // com1, com2, etc.
                return Dedup(list);
            }

            // TCP/IP host:port yakala
            string host; int port;
            if (!TrySplitHostPort(iface, out host, out port))
                return Dedup(list);

            // PAX A910SF için TCP/IP formatları
            // 1) TCP:IP:PORT (standart)
            list.Add("TCP:" + host + ":" + port);
            // 2) LAN:IP:PORT (alternatif)
            list.Add("LAN:" + host + ":" + port);
            // 3) IP:PORT (önek yok)
            list.Add(host + ":" + port);
            // 4) ETHERNET;IP=host;PORT=port (XML format)
            list.Add($"ETHERNET;IP={host};PORT={port}");
            // 5) TCPIP;IP=host;PORT=port (alternatif XML format)
            list.Add($"TCPIP;IP={host};PORT={port}");

            return Dedup(list);
        }

        private static bool TrySplitHostPort(string s, out string host, out int port)
        {
            host = null; port = 0;
            if (string.IsNullOrWhiteSpace(s)) return false;

            // Önekleri at (TCP:, LAN:, TCPIP:)
            int idx = s.IndexOf(':');
            string rest = idx > 0 ? s.Substring(idx + 1) : s;
            rest = rest.Replace(" ", "");

            int sep = rest.LastIndexOf(':');
            if (sep < 0) sep = rest.LastIndexOf(',');
            if (sep < 0) return false;

            var h = rest.Substring(0, sep);
            var p = rest.Substring(sep + 1);
            int prt;
            if (!int.TryParse(p, out prt)) return false;

            host = h; port = prt; return true;
        }

        private static List<string> Dedup(List<string> input)
        {
            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var outp = new List<string>();
            for (int i = 0; i < input.Count; i++)
            {
                var v = input[i];
                if (string.IsNullOrWhiteSpace(v)) continue;
                if (seen.Add(v)) outp.Add(v);
            }
            return outp;
        }
    }
}
