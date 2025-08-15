using GMP3Integration.Application.DTOs.PrintMessage;
using GMP3Integration.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GMP3Integration.Application.Features.Commands.PrintMessage
{
    public class PrintMessageHandler : IRequestHandler<PrintMessageCommand, PrintMessageResponse>
    {
        private readonly  IGmp3Service _gmp3Service;
        public PrintMessageHandler(IGmp3Service gmp3Service)
        {
            _gmp3Service = gmp3Service;
        }

        public async Task<PrintMessageResponse> Handle(PrintMessageCommand request, CancellationToken cancellationToken)
        {
            return await _gmp3Service.PrintMessageAsync(request.Request);
        }
    }
}
