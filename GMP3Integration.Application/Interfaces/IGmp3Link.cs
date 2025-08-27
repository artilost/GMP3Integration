using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP3Integration.Application.Interfaces
{
    public interface IGmp3Link
    {
        // rc==0 => OK, aksi hata kodu; okIface null değilse kabul edilen iface
        (int rc, string okIface) Probe(string iface);
    }
}
