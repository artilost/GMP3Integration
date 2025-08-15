using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP3Integration.Domain.Entities
{
    public class ItemSale
    {
        public ulong TransactionHandle { get; set; }
        public string ItemCode { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
