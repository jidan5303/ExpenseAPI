using CRM.Common.Models;
using CRM.DataAccess;
using CRM.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Services.Services
{
    public class AuthService : IAuthService
    {
        public readonly CRMDbContext _crmDbContext;
        public AuthService(CRMDbContext cRMDb) { 
            _crmDbContext = cRMDb;
        }
        public ExpenseUser Authenticate(UserLogin userLogin)
        {
            var currentUser = _crmDbContext.ExpenseUser.FirstOrDefault(u => u.UserName == userLogin.username && u.Password == userLogin.password);
            if (currentUser != null)
            {
                return currentUser;
            }
            return null;
        }

        public ExpenseUserSession CheckSession(ExpenseUser user)
        {
            var session = _crmDbContext.ExpenseUserSession.FirstOrDefault(s => s.UserID == user.UserID
            && s.Active == 1);
            if (session != null)
            {
                session.Active = 0;
                _crmDbContext.SaveChanges();
                return session;
            }
            return null;
        }

        public object CreateSession(string token, ExpenseUser user)
        {
            var session = new ExpenseUserSession
            {
                Token = token,
                Active = 1,
                UserID = user.UserID
            };
            _crmDbContext.ExpenseUserSession.Add(session);
            _crmDbContext.SaveChanges();
            return null;
        }

        public string getRole(string name)
        {
            var user = _crmDbContext.ExpenseUser.FirstOrDefault(u => u.UserName == name);
            if (user != null)
            {
                var role = user.Role;
                return role;
            }
            return null;
        }
    }
}

