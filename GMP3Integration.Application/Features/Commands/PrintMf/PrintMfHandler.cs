using GMP3Integration.API.Controllers;
using GMP3Integration.Application.DTOs.PrintMf;
using GMP3Integration.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GMP3Integration.Application.Features.Commands.PrintMf
{
    public class PrintMfHandler : IRequestHandler<PrintMfCommand, PrintMfResponse>
    {
        private readonly IGmp3Service _gmp3Service;
        public PrintMfHandler(IGmp3Service gmp3Service)
        {
            _gmp3Service = gmp3Service;
        }
        public Task<PrintMfResponse> Handle(PrintMfCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
