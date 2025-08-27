using GMP3Integration.Application.DTOs;
using GMP3Integration.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GMP3Integration.Application.Features.Commands.Start
{
    // Application, Infrastructure'a referans ALMAZ.
    // Pairing/Echo/Start mantığının tamamı IGmp3Service içinde (Infrastructure) tutulur.
    public sealed class StartTransactionHandler : IRequestHandler<StartTransactionCommand, StartTransactionResponse>
    {
        private readonly IGmp3Service _gmp3;
        private readonly IConfiguration _cfg;
        private readonly ILogger<StartTransactionHandler> _log;

        public StartTransactionHandler(IGmp3Service gmp3, IConfiguration cfg, ILogger<StartTransactionHandler> log)
        { _gmp3 = gmp3; _cfg = cfg; _log = log; }

        public async Task<StartTransactionResponse> Handle(StartTransactionCommand request, CancellationToken cancellationToken)
        {
            var iface = string.IsNullOrWhiteSpace(request?.CurrentInterface)
             ? _cfg["Gmp3:CurrentInterface"]
             : request.CurrentInterface.Trim();

            // Eski davranış gibi: asla throw etme; servise pasla.
            var resp = await _gmp3.StartTransactionAsync(new StartTransactionRequest { CurrentInterface = iface });
            return resp; 
        }
       
    }
}
