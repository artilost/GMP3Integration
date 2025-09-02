using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP3Integration.Application.DTOs.TaxRates
{
    public class GetTaxRatesResponse
    {
        public bool Success { get; set; }
        public List<TaxRateDto> Rates { get; set; } = new List<TaxRateDto>();
    }
}
