using GMP3Integration.Application.DTOs.TicketHeader;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP3Integration.Application.Features.Commands.TicketHeader
{
    public class SendTicketHeaderCommand : IRequest<SendTicketHeaderResponse>
    {
        public SendTicketHeaderRequest Request { get; private set; }
        public SendTicketHeaderCommand(SendTicketHeaderRequest request)
        {
            Request = request;
        }
    }
}
