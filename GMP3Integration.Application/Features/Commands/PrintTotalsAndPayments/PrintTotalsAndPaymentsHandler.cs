using GMP3Integration.Application.DTOs.PrintTotalsAndPayments;
using GMP3Integration.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GMP3Integration.Application.Features.Commands.PrintTotalsAndPayments
{
    public class PrintTotalsAndPaymentsHandler : IRequestHandler<PrintTotalsAndPaymentsCommand, PrintTotalsAndPaymentsResponse>
    {
        private readonly IGmp3Service _gmp3Service;
        public PrintTotalsAndPaymentsHandler(IGmp3Service gmp3Service)
        {
            _gmp3Service = gmp3Service;
        }
        public async Task<PrintTotalsAndPaymentsResponse> Handle(PrintTotalsAndPaymentsCommand request,CancellationToken cancellationToken)
            => await _gmp3Service.PrintTotalsAndPaymentsAsync(request.Request);
    }
}
