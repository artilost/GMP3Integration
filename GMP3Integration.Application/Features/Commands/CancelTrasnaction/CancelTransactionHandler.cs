using GMP3Integration.Application.DTOs.CancelTansaction;
using GMP3Integration.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GMP3Integration.Application.Features.Commands.CancelTrasnaction{
    public class CancelTransactionHandler : IRequestHandler<CancelTransactionCommand, CancelTransactionResponse>
    {
        private readonly IGmp3Service _gmp3Service;
        public CancelTransactionHandler(IGmp3Service gmp3Service) { _gmp3Service = gmp3Service; }

        public async Task<CancelTransactionResponse> Handle(CancelTransactionCommand request, CancellationToken cancellationToken)
            => await _gmp3Service.CancelTransactionAsync(request.Request);
    }
}
