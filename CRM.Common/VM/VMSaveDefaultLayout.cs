using CRM.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Common.VM
{
    public class VMSaveDefaultLayout
    {
        public DefaultLayout DefaultLayout { get; set; } = new DefaultLayout();

        public List<DefaultLayoutDetail> lstDefaultLayoutDetail { get; set; } = new List<DefaultLayoutDetail>();
    }
}
