using FluentValidation;
using GMP3Integration.Application.DTOs.ITransactionWorkflowService;
using GMP3Integration.Application.DTOs.ITransactionWorkflowService.Inputs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP3Integration.Application.Validators
{
    public class CompleteSaleRequestValidator : AbstractValidator<CompleteSaleRequest>
    {
        public CompleteSaleRequestValidator()
        {
            RuleFor(x => x.CurrentInterface).NotEmpty().WithMessage("currentInterface zorunludur.");
            RuleFor(x => x.Payment).NotNull().WithMessage("payment zorunludur.");
            RuleFor(x => x.Items).NotEmpty().WithMessage("en az bir item zorunludur.");

            RuleForEach(x => x.Items).SetValidator(new ItemSaleInputValidator());
        }
        public class ItemSaleInputValidator : AbstractValidator<WorkflowItem>
        {
            public ItemSaleInputValidator()
            {
                RuleFor(i => i.DeptIndex).GreaterThanOrEqualTo(0).WithMessage("items[].deptIndex >= 0 olmalı.");
                RuleFor(i => i.Amount).GreaterThan(0).WithMessage("items[].amount > 0 olmalı (kuruş).");
                RuleFor(i => i.CurrencyCode).Equal(949).WithMessage("items[].currencyCode 949 (TRY) olmalı.");
                RuleFor(i => i.Count).GreaterThan(0);
                RuleFor(i => i.UnitType).GreaterThan(0);
            }
        }
    }
}
