using CRM.Common.Helper.AuditLog;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Common.Helper
{
    public static class ExceptionHelper
    {

        public static string ProcessException(Exception ex, int actionType, int userId, string objJson = "", string methodName = "")
        {
            ExceptionLog objExceptionLog = new ExceptionLog();
            try
            {
                objExceptionLog.ExceptionMessage = ex.Message.ToString();
                objExceptionLog.InputObject = objJson;
                objExceptionLog.UserId = userId;
                objExceptionLog.ActionTypeID = actionType;
                objExceptionLog.MethodName = methodName;
                objExceptionLog.CreatedDate = DateTime.Now;
                using (var crmAuditLogDbContext = new CRMAuditLogDbContext())
                {
                    crmAuditLogDbContext.Add<ExceptionLog>(objExceptionLog);
                    crmAuditLogDbContext.SaveChanges();
                }
            }
            catch
            {

            }

            return ex.Message.ToString();
        }

    }
   
}
