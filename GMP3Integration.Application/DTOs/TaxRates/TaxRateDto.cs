using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP3Integration.Application.DTOs.TaxRates
{
    public class TaxRateDto
    {
        public int Index { get; set; }      // cihazdaki vergi index’i 
        public string TaxCode { get; set; } // cihazdaki code 
        public decimal Rate { get; set; }   // oran (örn: 8.0)
    }
}
