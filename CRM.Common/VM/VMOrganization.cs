using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Common.VM
{
    public class VMOrganization
    {
        public int OrganizationtID { get; set; }
        public string? OrganizationtName { get; set; }
        public int OfficeID { get; set; } = 0;
        public string? OfficeCode { get; set; }
        public string? OfficeLogo { get; set; }
    }
}
