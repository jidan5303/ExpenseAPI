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
#pragma warning disable CS8600
    public class ExpenseCategoryService : IExpenseCategoryService
    {
        private readonly CRMDbContext _crmDbContext;

        public ExpenseCategoryService(CRMDbContext ctx)
        {
            this._crmDbContext = ctx;
        }

        /// <summary>
        /// Get all System User
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public async Task<ResponseMessage> GetAllExpenseCategory(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                List<ExpenseCategory> lstExpenseCategory = new List<ExpenseCategory>();
                int totalSkip = 0;
                totalSkip = (requestMessage.PageNumber > 0) ? requestMessage.PageNumber * requestMessage.PageRecordSize : 0;

                lstExpenseCategory = await _crmDbContext.ExpenseCategory.OrderBy(x => x.ID).Skip(totalSkip).Take(requestMessage.PageRecordSize).ToListAsync();
                responseMessage.ResponseObj = lstExpenseCategory;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GetAllExpenseCategory");
            }
            catch (Exception ex)
            {
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, 
                    requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetAllExpenseCategory");
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
        public async Task<ResponseMessage> GetExpenseCategoryById(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                ExpenseCategory objExpenseCategory = new ExpenseCategory();
                int expenseCategoryID = JsonConvert.DeserializeObject<int>(requestMessage?.RequestObj.ToString());

                objExpenseCategory = await _crmDbContext.ExpenseCategory.AsNoTracking().FirstOrDefaultAsync(x => x.ID == expenseCategoryID && x.Status == (int)Enums.Status.Active);
                responseMessage.ResponseObj = objExpenseCategory;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GetExpenseCategoryById");
            }
            catch (Exception ex)
            {
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View,
                    requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetExpenseCategoryById");
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
        public async Task<ResponseMessage> SaveExpenseCategory(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                ExpenseCategory objExpenseCategory = JsonConvert.DeserializeObject<ExpenseCategory>(requestMessage?.RequestObj.ToString());

                int actionType = (int)Enums.ActionType.Insert;

                if (objExpenseCategory != null)
                {
                    if (CheckedValidation(objExpenseCategory, responseMessage))
                    {
                        if (objExpenseCategory.ID > 0)
                        {
                            //Update Mode
                            ExpenseCategory existingExpenseCategory = await this._crmDbContext.ExpenseCategory.FirstOrDefaultAsync(x => x.ID == objExpenseCategory.ID && x.Status == (int)Enums.Status.Active);
                            if (existingExpenseCategory != null)
                            {
                                actionType = (int)Enums.ActionType.Update;
                                objExpenseCategory.CreatedDate = existingExpenseCategory.CreatedDate;
                                objExpenseCategory.CreatedBy = existingExpenseCategory.CreatedBy;
                                objExpenseCategory.UpdatedDate = DateTime.Now;
                                objExpenseCategory.UpdatedBy = requestMessage.UserID;
                                _crmDbContext.ExpenseCategory.Update(objExpenseCategory);

                                responseMessage.ResponseObj = objExpenseCategory;
                            }
                        }
                        else
                        {
                            //Insert Mode
                            objExpenseCategory.Status = (int)Enums.Status.Active;
                            objExpenseCategory.CreatedDate = DateTime.Now;
                            objExpenseCategory.CreatedBy = requestMessage.UserID;
                            var res = await _crmDbContext.ExpenseCategory.AddAsync(objExpenseCategory);
                            responseMessage.ResponseObj = res.Entity;
                        }
                    }
                    await _crmDbContext.SaveChangesAsync();
                    responseMessage.Message = MessageConstant.SavedSuccessfully;
                    responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                    //Log write
                    LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "SaveExpenseCategory");
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
                   requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "SaveExpenseCategory");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }
        /// <summary>
        /// validation check
        /// </summary>
        /// <param name="objExpenseCategory"></param>
        /// <returns></returns>
        private bool CheckedValidation(ExpenseCategory objExpenseCategory, ResponseMessage responseMessage)
        {
            if (string.IsNullOrEmpty(objExpenseCategory.Name))
            {
                responseMessage.Message = MessageConstant.Name_Is_Required;
                return false;
            }

            ExpenseCategory existingExpenseCategory = new ExpenseCategory();
            existingExpenseCategory = _crmDbContext.ExpenseCategory.Where(x => x.Name == objExpenseCategory.Name && x.ID != objExpenseCategory.ID && x.Status == (int)Enums.Status.Active).AsNoTracking().FirstOrDefault();
            if (existingExpenseCategory != null)
            {
                responseMessage.Message = MessageConstant.Duplicate_Name;
                return false;
            }

            return true;
        }
#pragma warning restore CS8600

    }
}

