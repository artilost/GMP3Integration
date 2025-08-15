using GMP3Integration.Application.DTOs.PrintMessage;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP3Integration.Application.Features.Commands.PrintMessage
{
    public class PrintMessageCommand : IRequest<PrintMessageResponse>
    {
        public PrintMessageRequest Request { get; private set; }
        public PrintMessageCommand(PrintMessageRequest request) { Request = request; }
    }
}
