using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP3Integration.Domain.Entities
{
    /// <summary>
    /// Bir işlem için departman numarası ve ayarlarını tutar.
    /// </summary>
    public class DepartmentConfiguration
    {
        /// <summary>
        /// StartTransaction’dan alınan işlem kimliği.
        /// </summary>
        public ulong TransactionHandle { get; set; }

        /// <summary>
        /// Cihazdaki departman numarası (1–n arası).
        /// </summary>
        public int DepartmentNumber { get; set; }
    }
}
