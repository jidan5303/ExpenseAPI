using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Common.Models
{
    public class Organization: BaseClass
    {
        public int OrganizationID { get; set; }
        public string? OrganizationName { get; set; }
        public int OfficeID { get; set; } = 0;
        public string? OfficeCode { get; set; }
        public string? OfficeLogo { get; set; }
    }
}
