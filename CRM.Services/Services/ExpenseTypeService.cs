using CRM.Common.Constants;
using CRM.Common.DTO;
using CRM.Common.Enums;
using CRM.Common.Helper;
using CRM.Common.Models;
using CRM.DataAccess;
using CRM.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CRM.Services
{
    public class ExpenseTypeService : IExpenseTypeService
    {
        private readonly CRMDbContext _crmDbContext;

        public ExpenseTypeService(CRMDbContext ctx)
        {
            this._crmDbContext = ctx;
        }

        /// <summary>
        /// Get all System User
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public async Task<ResponseMessage> GetAllExpenseType(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                List<ExpenseType> lstExpenseType = new List<ExpenseType>();
                int totalSkip = 0;
                totalSkip = (requestMessage.PageNumber > 0) ? requestMessage.PageNumber * requestMessage.PageRecordSize : 0;

                lstExpenseType = await _crmDbContext.ExpenseType.OrderBy(x => x.ID).Skip(totalSkip).Take(requestMessage.PageRecordSize).ToListAsync();
                responseMessage.ResponseObj = lstExpenseType;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GetAllExpenseType");
            }
            catch (Exception ex)
            {
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View,
                    requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetAllExpenseType");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

        public async Task<ResponseMessage> GetExpenseTypeByExpenseCategoryId(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                List<ExpenseType> lstExpenseType = new List<ExpenseType>();
                ExpenseType expenseType = JsonConvert.DeserializeObject<ExpenseType>(requestMessage?.RequestObj.ToString());

                //lstExpenseType = await _crmDbContext.ExpenseType.Where(x => x.ExpenseCategoryID == expenseType.ExpenseCategoryID
                //&& x.Status == (int)Enums.Status.Active).ToListAsync();
                string sql = @"select * from ExpenseType where ExpenseCategoryID = " + expenseType.ExpenseCategoryID;
                lstExpenseType = await _crmDbContext.ExpenseType.FromSqlRaw(sql).ToListAsync();

                responseMessage.ResponseObj = lstExpenseType;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GetExpenseTypeByExpenseCategoryId");
            }
            catch (Exception ex)
            {
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View,
                    requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetExpenseTypeByExpenseCategoryId");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseMessage> GetExpenseTypeById(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                ExpenseType objExpenseType = new ExpenseType();
                int expenseTypeID = JsonConvert.DeserializeObject<int>(requestMessage?.RequestObj.ToString());

                objExpenseType = await _crmDbContext.ExpenseType.AsNoTracking().FirstOrDefaultAsync(x => x.ID == expenseTypeID && x.Status == (int)Enums.Status.Active);
                responseMessage.ResponseObj = objExpenseType;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GetExpenseTypeById");
            }
            catch (Exception ex)
            {
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View,
                    requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetAllExpenseType");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

        /// <summary>
        /// Save and update System user
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<ResponseMessage> SaveExpenseType(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {

                ExpenseType objExpenseType = JsonConvert.DeserializeObject<ExpenseType>(requestMessage?.RequestObj.ToString());

                int actionType = (int)Enums.ActionType.Insert;

                if (objExpenseType != null)
                {
                    if (CheckedValidation(objExpenseType, responseMessage))
                    {
                        if (objExpenseType.ID > 0)
                        {
                            //Upate Mode
                            ExpenseType existingExpenseType = await this._crmDbContext.ExpenseType.FirstOrDefaultAsync(x => x.ID == objExpenseType.ID && x.Status == (int)Enums.Status.Active);
                            if (existingExpenseType != null)
                            {
                                actionType = (int)Enums.ActionType.Update;
                                objExpenseType.CreatedDate = existingExpenseType.CreatedDate;
                                objExpenseType.CreatedBy = existingExpenseType.CreatedBy;
                                objExpenseType.UpdatedDate = DateTime.Now;
                                objExpenseType.UpdatedBy = requestMessage.UserID;
                                _crmDbContext.ExpenseType.Update(objExpenseType);

                                responseMessage.ResponseObj = objExpenseType;
                            }
                        }
                        else
                        {
                            //Insert Mode
                            objExpenseType.Status = (int)Enums.Status.Active;
                            objExpenseType.CreatedDate = DateTime.Now;
                            objExpenseType.CreatedBy = requestMessage.UserID;
                            var res = await _crmDbContext.ExpenseType.AddAsync(objExpenseType);
                            responseMessage.ResponseObj = res.Entity;
                        }
                    }
                    await _crmDbContext.SaveChangesAsync();
                    responseMessage.Message = MessageConstant.SavedSuccessfully;
                    responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                    //Log write
                    LogHelper.WriteLog(requestMessage.RequestObj, actionType, requestMessage.UserID, "SaveExpenseType");

                }
                else
                {
                    responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
                    responseMessage.Message = MessageConstant.SaveFailed;
                }

            }
            catch (Exception ex)
            {
                //Exception write
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View,
                    requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetAllExpenseType");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;

            }


            return responseMessage;
        }
        /// <summary>
        /// validation check
        /// </summary>
        /// <param name="objExpenseType"></param>
        /// <returns></returns>
        private bool CheckedValidation(ExpenseType objExpenseType, ResponseMessage responseMessage)
        {
            if (string.IsNullOrEmpty(objExpenseType.Name))
            {
                responseMessage.Message = MessageConstant.Name_Is_Required;
                return false;
            }

            ExpenseType existingExpenseType = new ExpenseType();
            existingExpenseType = _crmDbContext.ExpenseType.Where(x => x.Name == objExpenseType.Name && x.ID != objExpenseType.ID && x.Status == (int)Enums.Status.Active).AsNoTracking().FirstOrDefault();
            if (existingExpenseType != null)
            {
                responseMessage.Message = MessageConstant.Duplicate_Name;
                return false;
            }

            return true;
        }
    }
}

