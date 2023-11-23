using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Common.Models
{
    public class SystemUserOrganizationMapping
    {
        public int SystemUserOrganizationMappingID { get; set; }

        public int? OrganizationID { get; set; } = 0;

        public int? SystemUserID { get; set; } = 0;

    }
}
