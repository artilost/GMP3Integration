using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP3Integration.Application.DTOs.DepertmenConfiguration
{
    public class SetDepartmentsRequest
    {
        public ulong TransactionHandle { get; set; }
        public List<DepartmentConfigItem> Departments { get; set; } = new List<DepartmentConfigItem>();
    }
}
