using GMP3Integration.Application.DTOs.CloseTransaction;
using GMP3Integration.Application.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace GMP3Integration.Application.Features.Commands.CloseTransaction
{
    public class CloseTransactionCommandHandler : IRequestHandler<CloseTransactionCommand, CloseTransactionResponse>
    {
        private readonly IGmp3Service _gmp3Service;

        public CloseTransactionCommandHandler(IGmp3Service gmp3Service)
        {
            _gmp3Service = gmp3Service;
        }

        public async Task<CloseTransactionResponse> Handle(CloseTransactionCommand request, CancellationToken cancellationToken)
        {
            return await _gmp3Service.CloseTransactionAsync(request.Request);
        }
    }
}
