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
    public class KnowledgeBaseHistoryService : IKnowledgeBaseHistoryService
    {
        private readonly CRMDbContext _crmDbContext;

        public KnowledgeBaseHistoryService(CRMDbContext ctx)
        {
            this._crmDbContext = ctx;
        }

        /// <summary>
        /// Get All Knowledge Base History
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public async Task<ResponseMessage> GetAllKnowledgeBaseHistory(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                List<KnowledgeBaseHistory> lstKnowledgeBaseHistory = new List<KnowledgeBaseHistory>();
                int totalSkip = 0;
                totalSkip = (requestMessage.PageNumber > 0) ? requestMessage.PageNumber * requestMessage.PageRecordSize : 0;

                lstKnowledgeBaseHistory = await _crmDbContext.KnowledgeBaseHistory.OrderBy(x => x.KnowledgeBaseHistoryID).Skip(totalSkip).Take(requestMessage.PageRecordSize).ToListAsync();
                responseMessage.ResponseObj = lstKnowledgeBaseHistory;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View,requestMessage.UserID, "GetAllKnowledgeBaseHistory");
            }
            catch (Exception ex)
            {
                //Process excetion, Development mode show real exception and production mode will show custom exception.
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID,JsonConvert.SerializeObject(requestMessage.RequestObj), "GetAllKnowledgeBaseHistory");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

        /// <summary>
        ///  Get Knowledge Base History By Id
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseMessage> GetKnowledgeBaseHistoryById(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                KnowledgeBaseHistory objKnowledgeBaseHistory = new KnowledgeBaseHistory();
                int KnowledgeBaseHistoryID = JsonConvert.DeserializeObject<int>(requestMessage?.RequestObj.ToString());

                objKnowledgeBaseHistory = await _crmDbContext.KnowledgeBaseHistory.FirstOrDefaultAsync(x => x.KnowledgeBaseHistoryID == KnowledgeBaseHistoryID && x.Status == (int)Enums.Status.Active);
                responseMessage.ResponseObj = objKnowledgeBaseHistory;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View,requestMessage.UserID, "GetKnowledgeBaseHistoryById");
            }
            catch (Exception ex)
            {
                //Process excetion, Development mode show real exception and production mode will show custom exception.
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID,JsonConvert.SerializeObject(requestMessage.RequestObj), "GetKnowledgeBaseHistoryById");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

        /// <summary>
        /// Save and update Knowledge Base History
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<ResponseMessage> SaveKnowledgeBaseHistory(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            int actionType = (int)Enums.ActionType.Insert;
            try
            {

                KnowledgeBaseHistory objKnowledgeBaseHistory = JsonConvert.DeserializeObject<KnowledgeBaseHistory>(requestMessage?.RequestObj.ToString());

               

                if (objKnowledgeBaseHistory != null)
                {
                    if (CheckedValidation(objKnowledgeBaseHistory, responseMessage))
                    {
                        if (objKnowledgeBaseHistory.KnowledgeBaseHistoryID > 0)
                        {
                            KnowledgeBaseHistory existingKnowledgeBaseHistory = await this._crmDbContext.KnowledgeBaseHistory.AsNoTracking().FirstOrDefaultAsync(x => x.KnowledgeBaseHistoryID == objKnowledgeBaseHistory.KnowledgeBaseHistoryID && x.Status == (int)Enums.Status.Active);
                            if (existingKnowledgeBaseHistory != null)
                            {
                                actionType = (int)Enums.ActionType.Update;
                                objKnowledgeBaseHistory.CreatedDate = existingKnowledgeBaseHistory.CreatedDate;
                                objKnowledgeBaseHistory.CreatedBy = existingKnowledgeBaseHistory.CreatedBy;
                                objKnowledgeBaseHistory.UpdatedDate = DateTime.Now;
                                objKnowledgeBaseHistory.UpdatedBy = requestMessage.UserID;
                                _crmDbContext.KnowledgeBaseHistory.Update(objKnowledgeBaseHistory);

                            }
                        }
                        else
                        {
                            objKnowledgeBaseHistory.Status = (int)Enums.Status.Active;
                            objKnowledgeBaseHistory.CreatedDate = DateTime.Now;
                            objKnowledgeBaseHistory.CreatedBy = requestMessage.UserID;
                           await _crmDbContext.KnowledgeBaseHistory.AddAsync(objKnowledgeBaseHistory);
                            
                        }
                        await _crmDbContext.SaveChangesAsync();

                        responseMessage.ResponseObj = objKnowledgeBaseHistory;
                        responseMessage.Message = MessageConstant.SavedSuccessfully;
                        responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                        //Log write
                        LogHelper.WriteLog(requestMessage.RequestObj, actionType,requestMessage.UserID, "SaveKnowledgeBaseHistory");

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
                //Process excetion, Development mode show real exception and production mode will show custom exception.
                responseMessage.Message = ExceptionHelper.ProcessException(ex, actionType, requestMessage.UserID,JsonConvert.SerializeObject(requestMessage.RequestObj), "SaveKnowledgeBaseHistory");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;

            }


            return responseMessage;
        }
        /// <summary>
        /// validation check
        /// </summary>
        /// <param name="objKnowledgeBaseHistory"></param>
        /// <returns></returns>
        private bool CheckedValidation(KnowledgeBaseHistory objKnowledgeBaseHistory, ResponseMessage responseMessage)
        {
            if (string.IsNullOrEmpty(objKnowledgeBaseHistory.KnowledgeDetail))
            {
                responseMessage.Message = MessageConstant.KnowledgeDetail;
                return false;
            }
            if (objKnowledgeBaseHistory.KnowledgeBaseID <= 0)
            {
                responseMessage.Message = MessageConstant.KnowledgeBase;
                return false;
            }

            return true;
        }
#pragma warning restore CS8600

    }

    
}
