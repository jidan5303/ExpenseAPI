using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Common.VM
{
    public  class VMExpenseMonthlyAll
    {
        public long  ExpenseID { get; set; }
        public string Month { get; set; }
        public string Date { get; set; }
        public string ExpenseType { get; set; }
        public string Description { get; set; }
        public string Amount { get; set; }
        public string ExpensedBy { get; set; }
    }
}
