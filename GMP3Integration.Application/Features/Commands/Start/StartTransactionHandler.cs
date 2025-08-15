using GMP3Integration.Application.DTOs;
using GMP3Integration.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GMP3Integration.Application.Features.Commands.Start
{
    public class StartTransactionHandler :IRequestHandler<StartTransactionCommand, StartTransactionResponse>
    {
        private readonly IGmp3Service _gmp3Service;
        public StartTransactionHandler(IGmp3Service gmp3Service)
        {
            _gmp3Service = gmp3Service;
        }
        public async Task<StartTransactionResponse> Handle(StartTransactionCommand request, CancellationToken cancellationToken)
            => await _gmp3Service.StartTransactionAsync(request.Request);
    }
}
