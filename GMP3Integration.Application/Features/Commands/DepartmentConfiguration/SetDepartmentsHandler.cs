using GMP3Integration.Application.DTOs.DepertmenConfiguration;
using GMP3Integration.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GMP3Integration.Application.Features.Commands.DepartmentConfiguration
{
    public class SetDepartmentsHandler : IRequestHandler<SetDepartmentsCommand, SetDepartmentsResponse>
    {
        private readonly IGmp3Service _gmp3Service;
        public SetDepartmentsHandler(IGmp3Service gmp3Service)
        {
            _gmp3Service = gmp3Service;
        }
        public async Task<SetDepartmentsResponse> Handle(SetDepartmentsCommand request, CancellationToken cancellationToken)
        {
            return await _gmp3Service.SetDepartmentsAsync(request.Request);
        }
    }
}
