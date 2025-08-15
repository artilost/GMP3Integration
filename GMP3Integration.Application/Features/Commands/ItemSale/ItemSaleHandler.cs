using GMP3Integration.Application.DTOs.ItemSale;
using GMP3Integration.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GMP3Integration.Application.Features.Commands.ItemSale
{
    public class ItemSaleHandler : IRequestHandler<ItemSaleCommand, ItemSaleResponse>
    {
        private readonly IGmp3Service _gmp3Service;
        public ItemSaleHandler(IGmp3Service gmp3Service)
        {
            _gmp3Service = gmp3Service;
        }
        public async Task<ItemSaleResponse> Handle(ItemSaleCommand request, CancellationToken cancellationToken)
        => await _gmp3Service.SaleItemAsync(request.Request);
    }
}
