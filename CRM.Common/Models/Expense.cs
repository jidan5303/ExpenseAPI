using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Common.Models
{
    public class Expense : BaseClass
    {
        public long ID { get; set; }
        public int ExpenseCategory { get; set; }
        //[NotMapped]
        public int ExpenseCategoryValue { get; set; }
        public int ExpenseType { get; set; }
        public DateTime DateTime { get; set; }
        public string? Description { get; set; }
        public decimal Amount { get; set; }
        public string? ExpensedBy { get; set; }

        [NotMapped]
        public string DateTimeStr { get; set; }
        [NotMapped]
        public string FileName { get; set; }
        [NotMapped]
        public string FileUrl { get; set; }
        [NotMapped]
        public string? Base64File { get; set; }

    }
}
