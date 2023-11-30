using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Common.Models
{
    public class Deposit : BaseClass
    {
        public int DepositID { get; set; }
        public decimal DepositAmount { get; set; }
        public string? Description { get; set; }
        public DateTime? DepositDate { get; set; }
    }
}
