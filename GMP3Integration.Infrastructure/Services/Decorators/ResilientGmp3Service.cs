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
using Microsoft.Extensions.Logging;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Polly.Timeout;
using System;
using System.ComponentModel; 
using System.IO;
using System.Threading.Tasks;

namespace GMP3Integration.Infrastructure.Services.Decorators
{
    /// <summary>
    /// IGmp3Service çağrılarına timeout + retry + circuit-breaker uygular.
    /// </summary>
    public class ResilientGmp3Service :IGmp3Service
    {
        private readonly IGmp3Service _inner;
        private readonly ILogger<ResilientGmp3Service> _logger;

        private readonly AsyncTimeoutPolicy _timeoutPolicy;
        private readonly AsyncRetryPolicy _retryPolicy;
        private readonly AsyncCircuitBreakerPolicy _breakerPolicy;
        private readonly IAsyncPolicy _policy;

        public ResilientGmp3Service(IGmp3Service inner, ILogger<ResilientGmp3Service> logger)
        {
            _inner = inner ?? throw new ArgumentNullException(nameof(inner));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _timeoutPolicy = Policy.TimeoutAsync(TimeSpan.FromSeconds(10)); // her çağrı en fazla 10 sn

            _retryPolicy = Policy
                .Handle<TimeoutRejectedException>()
                .Or<IOException>()
                .Or<Win32Exception>()
                .WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: i => TimeSpan.FromMilliseconds(i == 1 ? 200 : i == 2 ? 400 : 800),
                    onRetry: (ex, delay, attempt, ctx) =>
                    {
                        _logger.LogWarning(ex, "GMP3 retry #{Attempt} (bekleme={Delay}ms)", attempt, (int)delay.TotalMilliseconds);
                    });

            _breakerPolicy = Policy
                .Handle<TimeoutRejectedException>()
                .Or<IOException>()
                .Or<Win32Exception>()
                .CircuitBreakerAsync(
                    exceptionsAllowedBeforeBreaking: 5,
                    durationOfBreak: TimeSpan.FromSeconds(30),
                    onBreak: (ex, ts) => _logger.LogWarning(ex, "GMP3 circuit OPEN (süre={Duration}s)", (int)ts.TotalSeconds),
                    onReset: () => _logger.LogInformation("GMP3 circuit RESET"),
                    onHalfOpen: () => _logger.LogInformation("GMP3 circuit HALF-OPEN"));

            // Sıra: breaker (dış) → retry → timeout (iç)
            _policy = Policy.WrapAsync(_breakerPolicy, _retryPolicy, _timeoutPolicy);
        }
        private Task<T> Execute<T>(Func<Task<T>> action, string op)
        {
            return _policy.ExecuteAsync(async () =>
            {
                _logger.LogInformation("GMP3 call START: {Op}", op);
                var result = await action();
                _logger.LogInformation("GMP3 call END: {Op}", op);
                return result;
            });
        }

        // ↓↓↓ IGmp3Service metotlarının her birini policy ile sarmalıyoruz ↓↓↓

        public Task<StartTransactionResponse> StartTransactionAsync(StartTransactionRequest request)
            => Execute(() => _inner.StartTransactionAsync(request), "StartTransaction");

        public Task<SetOptionFlagsResponse> SetOptionFlagsAsync(SetOptionFlagsRequest request)
            => Execute(() => _inner.SetOptionFlagsAsync(request), "SetOptionFlags");

        public Task<SendTicketHeaderResponse> SendTicketHeaderAsync(SendTicketHeaderRequest request)
            => Execute(() => _inner.SendTicketHeaderAsync(request), "SendTicketHeader");

        public Task<ItemSaleResponse> SaleItemAsync(ItemSaleRequest request)
            => Execute(() => _inner.SaleItemAsync(request), "ItemSale");

        public Task<PaymentResponse> MakePaymentAsync(PaymentRequest request)
            => Execute(() => _inner.MakePaymentAsync(request), "MakePayment");

        public Task<PrintTotalsAndPaymentsResponse> PrintTotalsAndPaymentsAsync(PrintTotalsAndPaymentsRequest request)
            => Execute(() => _inner.PrintTotalsAndPaymentsAsync(request), "PrintTotalsAndPayments");

        public Task<PrintBeforeMfResponse> PrintBeforeMfAsync(PrintBeforeMfRequest request)
            => Execute(() => _inner.PrintBeforeMfAsync(request), "PrintBeforeMF");

        public Task<PrintMessageResponse> PrintMessageAsync(PrintMessageRequest request)
            => Execute(() => _inner.PrintMessageAsync(request), "PrintMessage");

        public Task<PrintMfResponse> PrintMfAsync(PrintMfRequest request)
            => Execute(() => _inner.PrintMfAsync(request), "PrintMF");
        /*
        public Task<CloseTransactionResponse> CloseTransactionAsync(CloseTransactionRequest request)
            => Execute(() => _inner.CloseTransactionAsync(request), "CloseTransaction");
       
        public Task<CancelTransactionResponse> CancelTransactionAsync(CancelTransactionRequest request)
            => Execute(() => _inner.CancelTransactionAsync(request), "CancelTransaction");
         */
        public Task<GetTaxRatesResponse> GetTaxRatesAsync()
            => Execute(() => _inner.GetTaxRatesAsync(), "GetTaxRates");

        public Task<SetDepartmentsResponse> SetDepartmentsAsync(SetDepartmentsRequest request)
            => Execute(() => _inner.SetDepartmentsAsync(request), "SetDepartments");

        public Task<RefundResponse> RefundAsync(RefundRequest request)
        => Execute(() => _inner.RefundAsync(request), "Refund");

        public Task<ForceResetResponse> ForceResetAsync(ForceResetRequest request)
        => Execute(() => _inner.ForceResetAsync(request), "ForceReset");
    }
}
