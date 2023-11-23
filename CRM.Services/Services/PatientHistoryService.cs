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
    public class PatientHistoryService : IPatientHistoryService
    {
        private readonly CRMDbContext _crmDbContext;

        public PatientHistoryService(CRMDbContext ctx)
        {
            this._crmDbContext = ctx;
        }

        /// <summary>
        /// Get all patient history
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public async Task<ResponseMessage> GetAllPatientHistory(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                List<PatientHistory> lstPatientHistory = new List<PatientHistory>();
                int totalSkip = 0;
                totalSkip = (requestMessage.PageNumber > 0) ? requestMessage.PageNumber * requestMessage.PageRecordSize : 0;

                lstPatientHistory = await _crmDbContext.PatientHistory.OrderBy(x => x.PatientHistoryID).Skip(totalSkip).Take(requestMessage.PageRecordSize).ToListAsync();
                responseMessage.ResponseObj = lstPatientHistory;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GetAllPatientHistory");
            }
            catch (Exception ex)
            {
                //Process excetion, Development mode show real exception and production mode will show custom exception.
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetAllPatientHistory");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

        /// <summary>
        /// Get all patient history by id.
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseMessage> GetPatientHistoryById(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                PatientHistory objPatientHistory = new PatientHistory();
                int PatientHistoryID = JsonConvert.DeserializeObject<int>(requestMessage?.RequestObj.ToString());

                objPatientHistory = await _crmDbContext.PatientHistory.FirstOrDefaultAsync(x => x.PatientHistoryID == PatientHistoryID && x.Status == (int)Enums.Status.Active);
                responseMessage.ResponseObj = objPatientHistory;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GetPatientHistoryById");
            }
            catch (Exception ex)
            {
                //Process excetion, Development mode show real exception and production mode will show custom exception.
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetPatientHistoryById");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

        /// <summary>
        /// Save and update patient history
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<ResponseMessage> SavePatientHistory(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            int actionType = (int)Enums.ActionType.Insert;
            try
            {

                PatientHistory objPatientHistory = JsonConvert.DeserializeObject<PatientHistory>(requestMessage?.RequestObj.ToString());



                if (objPatientHistory != null)
                {
                    if (CheckedValidation(objPatientHistory, responseMessage))
                    {
                        if (objPatientHistory.PatientHistoryID > 0)
                        {
                            PatientHistory existingPatientHistory = await this._crmDbContext.PatientHistory.AsNoTracking().FirstOrDefaultAsync(x => x.PatientHistoryID == objPatientHistory.PatientHistoryID && x.Status == (int)Enums.Status.Active);
                            if (existingPatientHistory != null)
                            {
                                actionType = (int)Enums.ActionType.Update;
                                objPatientHistory.CreatedDate = existingPatientHistory.CreatedDate;
                                objPatientHistory.CreatedBy = existingPatientHistory.CreatedBy;
                                objPatientHistory.UpdatedDate = DateTime.Now;
                                objPatientHistory.UpdatedBy = requestMessage.UserID;
                                _crmDbContext.PatientHistory.Update(objPatientHistory);
                            }
                        }
                        else
                        {
                            objPatientHistory.Status = (int)Enums.Status.Active;
                            objPatientHistory.CreatedDate = DateTime.Now;
                            objPatientHistory.CreatedBy = requestMessage.UserID;
                           await _crmDbContext.PatientHistory.AddAsync(objPatientHistory);
                            
                        }
                        await _crmDbContext.SaveChangesAsync();

                        responseMessage.ResponseObj = objPatientHistory;
                        responseMessage.Message = MessageConstant.SavedSuccessfully;
                        responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                        //Log write
                        LogHelper.WriteLog(requestMessage.RequestObj, actionType, requestMessage.UserID, "SavePatientHistory");

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
                responseMessage.Message = ExceptionHelper.ProcessException(ex, actionType, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "SavePatientHistory");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;

            }


            return responseMessage;
        }
        /// <summary>
        /// validation check
        /// </summary>
        /// <param name="objPatientHistory"></param>
        /// <returns></returns>
        private bool CheckedValidation(PatientHistory objPatientHistory, ResponseMessage responseMessage)
        {
            return true;
        }
#pragma warning restore CS8600

    }
}
