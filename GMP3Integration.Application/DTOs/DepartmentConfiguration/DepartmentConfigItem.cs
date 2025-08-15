using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP3Integration.Application.DTOs.DepertmenConfiguration
{
    public class DepartmentConfigItem
    {
        public int DeptIndex { get; set; }     // 0-based (Departman 1 => 0)
        public int TaxIndex { get; set; }      // GetTaxRates'ten gelen Index
        public int CurrencyCode { get; set; }  // 949 = TRY
        public int UnitType { get; set; }      // 1 = sayılabilir ürün
        public string Name { get; set; }       // opsiyonel, boş geçilebilir
    }
}
