using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP3Integration.Application.DTOs.ITransactionWorkflowService.Inputs
{
    public class WorkflowTaxRate
    {
        public int TaxCode { get; set; }     // string değil: int
        public decimal Rate { get; set; }
    }
}
