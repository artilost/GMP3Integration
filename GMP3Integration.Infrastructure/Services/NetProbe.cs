using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GMP3Integration.Infrastructure.Services
{
    internal static class NetProbe
    {
        internal static bool TryParseIface(string iface, out string host, out int port)
        {
            host = null; port = 0;
            if (string.IsNullOrWhiteSpace(iface)) return false;

            int idx = iface.IndexOf(':');
            string rest = idx > 0 ? iface.Substring(idx + 1) : iface;
            rest = rest.Replace(" ", string.Empty);

            int sep = rest.LastIndexOf(':');
            if (sep < 0) sep = rest.LastIndexOf(',');
            if (sep < 0) return false;

            string ip = rest.Substring(0, sep);
            string p = rest.Substring(sep + 1);
            int prt;
            if (!int.TryParse(p, out prt)) return false;

            host = ip;
            port = prt;
            return true;
        }

        internal static bool CanConnect(string host, int port, int timeoutMs, out string err)
        {
            err = null;
            try
            {
                using (var client = new TcpClient())
                {
                    var ar = client.BeginConnect(host, port, null, null);
                    if (!ar.AsyncWaitHandle.WaitOne(TimeSpan.FromMilliseconds(timeoutMs)))
                    {
                        err = "TCP timeout";
                        return false;
                    }
                    client.EndConnect(ar);
                    return true;
                }
            }
            catch (Exception ex)
            {
                err = ex.GetType().Name + ": " + ex.Message;
                return false;
            }
        }
    }
}
