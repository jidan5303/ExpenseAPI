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
    public class ExpenseAttachmentService : IExpenseAttachmentService
    {
        private readonly CRMDbContext _crmDbContext;

        public ExpenseAttachmentService(CRMDbContext ctx)
        {
            this._crmDbContext = ctx;
        }
        public async Task<ResponseMessage> SaveExpenseAttachment(ExpenseAttachment objExpenseAttachment, int loggedInUserID)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                int actionType = (int)Enums.ActionType.Insert;

                if (objExpenseAttachment != null)
                {
                    if (CheckedValidation(objExpenseAttachment, responseMessage))
                    {
                        if (objExpenseAttachment.ID > 0)
                        {
                            //Upate Mode
                            ExpenseAttachment existingExpenseAttachment = await this._crmDbContext.ExpenseAttachment.FirstOrDefaultAsync(x => x.ID == objExpenseAttachment.ID && x.Status == (int)Enums.Status.Active);
                            if (existingExpenseAttachment != null)
                            {
                                actionType = (int)Enums.ActionType.Update;
                                objExpenseAttachment.CreatedDate = existingExpenseAttachment.CreatedDate;
                                objExpenseAttachment.CreatedBy = existingExpenseAttachment.CreatedBy;
                                objExpenseAttachment.UpdatedDate = DateTime.Now;
                                objExpenseAttachment.UpdatedBy = loggedInUserID;
                                _crmDbContext.ExpenseAttachment.Update(objExpenseAttachment);

                                responseMessage.ResponseObj = objExpenseAttachment;
                            }
                        }
                        else
                        {
                            //Insert Mode
                            objExpenseAttachment.Status = (int)Enums.Status.Active;
                            objExpenseAttachment.CreatedDate = DateTime.Now;
                            objExpenseAttachment.CreatedBy = loggedInUserID;
                            var res = await _crmDbContext.ExpenseAttachment.AddAsync(objExpenseAttachment);
                            responseMessage.ResponseObj = res.Entity;
                        }
                    }
                    await _crmDbContext.SaveChangesAsync();
                    responseMessage.Message = MessageConstant.SavedSuccessfully;
                    responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                    //Log write
                    LogHelper.WriteLog(objExpenseAttachment, actionType, loggedInUserID, "SaveExpenseAttachment");

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
                    loggedInUserID, JsonConvert.SerializeObject(objExpenseAttachment), "SaveExpenseAttachment");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

        private bool CheckedValidation(ExpenseAttachment expenseAttachment, ResponseMessage responseMessage)
        {
            return true;
        }
    }
}

