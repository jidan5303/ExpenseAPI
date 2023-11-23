using CRM.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Services.Interface
{
    public interface IAuthService
    {
        string getRole(string name);
        ExpenseUser Authenticate(UserLogin userLogin);
        object CreateSession(string token, ExpenseUser user);
        ExpenseUserSession CheckSession(ExpenseUser user);
    }
}
