using GMP3Integration.Application.DTOs.ITransactionWorkflowService;
using GMP3Integration.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GMP3Integration.Application.Features.Commands.CompleteSale
{
    public class CompleteSaleHandler : IRequestHandler<CompleteSaleCommand, CompleteSaleResponse>
    {
        private readonly ITransactionWorkflowService _workflow;
        private readonly ILogger<CompleteSaleHandler> _logger;

        public CompleteSaleHandler(ITransactionWorkflowService workflow, ILogger<CompleteSaleHandler> logger)
        {
            _workflow = workflow;
            _logger = logger;
        }
        public async Task<CompleteSaleResponse> Handle(CompleteSaleCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("CompleteSaleCommand received.");
            var response = await _workflow.ExecuteCompleteSaleAsync(command.Request);
            _logger.LogInformation("CompleteSaleCommand finished. Handle={Handle}", response.TransactionHandle);
            return response;
        }
    }
}
