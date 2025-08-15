using GMP3Integration.Application.DTOs.ItemSale;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP3Integration.Application.Features.Commands.ItemSale
{
    public class ItemSaleCommand : IRequest<ItemSaleResponse>
    {
        public ItemSaleRequest Request { get; private set; }
        public ItemSaleCommand(ItemSaleRequest request)
        {
            Request = request;
        }
    }
}
