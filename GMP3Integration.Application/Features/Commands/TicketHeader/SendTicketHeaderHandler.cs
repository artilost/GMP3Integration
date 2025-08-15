using GMP3Integration.Application.DTOs.TicketHeader;
using GMP3Integration.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GMP3Integration.Application.Features.Commands.TicketHeader
{
    public class SendTicketHeaderHandler : IRequestHandler<SendTicketHeaderCommand, SendTicketHeaderResponse>
    {
        private readonly IGmp3Service _gmp3Service;
        public SendTicketHeaderHandler(IGmp3Service gmp3Service)
        {
            _gmp3Service = gmp3Service;
        }
        public async Task<SendTicketHeaderResponse> Handle(SendTicketHeaderCommand request, CancellationToken cancellationToken)
            => await _gmp3Service.SendTicketHeaderAsync(request.Request);
        
    }
}
