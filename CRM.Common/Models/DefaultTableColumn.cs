using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Common.Models
{
    public class DefaultTableColumn:BaseClass
    {
        public int DefaultTableColumnID { get; set; }

        public int? DefaultTableID { get; set; } = 0;

        public string? ColumnName { get; set; }

    }
}
