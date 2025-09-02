using GMP3Integration.Application.DTOs;
using GMP3Integration.Application.DTOs.CancelTansaction;
using GMP3Integration.Application.DTOs.CanselTransaction;
using GMP3Integration.Application.DTOs.CloseTransaction;
using GMP3Integration.Application.DTOs.DepertmenConfiguration;
using GMP3Integration.Application.DTOs.ForceReset;
using GMP3Integration.Application.DTOs.ItemSale;
using GMP3Integration.Application.DTOs.OptionFlags;
using GMP3Integration.Application.DTOs.Payment;
using GMP3Integration.Application.DTOs.PrintBeforeMf;
using GMP3Integration.Application.DTOs.PrintMessage;
using GMP3Integration.Application.DTOs.PrintMf;
using GMP3Integration.Application.DTOs.PrintTotalsAndPayments;
using GMP3Integration.Application.DTOs.Refund;
using GMP3Integration.Application.DTOs.TaxRates;
using GMP3Integration.Application.DTOs.TicketHeader;
using GMP3Integration.Application.Interfaces;
using System;
using System.Threading.Tasks;

namespace GMP3Integration.Application.Services
{
    // ⚠️ BU SINIF KULLANILMIYOR - TÜM METODLAR NotImplementedException FIRLATIYOR
    // Infrastructure katmanındaki Gmp3InteropService kullanılıyor
    // Bu sınıfı sil veya tamamen kaldır
    [Obsolete("Bu sınıf kullanılmıyor. Gmp3InteropService kullanın.")]
    public class Gmp3Service : IGmp3Service
    {
        // Tüm metodlar NotImplementedException fırlatıyor - bu sınıf kullanılmıyor
        public Task<CancelTransactionResponse> CancelTransactionAsync(CancelTransactionRequest request)
        {
            throw new NotImplementedException("Bu sınıf kullanılmıyor. Gmp3InteropService kullanın.");
        }
        
        public Task<CloseTransactionResponse> CloseTransactionAsync(CloseTransactionRequest request)
        {
            throw new NotImplementedException("Bu sınıf kullanılmıyor. Gmp3InteropService kullanın.");
        }

        public Task<ForceResetResponse> ForceResetAsync(ForceResetRequest request)
        {
            throw new NotImplementedException("Bu sınıf kullanılmıyor. Gmp3InteropService kullanın.");
        }

        public Task<GetTaxRatesResponse> GetTaxRatesAsync()
        {
            throw new NotImplementedException("Bu sınıf kullanılmıyor. Gmp3InteropService kullanın.");
        }

        public Task<PaymentResponse> MakePaymentAsync(PaymentRequest request)
        {
            throw new NotImplementedException("Bu sınıf kullanılmıyor. Gmp3InteropService kullanın.");
        }

        public Task<PrintBeforeMfResponse> PrintBeforeMfAsync(PrintBeforeMfRequest request)
        {
            throw new NotImplementedException("Bu sınıf kullanılmıyor. Gmp3InteropService kullanın.");
        }

        public Task<PrintMessageResponse> PrintMessageAsync(PrintMessageRequest request)
        {
            throw new NotImplementedException("Bu sınıf kullanılmıyor. Gmp3InteropService kullanın.");
        }

        public Task<PrintMfResponse> PrintMfAsync(PrintMfRequest request)
        {
            throw new NotImplementedException("Bu sınıf kullanılmıyor. Gmp3InteropService kullanın.");
        }

        public Task<PrintTotalsAndPaymentsResponse> PrintTotalsAndPaymentsAsync(PrintTotalsAndPaymentsRequest request)
        {
            throw new NotImplementedException("Bu sınıf kullanılmıyor. Gmp3InteropService kullanın.");
        }

        public Task<RefundResponse> RefundAsync(RefundRequest request)
        {
            throw new NotImplementedException("Bu sınıf kullanılmıyor. Gmp3InteropService kullanın.");
        }

        public Task<ItemSaleResponse> SaleItemAsync(ItemSaleRequest request)
        {
            throw new NotImplementedException("Bu sınıf kullanılmıyor. Gmp3InteropService kullanın.");
        }

        public Task<SendTicketHeaderResponse> SendTicketHeaderAsync(SendTicketHeaderRequest request)
        {
            throw new NotImplementedException("Bu sınıf kullanılmıyor. Gmp3InteropService kullanın.");
        }

        public Task<SetDepartmentsResponse> SetDepartmentsAsync(SetDepartmentsRequest request)
        {
            throw new NotImplementedException("Bu sınıf kullanılmıyor. Gmp3InteropService kullanın.");
        }

        public Task<SetOptionFlagsResponse> SetOptionFlagsAsync(SetOptionFlagsRequest request)
        {
            throw new NotImplementedException("Bu sınıf kullanılmıyor. Gmp3InteropService kullanın.");
        }

        public Task<StartTransactionResponse> StartTransactionAsync(StartTransactionRequest request)
        {
            throw new NotImplementedException("Bu sınıf kullanılmıyor. Gmp3InteropService kullanın.");
        }
    }
}
