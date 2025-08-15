using GMP3Integration.Application.DTOs.CloseTransaction;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP3Integration.Application.Features.Commands.CloseTransaction
{
    public class CloseTransactionCommand : IRequest<CloseTransactionResponse>
    {
        public CloseTransactionRequest Request { get; private set; }
        public CloseTransactionCommand(CloseTransactionRequest request)
        {
            Request = request;
        }
    }
}
