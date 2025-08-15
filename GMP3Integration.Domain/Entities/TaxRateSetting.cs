using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP3Integration.Domain.Entities
{
    /// <summary>
    /// Bir işlem için vergi oranı ayarlarını tutar.
    /// </summary>
    public class TaxRateSetting
    {
        /// <summary>
        /// StartTransaction’dan alınan işlem tanımlayıcısı.
        /// </summary>
        public ulong TransactionHandle { get; set; }

        /// <summary>
        /// Vergi kodu veya tipi (örneğin “A”, “B” veya %18).
        /// </summary>
        public string TaxCode { get; set; }

        /// <summary>
        /// Yüzdelik vergi oranı (ör. 18.0m).
        /// </summary>
        public decimal Rate { get; set; }
    }
}
