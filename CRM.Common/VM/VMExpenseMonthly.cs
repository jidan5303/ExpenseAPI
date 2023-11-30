using CRM.Common.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Common.VM
{
    public class VMExpenseMonthly
    {
        public long ExpenseID { get; set; }
        public int Month { get; set; }
        public DateTime Date { get; set; }
        public string ExpenseType { get; set; }
        public string? Description { get; set; }
        public decimal Amount { get; set; }
        public string? ExpensedBy { get; set; }
        [NotMapped]
        public ExpenseAttachment ExpenseAttachment { get; set; } = new ExpenseAttachment();
    }
}
