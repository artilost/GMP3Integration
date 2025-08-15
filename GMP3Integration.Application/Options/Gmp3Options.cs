using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP3Integration.Application.Options
{
    public class Gmp3Options
    {
        public string CurrentInterface { get; set; }
        public string DllPath { get; set; }
        public int? SimulateLatencyMs { get; set; }
    }
}
