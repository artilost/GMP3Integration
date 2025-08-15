using GMP3Integration.Application.DTOs.CancelTansaction;
using GMP3Integration.Application.DTOs.CanselTransaction;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP3Integration.Application.Features.Commands.CancelTrasnaction
{
    public class CancelTransactionCommand : IRequest<CancelTransactionResponse>
    {
        public CancelTransactionRequest Request { get; private set; }
        public CancelTransactionCommand(CancelTransactionRequest request) { Request = request; }
    }
}
