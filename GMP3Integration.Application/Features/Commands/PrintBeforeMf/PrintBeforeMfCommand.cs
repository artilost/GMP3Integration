using GMP3Integration.Application.DTOs.PrintBeforeMf;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP3Integration.Application.Features.Commands.PrintBeforeMf
{
    public class PrintBeforeMfCommand :IRequest<PrintBeforeMfResponse>
    {
        public PrintBeforeMfRequest Request { get; private set; }
        public PrintBeforeMfCommand(PrintBeforeMfRequest request) { Request = request; }
    }
}
