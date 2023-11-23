using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Services.Interface
{
    public interface IExpensePermissionService
    {
        string Validate(string url, string role);
    }
}
