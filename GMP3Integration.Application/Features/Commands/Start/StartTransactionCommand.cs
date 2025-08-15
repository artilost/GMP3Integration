using GMP3Integration.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP3Integration.Application.Features.Commands.Start
{
    public class StartTransactionCommand : IRequest<StartTransactionResponse>
    {
        public StartTransactionRequest Request { get; private set; }
        public StartTransactionCommand(StartTransactionRequest request)
        {
            Request = request;
        }
    }
}
