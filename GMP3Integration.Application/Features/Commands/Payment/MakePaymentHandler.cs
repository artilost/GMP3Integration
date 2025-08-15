using GMP3Integration.Application.DTOs.Payment;
using GMP3Integration.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GMP3Integration.Application.Features.Commands.Payment
{
    public class MakePaymentHandler : IRequestHandler<MakePaymentCommand, PaymentResponse>
    {
        private readonly IGmp3Service _gmp3Service;
        public MakePaymentHandler(IGmp3Service gmp3Service)
        {
            _gmp3Service = gmp3Service;
        }
        public async Task<PaymentResponse> Handle(MakePaymentCommand request, CancellationToken cancellationToken)
        {
            return await _gmp3Service.MakePaymentAsync(request.Request);
        }
    }
}
