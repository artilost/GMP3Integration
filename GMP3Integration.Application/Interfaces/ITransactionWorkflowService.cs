using GMP3Integration.Application.DTOs.ITransactionWorkflowService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP3Integration.Application.Interfaces
{
    /// <summary>
    /// Tüm satış akışını baştan sona yürüten servis arayüzü.
    /// </summary>
    public interface ITransactionWorkflowService
    {
        /// <summary>
        /// Başlangıçtan kapanışa kadar Complete Sale akışını tetikler.
        /// </summary>
        /// <param name="request">CompleteSaleRequest ile tüm parametreleri alır.</param>
        /// <returns>CompleteSaleResponse ile işlem sonucu ve handle döner.</returns>
        Task<CompleteSaleResponse> ExecuteCompleteSaleAsync(CompleteSaleRequest request);
    }
}
