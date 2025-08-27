using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP3Integration.Infrastructure.Services
{
    internal static class InterfaceHelper
    {
        /// <summary>
        /// Verilen iface string için olası tüm varyantları üretir.
        /// Örn girdiler:
        ///   "TCP:192.168.137.99:7500"
        ///   "TCP:192.168.137.99,7500"
        ///   "LAN:192.168.137.99:7500"
        ///   "TCPIP:192.168.137.99:7500"
        /// </summary>
        internal static List<string> BuildVariants(string iface)
        {
            var list = new List<string>();
            if (string.IsNullOrWhiteSpace(iface)) return list;

            iface = iface.Trim();
            list.Add(iface); // orijinal

            // host:port yakala (TCP:IP:PORT veya TCP:IP,PORT ya da IP:PORT / IP,PORT)
            string host; int port;
            if (!TrySplitHostPort(iface, out host, out port))
                return Dedup(list);

            // 1) TCP:IP:PORT
            list.Add("TCP:" + host + ":" + port);
            // 2) TCP:IP,PORT
            list.Add("TCP:" + host + "," + port);
            // 3) LAN:IP:PORT
            list.Add("LAN:" + host + ":" + port);
            // 4) IP:PORT (önek yok)
            list.Add(host + ":" + port);
            // 5) IP,PORT (önek yok)
            list.Add(host + "," + port);
            // 6) TCPIP:IP:PORT

            list.Add("TCPIP:" + host + ":" + port);
            list.Add($"ETHERNET;IP={host};PORT={port}");
            list.Add($"TCPIP;IP={host};PORT={port}");
            list.Add($"LAN;IP={host};PORT={port}");

            list.Add($"ETHERNET:{host},{port}");
            list.Add($"ETH:{host},{port}");
            list.Add($"IP:{host},{port}");
            list.Add($"TCPIP,{host},{port}");

            list.Add($"{host}");
            list.Add($"TCP:{host}");
            list.Add($"ETHERNET:{host}");

            list.Add("ETHERNET");
            list.Add("TCPIP");
            list.Add("LAN");

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
