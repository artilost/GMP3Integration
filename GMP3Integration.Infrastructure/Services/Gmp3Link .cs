using GMP3Integration.Application.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP3Integration.Infrastructure.Services
{
    public sealed class Gmp3Link : IGmp3Link
    {
        private readonly ILogger<Gmp3Link> _log;
        public Gmp3Link(ILogger<Gmp3Link> log) => _log = log;

        public (int rc, string okIface) Probe(string iface)
        {
            var variants = InterfaceHelper.BuildVariants(iface);

            int lastRc = 0xF034;
            foreach (var v in variants)
            {
                _log.LogInformation(">> TRY iface='{iface}'", v);
                var rc = Gmp3EchoRunner.TryAll(v, _log);
                if (rc == 0) return (0, v);     // başarı
                if (rc != 0xF034) return (rc, null); // farklı hata → dur
                lastRc = rc; // 0xF034 ise sonraki varyant
            }
            return (lastRc, null);
        }
    }
}
