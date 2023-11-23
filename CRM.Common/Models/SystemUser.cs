using CRM.Common.VM;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Common.Models
{
    public class SystemUser : BaseClass
    {
        public int SystemUserID { get; set; }

        public string UserName { get; set; } = "";

        public string? FullName { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }
        [NotMapped]
        public string? ConfirmPassword { get; set; }

        public int Role { get; set; } = 0;

        public int TaskCapacity { get; set; } = 0;

        public string? Supervisor { get; set; }

        public bool IsSuperAdmin { get; set; }

        [NotMapped]
        public List<Department> lstDepartment { get; set; } = new List<Department>();

        [NotMapped]
        public List<Organization> lstOrganization { get; set; } = new List<Organization>();

    }

}
