using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GMP3Integration.Application.DTOs.PrintMf;
using MediatR;

namespace GMP3Integration.API.Controllers
{
    public class PrintMfCommand : IRequest<PrintMfResponse>
    {
        public PrintMfRequest Request { get; private set; }
        public PrintMfCommand(PrintMfRequest request)
        {
            Request = request;
        }
    }
}
