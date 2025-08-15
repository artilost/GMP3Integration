using GMP3Integration.Application.DTOs.ITransactionWorkflowService;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP3Integration.Application.Features.Commands.CompleteSale
{
    public class CompleteSaleCommand : IRequest<CompleteSaleResponse>
    {
        public CompleteSaleRequest Request { get; private set; }

        public CompleteSaleCommand(CompleteSaleRequest request)
        {
            Request = request;
        }
    }
}
