
using CRM.Common.Helper.AuditLog;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Common.Helper
{
    public static class LogHelper
    {

        public static void WriteLog(object obj, int actionType,int userId, string? methodName = "")
        {
            AuditLogMain  objAuditLogMain = new AuditLogMain();
            try
            {
                objAuditLogMain.InputObject = JsonConvert.SerializeObject(obj);
                objAuditLogMain.UserId = userId;
                objAuditLogMain.ActionTypeID = actionType;
                objAuditLogMain.MethodName = methodName;
                objAuditLogMain.CreatedDate = DateTime.Now;

                using (var crmAuditLogDbContext = new CRMAuditLogDbContext())
                {
                    crmAuditLogDbContext.Add<AuditLogMain>(objAuditLogMain);
                    crmAuditLogDbContext.SaveChanges();
                }

            }
            catch 
            {

                
            }
           
        }
    }
}
