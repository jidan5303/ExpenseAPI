using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Common.Models
{
    public class TagAndDepartmentMapping
    {
        public int TagAndDepartmentMappingID { get; set; }

        public int? TagID { get; set; } = 0;

        public int? DepartmentID { get; set; } = 0;

    }
}
