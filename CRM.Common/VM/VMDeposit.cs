using CRM.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Common.VM
{
    public class VMDeposit
    {
        public List<Deposit> lstDeposit { get; set; } = new List<Deposit>();
        public int Balance { get; set; }
    }
}
