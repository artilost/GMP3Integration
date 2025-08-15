using GMP3Integration.Application.DTOs.PrintBeforeMf;
using GMP3Integration.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GMP3Integration.Application.Features.Commands.PrintBeforeMf
{
    public class PrintBeforeMfHandler : IRequestHandler<PrintBeforeMfCommand, PrintBeforeMfResponse>
    {
        private readonly IGmp3Service _gmp3Service;
        public PrintBeforeMfHandler(IGmp3Service gmp3Service)
        {
            _gmp3Service = gmp3Service;
        }
        public async Task<PrintBeforeMfResponse> Handle(PrintBeforeMfCommand request, CancellationToken cancellationToken)
        {
            return await _gmp3Service.PrintBeforeMfAsync(request.Request);
        }
    }
}
