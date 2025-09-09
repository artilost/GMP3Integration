using GMP3Integration.Application.DTOs;
using GMP3Integration.Application.DTOs.CloseTransaction;
using GMP3Integration.Application.DTOs.DepertmenConfiguration;
using GMP3Integration.Application.DTOs.ItemSale;
using GMP3Integration.Application.DTOs.ITransactionWorkflowService;
using GMP3Integration.Application.DTOs.ITransactionWorkflowService.Inputs;
using GMP3Integration.Application.DTOs.OptionFlags;
using GMP3Integration.Application.DTOs.Payment;
using GMP3Integration.Application.DTOs.PrintBeforeMf;
using GMP3Integration.Application.DTOs.PrintMessage;
using GMP3Integration.Application.DTOs.PrintMf;
using GMP3Integration.Application.DTOs.PrintTotalsAndPayments;
using GMP3Integration.Application.DTOs.TicketHeader;
using GMP3Integration.Application.Interfaces;
using GMP3Integration.Application.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace GMP3Integration.Application.Services
{
    public class TransactionWorkflowService : ITransactionWorkflowService
    {
        private readonly ILogger<TransactionWorkflowService> _logger;

        private readonly IGmp3Service _gmp3Service;
        private readonly Gmp3Options _opts;
        /// <summary>
        /// Constructor: IGmp3Service bağımlılığı DI ile sağlanır.
        /// </summary>
        /// <param name="gmp3Service">GMP3 entegrasyon servisi.</param>
        public TransactionWorkflowService(IGmp3Service gmp3Service,
            IOptions<Gmp3Options> opts,
            ILogger<TransactionWorkflowService> logger)
        {
            _gmp3Service = gmp3Service;
            _opts = opts.Value;
            _logger = logger ?? NullLogger<TransactionWorkflowService>.Instance;
        }

        /// <inheritdoc />
        public async Task<CompleteSaleResponse> ExecuteCompleteSaleAsync(CompleteSaleRequest request)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(request.CurrentInterface))
                throw new ArgumentException("currentInterface zorunludur.");

            if (request.Items == null || request.Items.Count == 0)
                throw new ArgumentException("En az bir item gereklidir.");

            if (request.Payment == null)
                throw new ArgumentException("payment zorunludur.");

            // temel item kontrolleri
            foreach (var it in request.Items)
            {
                if (it.DeptIndex < 0) throw new ArgumentException("items[].deptIndex 0 veya büyük olmalıdır.");
                if (it.Amount <= 0) throw new ArgumentException("items[].amount > 0 olmalıdır (kuruş).");
                if (it.CurrencyCode <= 0) throw new ArgumentException("items[].currencyCode zorunludur (ör. 949).");
                if (it.Count <= 0) throw new ArgumentException("items[].count > 0 olmalıdır.");
                if (it.UnitType <= 0) throw new ArgumentException("items[].unitType > 0 olmalıdır.");
            }

            var iface = string.IsNullOrWhiteSpace(request.CurrentInterface)
            ? _opts.CurrentInterface
            : request.CurrentInterface;

            // 1. StartTransaction
            var startResp = await _gmp3Service.StartTransactionAsync(
                new StartTransactionRequest { CurrentInterface = iface });

            var handle = startResp.TransactionHandle;

            using (_logger.BeginScope(new Dictionary<string, object> { { "transactionHandle", handle } }))
            {
                _logger.LogInformation("Transaction started. Interface={Interface}", iface);

                // 2. OptionFlags
                await _gmp3Service.SetOptionFlagsAsync(
                    new SetOptionFlagsRequest
                    {
                        TransactionHandle = handle,
                        ActiveFlags = request.ActiveFlags,
                        FlagsToBeSet = request.FlagsToBeSet
                    });

                // 3. TicketHeader
                await _gmp3Service.SendTicketHeaderAsync(
                    new SendTicketHeaderRequest
                    {
                        TransactionHandle = handle,
                        TicketType = request.TicketType
                    });

                // 4. ItemSale
                foreach (var x in request.Items ?? Enumerable.Empty<WorkflowItem>())
                {
                    var saleRequest = new ItemSaleRequest
                    {
                        TransactionHandle = handle,
                        Type = x.Type,
                        SubType = x.SubType,
                        DeptIndex = x.DeptIndex,
                        Amount = x.Amount,
                        CurrencyCode = x.CurrencyCode,
                        Count = x.Count,
                        UnitType = x.UnitType,
                        ItemCode = x.ItemCode ?? string.Empty,
                        Name = x.Name ?? string.Empty,
                        Barcode = x.Barcode ?? string.Empty,
                        Flag = x.Flag ?? 0
                    };
                    await _gmp3Service.SaleItemAsync(saleRequest);
                }

                // 5. Payment
                var pay = request.Payment;
                var payReq = new PaymentRequest
                {
                    // PaymentRequest'te artık TransactionHandle yok
                    // Doküman isimleri → PaymentRequest alanları
                    TypeOfPayment = MapPaymentType(pay.TypeOfPayment),
                    SubtypeOfPayment = MapSubtypeOfPayment(pay.SubtypeOfPayment),
                    PayAmount = (uint)pay.PayAmount,
                    PayAmountCurrencyCode = (ushort)pay.PayAmountCurrencyCode,
                    BankPaymentUniqueId = string.IsNullOrWhiteSpace(pay.BankPaymentUniqueId) ? string.Empty : pay.BankPaymentUniqueId
                };
                await _gmp3Service.MakePaymentAsync(payReq);

                // 6. PrintTotalsAndPayments
                await _gmp3Service.PrintTotalsAndPaymentsAsync(
                    new PrintTotalsAndPaymentsRequest { TransactionHandle = handle });

                // 7. PrintBeforeMF
                await _gmp3Service.PrintBeforeMfAsync(
                    new PrintBeforeMfRequest { TransactionHandle = handle });

                // 8. PrintMessages
                if (request.Messages != null)
                {
                    foreach (var m in request.Messages)
                    {
                        var msgReq = new PrintMessageRequest
                        {
                            TransactionHandle = handle,
                            MessageText = m.MessageText
                        };
                        await _gmp3Service.PrintMessageAsync(msgReq);
                    }
                }

                // 9. PrintMF
                await _gmp3Service.PrintMfAsync(
                    new PrintMfRequest { TransactionHandle = handle });
                /*
                // 12. CloseTransaction
                await _gmp3Service.CloseTransactionAsync(
                    new CloseTransactionRequest { TransactionHandle = handle });
                */
                return new CompleteSaleResponse
                {
                    TransactionHandle = handle,
                    Success = true
                };
            }
        }

        /// <summary>
        /// Map payment type string to correct uint value from documentation
        /// </summary>
        private uint MapPaymentType(string paymentType)
        {
            // C# 7.3 compatible switch statement
            switch (paymentType?.ToUpperInvariant())
            {
                case "CASH":
                case "CASH_TL":
                    return 0x00000001; // PAYMENT_CASH_TL
                case "CREDIT_CARD":
                case "BANK_CARD":
                    return 0x00000004; // PAYMENT_BANK_CARD
                case "YEMEKCEKI":
                    return 0x00000008; // PAYMENT_YEMEKCEKI
                case "MOBILE":
                    return 0x00000010; // PAYMENT_MOBILE
                default:
                    return 0x00000001; // Default to cash
            }
        }
        
        /// <summary>
        /// Map payment subtype string to uint value 
        /// </summary>
        private uint MapSubtypeOfPayment(string subtypeOfPayment)
        {
            // Try to parse as uint first
            if (uint.TryParse(subtypeOfPayment, out uint result))
            {
                return result;
            }
            
            // If it's a string, map to known values
            switch (subtypeOfPayment?.ToUpperInvariant())
            {
                case "SALE":
                case "REGULAR":
                case "NORMAL":
                    return 0; // Regular sale
                case "INSTALLMENT":
                case "TAKSIT":
                    return 1; // Installment sale
                case "BONUS":
                case "LOYALTY":
                    return 2; // Bonus/Loyalty sale
                default:
                    return 0; // Default to regular sale
            }
        }
    }
}
