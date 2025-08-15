using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP3Integration.Application.DTOs.ItemSale
{
    public class ItemSaleRequest
    {
        public ulong TransactionHandle { get; set; }

        public int Type { get; set; }          // genelde 1
        public int SubType { get; set; }       // genelde 0
        public int DeptIndex { get; set; }     // 0-based (Departman 1 => 0)
        public int Amount { get; set; }        // TL * 100 (ör. 175.00 TL => 17500)
        public int CurrencyCode { get; set; }  // 949 = TRY
        public int Count { get; set; }         // adet
        public int UnitType { get; set; }      // sayılabilir ürün = 1

        // Kimlik/opsiyonel alanlar:
        public string ItemCode { get; set; }   // ürün kodu
        public string Name { get; set; }       
        public string Barcode { get; set; }    
        public int Flag { get; set; }          
    }
}
