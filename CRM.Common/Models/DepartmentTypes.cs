using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Common.Models
{
    public class DepartmentTypes
    {
        public int DepartmentTypeID { get; set; }
        public string? DepartmentTypeName { get; set; }
        public int Sequence { get; set; } = 0;      
    }
}
