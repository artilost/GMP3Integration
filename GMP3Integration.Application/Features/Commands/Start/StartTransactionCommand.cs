using GMP3Integration.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP3Integration.Application.Features.Commands.Start
{
    public class StartTransactionCommand : IRequest<StartTransactionResponse>
    {
        // Body boş gelirse config'teki interface kullanılacak
        public string CurrentInterface { get; set; }
        // public int TimeoutMs { get; set; } = 3000;
    }
}
