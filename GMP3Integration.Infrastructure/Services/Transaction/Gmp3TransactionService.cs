using GMP3Integration.Application.DTOs;
using GMP3Integration.Application.DTOs.OptionFlags;
using GMP3Integration.Application.DTOs.TicketHeader;
using GMP3Integration.Application.DTOs.ItemSale;
using GMP3Integration.Application.DTOs.Payment;
using GMP3Integration.Application.DTOs.PrintTotalsAndPayments;
using GMP3Integration.Application.DTOs.PrintBeforeMf;
using GMP3Integration.Application.DTOs.PrintMf;
using GMP3Integration.Application.DTOs.Refund;
using GMP3Integration.Application.DTOs.PrintMessage;
using GMP3Integration.Application.DTOs.TaxRates;
using GMP3Integration.Application.DTOs.DepertmenConfiguration;
using GMP3Integration.Application.DTOs.ForceReset;
using GMP3Integration.Application.DTOs.CloseTransaction;
using GMP3Integration.Application.Interfaces;
using System.Threading.Tasks;

namespace GMP3Integration.Infrastructure.Services.Transaction
{
    /// <summary>
    /// GMP3 Transaction işlemleri için ayrı service
    /// </summary>
    public class Gmp3TransactionService : IGmp3Service
    {
        private readonly IGmp3Service _gmp3Service;

        public Gmp3TransactionService(IGmp3Service gmp3Service)
        {
            _gmp3Service = gmp3Service;
        }

        public async Task<StartTransactionResponse> StartTransactionAsync(StartTransactionRequest request)
        {
            return await _gmp3Service.StartTransactionAsync(request);
        }

        public async Task<SetOptionFlagsResponse> SetOptionFlagsAsync(SetOptionFlagsRequest request)
        {
            return new SetOptionFlagsResponse { Success = true };
        }

        public async Task<SendTicketHeaderResponse> SendTicketHeaderAsync(SendTicketHeaderRequest request)
        {
            return new SendTicketHeaderResponse { Success = true };
        }

        public async Task<ItemSaleResponse> SaleItemAsync(ItemSaleRequest request)
        {
            return new ItemSaleResponse { Success = true };
        }

        public async Task<PaymentResponse> MakePaymentAsync(PaymentRequest request)
        {
            return new PaymentResponse { Success = true };
        }

        public async Task<PrintTotalsAndPaymentsResponse> PrintTotalsAndPaymentsAsync(PrintTotalsAndPaymentsRequest request)
        {
            return new PrintTotalsAndPaymentsResponse { Success = true };
        }

        public async Task<PrintBeforeMfResponse> PrintBeforeMfAsync(PrintBeforeMfRequest request)
        {
            return new PrintBeforeMfResponse { Success = true };
        }

        public async Task<PrintMfResponse> PrintMfAsync(PrintMfRequest request)
        {
            return new PrintMfResponse { Success = true };
        }

        public async Task<RefundResponse> RefundAsync(RefundRequest request)
        {
            return new RefundResponse { Success = true };
        }

        public async Task<CloseTransactionResponse> CloseTransactionAsync(CloseTransactionRequest request)
        {
            return await _gmp3Service.CloseTransactionAsync(request);
        }

        public async Task<PrintMessageResponse> PrintMessageAsync(PrintMessageRequest request)
        {
            return new PrintMessageResponse { Success = true };
        }

        public async Task<GetTaxRatesResponse> GetTaxRatesAsync()
        {
            return new GetTaxRatesResponse { Success = true };
        }

        public async Task<SetDepartmentsResponse> SetDepartmentsAsync(SetDepartmentsRequest request)
        {
            return new SetDepartmentsResponse { Success = true };
        }

        public async Task<ForceResetResponse> ForceResetAsync(ForceResetRequest request)
        {
            return new ForceResetResponse { Reset = true };
        }
    }
}
