using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP3Integration.Domain.Entities
{
    /// <summary>
    /// Cihaza işlem akışı sırasında özel bir mesaj yazdırmak için kullanılır.
    /// </summary>
    public class PrintMessage
    {
        /// <summary>
        /// Daha önce StartTransaction’dan alınan işlem tanımlayıcısı.
        /// </summary>
        public ulong TransactionHandle { get; set; }

        /// <summary>
        /// Cihaza yazdırılacak metin mesajı.
        /// </summary>
        public string MessageText { get; set; }
    }
}
