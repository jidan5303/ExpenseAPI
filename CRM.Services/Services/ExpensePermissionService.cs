using CRM.DataAccess;
using CRM.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Services.Services
{
    public class ExpensePermissionService : IExpensePermissionService
    {
        private readonly CRMDbContext _context;
        public ExpensePermissionService(CRMDbContext context)
        {
            _context = context;
        }
        public string Validate(string url, string role)
        {
            var permission = _context.ExpensePermission.FirstOrDefault(p => p.Role == role && p.RequestURL == url);
            if (permission != null)
            {
                return url;
            }
            return null;
        }
    }
}
