using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP3Integration.Application.DTOs
{
    /// <summary>
    /// StartTransaction için response DTO
    /// Doküman sayfa 16: "First function to start a transaction"
    /// </summary>
    public class StartTransactionResponse
    {
        /// <summary>
        /// İşlem başarılı mı?
        /// </summary>
        public bool Success { get; set; }
        
        /// <summary>
        /// Transaction handle (Doküman sayfa 16: "transaction handle")
        /// </summary>
        public ulong TransactionHandle { get; set; }
        
        /// <summary>
        /// Return code from DLL
        /// </summary>
        public int Rc { get; set; }
        
        /// <summary>
        /// Success message
        /// </summary>
        public string Message { get; set; }
        
        /// <summary>
        /// Error message if failed
        /// </summary>
        public string ErrorMessage { get; set; }
        
        /// <summary>
        /// Existing open ticket flag
        /// </summary>
        public bool ExistingOpenTicket { get; set; }
        
        /// <summary>
        /// Interface that was used (legacy)
        /// </summary>
        public string InterfaceUsed { get; set; }
        
        /// <summary>
        /// Interface that was used
        /// </summary>
        public string Interface { get; set; }
    }
}
