using GMP3Integration.API.Filters;
using GMP3Integration.Application.DTOs;
using GMP3Integration.Application.DTOs.CancelTansaction;
using GMP3Integration.Application.DTOs.CanselTransaction;
using GMP3Integration.Application.DTOs.CloseTransaction;
using GMP3Integration.Application.DTOs.DepertmenConfiguration;
using GMP3Integration.Application.DTOs.ItemSale;
using GMP3Integration.Application.DTOs.ITransactionWorkflowService;
using GMP3Integration.Application.DTOs.OptionFlags;
using GMP3Integration.Application.DTOs.Payment;
using GMP3Integration.Application.DTOs.PrintBeforeMf;
using GMP3Integration.Application.DTOs.PrintMessage;
using GMP3Integration.Application.DTOs.PrintMf;
using GMP3Integration.Application.DTOs.PrintTotalsAndPayments;
using GMP3Integration.Application.DTOs.Refund;
using GMP3Integration.Application.DTOs.TaxRates;
using GMP3Integration.Application.DTOs.TicketHeader;
using GMP3Integration.Application.Features.Commands;
using GMP3Integration.Application.Features.Commands.CancelTrasnaction;
using GMP3Integration.Application.Features.Commands.CloseTransaction;
using GMP3Integration.Application.Features.Commands.CompleteSale;
using GMP3Integration.Application.Features.Commands.DepartmentConfiguration;
using GMP3Integration.Application.Features.Commands.ItemSale;
using GMP3Integration.Application.Features.Commands.OptionFlags;
using GMP3Integration.Application.Features.Commands.Payment;
using GMP3Integration.Application.Features.Commands.PrintBeforeMf;
using GMP3Integration.Application.Features.Commands.PrintTotalsAndPayments;
using GMP3Integration.Application.Features.Commands.Start;
using GMP3Integration.Application.Features.Commands.TicketHeader;
using GMP3Integration.Application.Features.Queries;
using GMP3Integration.Application.Features.Queries.GetTaxRates;
using GMP3Integration.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.Tasks;

namespace GMP3Integration.API.Controllers
{
    [EnableRateLimiting("device-serial")]
    [ApiController]
    [Route("api/[controller]")]
    public class Gmp3Controller : ControllerBase
    {
        private readonly ITransactionWorkflowService _workflowService;
        private readonly IGmp3Service _gmp3Service;
        private readonly ILogger<Gmp3Controller> _logger;
        private readonly IMediator _mediator;

        public Gmp3Controller(IGmp3Service gmp3Service, ITransactionWorkflowService workflowService, ILogger<Gmp3Controller> logger, IMediator mediator)
        {
            _workflowService = workflowService;
            _gmp3Service = gmp3Service;
            _logger = logger;
            _mediator = mediator;
        }

        /// <summary>
        /// Tüm satış akışını (complete sale) başlatır.
        /// </summary>
        [HttpPost("complete-sale")]
        public async Task<ActionResult<CompleteSaleResponse>> CompleteSale([FromBody] CompleteSaleRequest request)
        {
            var resp = await _mediator.Send(new CompleteSaleCommand(request));
            return Ok(resp);
        }

        /// <summary>
        /// Yeni bir işlem başlatır ve transaction handle döner.

        /// </summary>
        [HttpPost("start")]
        public async Task<ActionResult<StartTransactionResponse>> Start([FromBody] StartTransactionRequest request)
        {
            var resp = await _mediator.Send(new StartTransactionCommand(request));
            return Ok(resp);
        }

        [ServiceFilter(typeof(TransactionHandleScopeFilter))]
        [HttpPost("option-flags")]
        public async Task<ActionResult<SetOptionFlagsResponse>> SetOptionFlags([FromBody] SetOptionFlagsRequest request)
        {
            var resp = await _mediator.Send(new SetOptionFlagsCommand(request));
            return Ok(resp);
        }

        [ServiceFilter(typeof(TransactionHandleScopeFilter))]
        [HttpPost("ticket-header")]
        public async Task<ActionResult<SendTicketHeaderResponse>> TicketHeader([FromBody] SendTicketHeaderRequest request)
        {
            var resp = await _mediator.Send(new SendTicketHeaderCommand(request));
            return Ok(resp);
        }

        [ServiceFilter(typeof(TransactionHandleScopeFilter))]
        [HttpPost("item-sale")]
        public async Task<ActionResult<ItemSaleResponse>> ItemSale([FromBody] ItemSaleRequest request)
        {
            var resp = await _mediator.Send(new ItemSaleCommand(request));
            return Ok(resp);
        }

        [ServiceFilter(typeof(TransactionHandleScopeFilter))]
        [HttpPost("payment")]
        public async Task<ActionResult<PaymentResponse>> Payment([FromBody] PaymentRequest request)
        {
            var resp = await _mediator.Send(new MakePaymentCommand(request));
            return Ok(resp);
        }

        [ServiceFilter(typeof(TransactionHandleScopeFilter))]
        [HttpPost("print-totals-and-payments")]
        public async Task<ActionResult<PrintTotalsAndPaymentsResponse>> PrintTotalsAndPayments([FromBody] PrintTotalsAndPaymentsRequest request)
        {
            var resp = await _mediator.Send(new PrintTotalsAndPaymentsCommand(request));
            return Ok(resp);
        }

        [ServiceFilter(typeof(TransactionHandleScopeFilter))]
        [HttpPost("print-before-mf")]
        public async Task<ActionResult<PrintBeforeMfResponse>> PrintBeforeMf([FromBody] PrintBeforeMfRequest request)
        {
            var resp = await _mediator.Send(new PrintBeforeMfCommand(request));
            return Ok(resp);
        }

        [ServiceFilter(typeof(TransactionHandleScopeFilter))]
        [HttpPost("print-mf")]
        public async Task<ActionResult<PrintMfResponse>> PrintMf([FromBody] PrintMfRequest request)
        {
            var resp = await _mediator.Send(new PrintMfCommand(request));
            return Ok(resp);
        }

        [ServiceFilter(typeof(TransactionHandleScopeFilter))]
        [HttpPost("close")]
        public async Task<ActionResult<CloseTransactionResponse>> Close([FromBody] CloseTransactionRequest request)
        {
            var resp = await _mediator.Send(new CloseTransactionCommand(request));
            return Ok(resp);
        }

        [ServiceFilter(typeof(TransactionHandleScopeFilter))]
        [HttpPost("cancel")]
        public async Task<ActionResult<CancelTransactionResponse>> Cancel([FromBody] CancelTransactionRequest request)
        {
            var resp = await _mediator.Send(new CancelTransactionCommand(request));
            return Ok(resp);
        }

        [ServiceFilter(typeof(TransactionHandleScopeFilter))]
        [HttpPost("set-departments")]
        public async Task<ActionResult<SetDepartmentsResponse>> SetDepartments([FromBody] SetDepartmentsRequest request)
        {
            var resp = await _mediator.Send(new SetDepartmentsCommand(request));
            return Ok(resp);
        }

        [HttpGet("tax-rates")]
        public async Task<ActionResult<GetTaxRatesResponse>> GetTaxRates()
        {
            var resp = await _mediator.Send(new GetTaxRatesQuery());
            return Ok(resp);
        }
    }
}
