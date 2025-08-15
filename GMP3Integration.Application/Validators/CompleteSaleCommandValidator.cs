using FluentValidation;
using GMP3Integration.Application.DTOs.ITransactionWorkflowService;
using GMP3Integration.Application.Features.Commands.CompleteSale;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP3Integration.Application.Validators
{
    public class CompleteSaleCommandValidator : AbstractValidator<CompleteSaleCommand>
    {
        public CompleteSaleCommandValidator(IValidator<CompleteSaleRequest> requestValidator)
        {
            RuleFor(c => c.Request)
                .NotNull().WithMessage("Request zorunludur.")
                .SetValidator(requestValidator);
        }

    }
}
