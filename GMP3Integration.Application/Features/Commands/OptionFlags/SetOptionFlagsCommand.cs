using GMP3Integration.Application.DTOs.OptionFlags;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP3Integration.Application.Features.Commands.OptionFlags
{
    public class SetOptionFlagsCommand : IRequest<SetOptionFlagsResponse>
    {
        public SetOptionFlagsRequest Request { get; private set; }
        public SetOptionFlagsCommand(SetOptionFlagsRequest request)
        {
            Request = request;
        }
    }
}
