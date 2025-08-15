using GMP3Integration.Application.DTOs;
using GMP3Integration.Application.DTOs.CancelTansaction;
using GMP3Integration.Application.DTOs.CanselTransaction;
using GMP3Integration.Application.DTOs.CloseTransaction;
using GMP3Integration.Application.DTOs.DepertmenConfiguration;
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP3Integration.Application.Services
{
    public class Gmp3Service : IGmp3Service
    {
        public Task<CancelTransactionResponse> CancelTransactionAsync(CancelTransactionRequest request)
        {
            throw new NotImplementedException();
        }
        public Task<CloseTransactionResponse> CloseTransactionAsync(CloseTransactionRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<GetTaxRatesResponse> GetTaxRatesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<PaymentResponse> MakePaymentAsync(PaymentRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<PrintBeforeMfResponse> PrintBeforeMfAsync(PrintBeforeMfRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<PrintMessageResponse> PrintMessageAsync(PrintMessageRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<PrintMfResponse> PrintMfAsync(PrintMfRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<PrintTotalsAndPaymentsResponse> PrintTotalsAndPaymentsAsync(PrintTotalsAndPaymentsRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<RefundResponse> RefundAsync(RefundRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ItemSaleResponse> SaleItemAsync(ItemSaleRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<SendTicketHeaderResponse> SendTicketHeaderAsync(SendTicketHeaderRequest request)
        {
            throw new NotImplementedException();
        }


        public Task<SetDepartmentsResponse> SetDepartmentsAsync(SetDepartmentsRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<SetOptionFlagsResponse> SetOptionFlagsAsync(SetOptionFlagsRequest request)
        {
            throw new NotImplementedException();
        }

        Task<StartTransactionResponse> IGmp3Service.StartTransactionAsync(StartTransactionRequest request)
        {
            throw new NotImplementedException();
        }

    }
}
