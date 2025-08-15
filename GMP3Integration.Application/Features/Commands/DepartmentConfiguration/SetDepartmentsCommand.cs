using GMP3Integration.Application.DTOs.DepertmenConfiguration;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP3Integration.Application.Features.Commands.DepartmentConfiguration
{
    public class SetDepartmentsCommand : IRequest<SetDepartmentsResponse>
    {
        public SetDepartmentsRequest Request { get; private set; }
        public SetDepartmentsCommand(SetDepartmentsRequest request) { Request = request; }
    }
}
