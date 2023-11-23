using CRM.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Common.VM
{
    public class VMDefaultLayoutIntData
    {
        public List<DefaultTable> lstDefaultTable { get; set; } = new List<DefaultTable>();
        public List<DefaultTableColumn> lstDefaultTableColumn { get; set; } = new List<DefaultTableColumn>();
        public List<DefaultLayoutDetail> lstDefaultLayoutDetail { get; set; } = new List<DefaultLayoutDetail>();
    }
}
