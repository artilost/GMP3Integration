using GMP3Integration.Application.DTOs.CloseTransaction;
using GMP3Integration.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GMP3Integration.Application.Features.Commands.CloseTransaction
{
    public class CloseTransactionHandler : IRequestHandler<CloseTransactionCommand, CloseTransactionResponse>
    {
        private readonly IGmp3Service _gmp3Service;
        public CloseTransactionHandler(IGmp3Service gmp3Service)
        {
            _gmp3Service = gmp3Service;
        }
        public Task<CloseTransactionResponse> Handle(CloseTransactionCommand request, CancellationToken cancellationToken)
        {
            return _gmp3Service.CloseTransactionAsync(request.Request);
        }
    }
}
