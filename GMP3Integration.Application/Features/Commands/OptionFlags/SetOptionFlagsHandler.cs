using GMP3Integration.Application.DTOs.OptionFlags;
using GMP3Integration.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GMP3Integration.Application.Features.Commands.OptionFlags
{
    public class SetOptionFlagsHandler : IRequestHandler<SetOptionFlagsCommand, SetOptionFlagsResponse>
    {
        private readonly IGmp3Service _gmp3Service;
        public SetOptionFlagsHandler(IGmp3Service gmp3Service)
        {
            _gmp3Service = gmp3Service;
        }
        public async Task<SetOptionFlagsResponse> Handle(SetOptionFlagsCommand request, CancellationToken cancellationToken)
            => await _gmp3Service.SetOptionFlagsAsync(request.Request);
        
    }
}
