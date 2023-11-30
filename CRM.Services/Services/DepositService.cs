using CRM.Common.DTO;
using CRM.Common.Enums;
using CRM.Common.Helper;
using CRM.Common.Models;
using CRM.Common.VM;
using CRM.DataAccess;
using CRM.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Services.Services
{
    public class DepositService : IDepositService
    {
        private readonly CRMDbContext _crmDbContext;
        public DepositService(CRMDbContext crmDbContext)
        {

            _crmDbContext = crmDbContext;

        }
        public async Task<ResponseMessage> GetAllDeposit(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                VMDeposit vMDeposit = new VMDeposit();
                int balance = 0;
                List<Deposit> lstDeposit = await _crmDbContext.Deposit.Where(x => x.Status == (int)Enums.Status.Active).ToListAsync();
                //foreach (Deposit deposit in lstDeposit)
                //{
                //    balance += deposit.DepositAmount;
                //}

                //vMDeposit.Balance = balance;
                //vMDeposit.lstDeposit = lstDeposit;

                responseMessage.ResponseObj = lstDeposit;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GetAllDeposit");
            }
            catch (Exception ex)
            {
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View,
                    requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetAllDeposit");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }
            return responseMessage;
        }

        public async Task<ResponseMessage> DeleteDeposit(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                int depositID = JsonConvert.DeserializeObject<int>(requestMessage?.RequestObj.ToString());

                string sqlUpdateQuery = $"UPDATE Deposit SET Status = {(int)Enums.Status.Delete} WHERE ID = {depositID} AND Status = {(int)Enums.Status.Active};";
                await _crmDbContext.Database.ExecuteSqlRawAsync(sqlUpdateQuery);

                var objDeposit = await _crmDbContext.Deposit.AsNoTracking().FirstOrDefaultAsync(x => x.DepositID == depositID);

                if (objDeposit != null)
                {
                    responseMessage.ResponseObj = objDeposit;
                    responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
                }
                else
                {
                    responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
                }
            }
            catch (Exception ex)
            {
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }
            return responseMessage;
        }

        public async Task<ResponseMessage> SaveDeposit(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                Deposit objDeposit = JsonConvert.DeserializeObject<Deposit>(requestMessage.RequestObj.ToString());
                if(objDeposit != null && objDeposit.DepositID > 0)
                {
                    Deposit existDeposit = await _crmDbContext.Deposit.AsNoTracking().Where(d => d.DepositID == objDeposit.DepositID).FirstOrDefaultAsync();
                    if (existDeposit != null)
                    {
                        objDeposit.UpdatedBy = requestMessage.UserID;
                        objDeposit.UpdatedDate = DateTime.Now;
                        objDeposit.CreatedDate = existDeposit.CreatedDate;
                        objDeposit.CreatedBy = existDeposit.CreatedBy;
                        objDeposit.Status = existDeposit.Status;

                        _crmDbContext.Update(objDeposit);
                    }
                }
                else
                {
                    //Deposit existLastDeposit = await _crmDbContext.Deposit.AsNoTracking().OrderBy(x => x.DepositID).LastOrDefaultAsync();
                    objDeposit.CreatedDate = DateTime.Now;
                    objDeposit.CreatedBy = requestMessage.UserID;
                    objDeposit.Status = (int)Enums.Status.Active;

                    await _crmDbContext.Deposit.AddAsync(objDeposit);
                }
                await _crmDbContext.SaveChangesAsync();

                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.Insert, requestMessage.UserID, "SaveDeposit");
            }
            catch (Exception ex)
            {
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View,
                    requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "SaveDeposit");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }
            return responseMessage;
        }

        public async Task<ResponseMessage> GetBalance(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();

            try
            {
                List<Deposit> lstDeposit = await _crmDbContext.Deposit.Where(x => x.Status == (int)Enums.Status.Active).ToListAsync();
                List<Expense> lstExpense = await _crmDbContext.Expense.Where(x => x.Status == (int)Enums.Status.Active).ToListAsync();

                decimal totalDepositAmount = 0;
                decimal totalExpenseAmount = 0;

                foreach(Deposit deposit in lstDeposit)
                {
                    totalDepositAmount += deposit.DepositAmount;
                }
                foreach(Expense expense in lstExpense)
                {
                    totalExpenseAmount += expense.Amount;
                }

                decimal balance = totalDepositAmount - totalExpenseAmount;


                responseMessage.ResponseObj = balance;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GetBalance");
            }
            catch (Exception ex)
            {
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View,
                    requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetBalance");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }
            return responseMessage;
        }
    }
}
