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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP3Integration.Application.Interfaces
{
    public interface IGmp3Service
    {
        Task<StartTransactionResponse> StartTransactionAsync(StartTransactionRequest request);
        Task<SetOptionFlagsResponse> SetOptionFlagsAsync(SetOptionFlagsRequest request);
        Task<SendTicketHeaderResponse> SendTicketHeaderAsync(SendTicketHeaderRequest request);
        Task<ItemSaleResponse> SaleItemAsync(ItemSaleRequest request);
        Task<PaymentResponse> MakePaymentAsync(PaymentRequest request);
        Task<PrintTotalsAndPaymentsResponse> PrintTotalsAndPaymentsAsync(PrintTotalsAndPaymentsRequest request);
        Task<PrintBeforeMfResponse> PrintBeforeMfAsync(PrintBeforeMfRequest request);
        Task<PrintMfResponse> PrintMfAsync(PrintMfRequest request);
        Task<CloseTransactionResponse> CloseTransactionAsync(CloseTransactionRequest request);
        Task<RefundResponse> RefundAsync(RefundRequest request);
        Task<PrintMessageResponse> PrintMessageAsync(PrintMessageRequest request);
        Task<GetTaxRatesResponse> GetTaxRatesAsync();
        Task<SetDepartmentsResponse> SetDepartmentsAsync(SetDepartmentsRequest request);
        //Task<CancelTransactionResponse> CancelTransactionAsync(CancelTransactionRequest request);
        Task<ForceResetResponse> ForceResetAsync(ForceResetRequest request);
    }
}
