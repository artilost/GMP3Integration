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
using GMP3Integration.Infrastructure.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP3Integration.Infrastructure.Services
{
    public class Gmp3InteropService : IGmp3Service
    {
        Task<StartTransactionResponse> IGmp3Service.StartTransactionAsync(StartTransactionRequest request)
        {
            // Native stub çağrısı
            var result = new StartTransactionResponse();
            try
            {

                Gmp3NativeMethods.FP3_Start_Native(request.CurrentInterface, out var handle);
                result.TransactionHandle = handle;
                return Task.FromResult(result);
            }
            catch (NotImplementedException)
            {
                // DLL hazır olana kadar geçici stub
                result.TransactionHandle = 123456;
                return Task.FromResult(result);
            }
        }
        public async Task<SetOptionFlagsResponse> SetOptionFlagsAsync(SetOptionFlagsRequest request)
        {
            try
            {
                Gmp3NativeMethods.FP3_OptionFlags_Native(
                    request.TransactionHandle,
                    request.ActiveFlags,
                    request.FlagsToBeSet
                    );
                return new SetOptionFlagsResponse { Success = true };
            }
            catch (NotImplementedException)
            {
                // placeholder davranış
                return new SetOptionFlagsResponse { Success = true };
            }
        }
        public async Task<SendTicketHeaderResponse> SendTicketHeaderAsync(SendTicketHeaderRequest request)
        {
            try
            {
                Gmp3NativeMethods.FP3_TicketHeader_Native(request.TransactionHandle, request.TicketType);
                return new SendTicketHeaderResponse { Success = true };
            }
            catch (NotImplementedException)
            {
                // Stub davranışı: her zaman başarılı dön
                return new SendTicketHeaderResponse { Success = true };
            }
        }
        public async Task<ItemSaleResponse> SaleItemAsync(ItemSaleRequest request)
        {
            try
            {
                Gmp3NativeMethods.FP3_ItemSale_Native(
                    request.TransactionHandle, request.Type, request.SubType, request.DeptIndex, request.Amount, request.CurrencyCode,
                    request.Count, request.UnitType, request.ItemCode ?? string.Empty, request.Name ?? string.Empty, request.Barcode ?? string.Empty, request.Flag);
                return new ItemSaleResponse { Success = true };
            }
            catch (NotImplementedException) { return new ItemSaleResponse { Success = true }; }
        }
        public async Task<PaymentResponse> MakePaymentAsync(PaymentRequest request)
        {

            try
            {
                Gmp3NativeMethods.FP3_Payment_Native(
                    request.TransactionHandle,
                    request.TypeOfPayment,
                    request.SubtypeOfPayment,
                    request.PayAmount,
                    request.PayAmountCurrencyCode,
                    string.IsNullOrWhiteSpace(request.BankPaymentUniqueId) ? string.Empty : request.BankPaymentUniqueId);
                return new PaymentResponse { Success = true };
            }
            catch (NotImplementedException) { return new PaymentResponse { Success = true }; }
        }
        public async Task<PrintTotalsAndPaymentsResponse> PrintTotalsAndPaymentsAsync(PrintTotalsAndPaymentsRequest request)
        {
            try
            {
                Gmp3NativeMethods.FP3_PrintTotalsAndPayments_Native(request.TransactionHandle);
                return new PrintTotalsAndPaymentsResponse { Success = true };
            }
            catch (NotImplementedException)
            {
                // Stub davranışı: her zaman başarılı dön
                return new PrintTotalsAndPaymentsResponse { Success = true };
            }
        }
        public async Task<PrintBeforeMfResponse> PrintBeforeMfAsync(PrintBeforeMfRequest request)
        {
            try
            {
                Gmp3NativeMethods.FP3_PrintBeforeMF_Native(request.TransactionHandle);
                return new PrintBeforeMfResponse { Success = true };
            }
            catch (NotImplementedException)
            {
                // Stub davranışı: placeholder olarak başarılı dön
                return new PrintBeforeMfResponse { Success = true };
            }
        }
        public async Task<PrintMfResponse> PrintMfAsync(PrintMfRequest request)
        {
            try
            {
                Gmp3NativeMethods.FP3_PrintMF_Native(request.TransactionHandle);
                return new PrintMfResponse { Success = true };
            }
            catch (NotImplementedException)
            {
                // Stub davranışı: placeholder olarak başarılı dön
                return new PrintMfResponse { Success = true };
            }
        }
        public async Task<CloseTransactionResponse> CloseTransactionAsync(CloseTransactionRequest request)
        {
            try
            {
                Gmp3NativeMethods.FP3_Close_Native(request.TransactionHandle);
                return new CloseTransactionResponse { Success = true };
            }
            catch (NotImplementedException)
            {
                // Stub davranışı: her zaman başarılı dön
                return new CloseTransactionResponse { Success = true };
            }
        }
        public async Task<RefundResponse> RefundAsync(RefundRequest request)
        {
            try
            {
                Gmp3NativeMethods.FP3_Refund_Native(request.TransactionHandle, request.Amount);
                return new RefundResponse { Success = true };
            }
            catch (NotImplementedException)
            {
                // Stub davranışı: her zaman başarılı dönüyoruz
                return new RefundResponse { Success = true };
            }
        }

        public async Task<PrintMessageResponse> PrintMessageAsync(PrintMessageRequest request)
        {
            try
            {
                Gmp3NativeMethods.FP3_PrintMessage_Native(request.TransactionHandle, request.MessageText);
                return new PrintMessageResponse { Success = true };

            }
            catch (NotImplementedException)
            {

                return new PrintMessageResponse { Success = true };
            }
        }
        public async Task<GetTaxRatesResponse> GetTaxRatesAsync()
        {
            // DLL gelene kadar stub veri döndürüyoruz
           return new GetTaxRatesResponse
            {
                Rates = new List<TaxRateDto>
                {
                    new TaxRateDto { Index = 0, TaxCode = "1",  Rate = 1.0m },
                    new TaxRateDto { Index = 1, TaxCode = "8",  Rate = 8.0m },
                    new TaxRateDto { Index = 2, TaxCode = "18", Rate = 18.0m }
                }
            };
        }
        public async Task<SetDepartmentsResponse> SetDepartmentsAsync(SetDepartmentsRequest request)
        {
            try
            {
                var arr = (request.Departments ?? new List<DepartmentConfigItem>()).ToArray();
                Gmp3NativeMethods.FP3_SetDepartments_Native(request.TransactionHandle, arr, arr.Length);
                return new SetDepartmentsResponse { Success = true };
            }
            catch (NotImplementedException)
            {
                // DLL yokken stub olarak başarılı sayıyoruz
                return new SetDepartmentsResponse { Success = true };
            }
        }
        public async Task<CancelTransactionResponse> CancelTransactionAsync(CancelTransactionRequest request)
        {
            try
            {
                Gmp3NativeMethods.FP3_CancelTransaction_Native(request.TransactionHandle);
                return new CancelTransactionResponse { Success = true };
            }
            catch (NotImplementedException)
            {
                // Stub aşamasında başarılı sayıyoruz
                return new CancelTransactionResponse { Success = true };
            }
        }
    }
}
