using GMP3Integration.Application.DTOs.PrintTotalsAndPayments;
using MediatR;

namespace GMP3Integration.Application.Features.Commands.PrintTotalsAndPayments
{
    public class PrintTotalsAndPaymentsCommand : IRequest<PrintTotalsAndPaymentsResponse>
    {
        public PrintTotalsAndPaymentsRequest Request { get; private set; }
        public PrintTotalsAndPaymentsCommand(PrintTotalsAndPaymentsRequest request)
        {
            Request = request;
        }
    }
}
