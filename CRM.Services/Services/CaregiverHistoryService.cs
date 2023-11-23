using CRM.Common.Constants;
using CRM.Common.DTO;
using CRM.Common.Enums;
using CRM.Common.Helper;
using CRM.Common.Models;
using CRM.DataAccess;
using CRM.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Services
{
#pragma warning disable CS8600
    public class CaregiverHistoryService : ICaregiverHistoryService
    {
        private readonly CRMDbContext _crmDbContext;

        public CaregiverHistoryService(CRMDbContext ctx)
        {
            this._crmDbContext = ctx;
        }

        /// <summary>
        /// Get all System User
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public async Task<ResponseMessage> GetAllCaregiverHistory(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                List<CaregiverHistory> lstCaregiverHistory = new List<CaregiverHistory>();
                int totalSkip = 0;
                totalSkip = (requestMessage.PageNumber > 0) ? requestMessage.PageNumber * requestMessage.PageRecordSize : 0;

                lstCaregiverHistory = await _crmDbContext.CaregiverHistory.OrderBy(x => x.CaregiverHistoryID).Skip(totalSkip).Take(requestMessage.PageRecordSize).ToListAsync();
                responseMessage.ResponseObj = lstCaregiverHistory;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GetAllCaregiverHistory");
            }
            catch (Exception ex)
            {
                //Process excetion, Development mode show real exception and production mode will show custom exception.
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetAllCaregiverHistory");
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
        public async Task<ResponseMessage> GetCaregiverHistoryById(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                CaregiverHistory objCaregiverHistory = new CaregiverHistory();
                int CaregiverHistoryID = JsonConvert.DeserializeObject<int>(requestMessage?.RequestObj.ToString());

                objCaregiverHistory = await _crmDbContext.CaregiverHistory.FirstOrDefaultAsync(x => x.CaregiverHistoryID == CaregiverHistoryID && x.Status == (int)Enums.Status.Active);
                responseMessage.ResponseObj = objCaregiverHistory;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GetCaregiverHistoryById");
            }
            catch (Exception ex)
            {
                //Process excetion, Development mode show real exception and production mode will show custom exception.
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "");
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
        public async Task<ResponseMessage> SaveCaregiverHistory(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            int actionType = (int)Enums.ActionType.Insert;
            try
            {

                CaregiverHistory objCaregiverHistory = JsonConvert.DeserializeObject<CaregiverHistory>(requestMessage?.RequestObj.ToString());



                if (objCaregiverHistory != null)
                {
                    if (CheckedValidation(objCaregiverHistory, responseMessage))
                    {
                        if (objCaregiverHistory.CaregiverHistoryID > 0)
                        {
                            CaregiverHistory existingCaregiverHistory = await this._crmDbContext.CaregiverHistory.AsNoTracking().FirstOrDefaultAsync(x => x.CaregiverHistoryID == objCaregiverHistory.CaregiverHistoryID && x.Status == (int)Enums.Status.Active);
                            if (existingCaregiverHistory != null)
                            {
                                actionType = (int)Enums.ActionType.Update;
                                objCaregiverHistory.CreatedDate = existingCaregiverHistory.CreatedDate;
                                objCaregiverHistory.CreatedBy = existingCaregiverHistory.CreatedBy;
                                objCaregiverHistory.UpdatedDate = DateTime.Now;
                                objCaregiverHistory.UpdatedBy = requestMessage.UserID;
                                _crmDbContext.CaregiverHistory.Update(objCaregiverHistory);
                            }
                        }
                        else
                        {
                            objCaregiverHistory.Status = (int)Enums.Status.Active;
                            objCaregiverHistory.CreatedDate = DateTime.Now;
                            objCaregiverHistory.CreatedBy = requestMessage.UserID;
                            await _crmDbContext.CaregiverHistory.AddAsync(objCaregiverHistory);

                        }
                        await _crmDbContext.SaveChangesAsync();

                        responseMessage.ResponseObj = objCaregiverHistory;
                        responseMessage.Message = MessageConstant.SavedSuccessfully;
                        responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                        //Log write
                        LogHelper.WriteLog(requestMessage.RequestObj, actionType, requestMessage.UserID, "SaveCaregiverHistory");

                    }
                    else
                    {
                        responseMessage.ResponseCode = (int)Enums.ResponseCode.Warning;
                    }
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
                responseMessage.Message = ExceptionHelper.ProcessException(ex, actionType, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "SaveCaregiverHistory");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;

            }


            return responseMessage;
        }
        /// <summary>
        /// validation check
        /// </summary>
        /// <param name="objCaregiverHistory"></param>
        /// <returns></returns>
        private bool CheckedValidation(CaregiverHistory objCaregiverHistory, ResponseMessage responseMessage)
        {


            return true;
        }
#pragma warning restore CS8600

    }


}
