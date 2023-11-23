using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Common.Models
{
    public class Tag:BaseClass
    {
        public int TagID { get; set; }

        public string? TagTitle { get; set; }

        public string? Color { get; set; }

        [NotMapped]
        public List<Department> lstDepartment { get; set; } = new List<Department> ();

    }
}
