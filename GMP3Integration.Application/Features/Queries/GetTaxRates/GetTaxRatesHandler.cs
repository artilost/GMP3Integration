using GMP3Integration.Application.DTOs.TaxRates;
using GMP3Integration.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GMP3Integration.Application.Features.Queries.GetTaxRates
{
    public class GetTaxRatesHandler : IRequestHandler<GetTaxRatesQuery, GetTaxRatesResponse>
    {
        private readonly IGmp3Service _gmp3;

        public GetTaxRatesHandler(IGmp3Service gmp3)
        {
            _gmp3 = gmp3;
        }

        public async Task<GetTaxRatesResponse> Handle(GetTaxRatesQuery request, CancellationToken cancellationToken)
        {
            return await _gmp3.GetTaxRatesAsync();
        }
    }
}
