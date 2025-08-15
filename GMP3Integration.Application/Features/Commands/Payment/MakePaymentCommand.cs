using GMP3Integration.Application.DTOs.Payment;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP3Integration.Application.Features.Commands.Payment
{
    public class MakePaymentCommand : IRequest<PaymentResponse>
    {
        public PaymentRequest Request { get; private set; }
        public MakePaymentCommand(PaymentRequest request)
        {
            Request = request;
        }
    }
}
