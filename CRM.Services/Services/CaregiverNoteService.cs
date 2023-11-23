using CRM.Common.Constants;
using CRM.Common.DTO;
using CRM.Common.Enums;
using CRM.Common.Helper;
using CRM.Common.Models;
using CRM.Common.VM;
using CRM.DataAccess;
using CRM.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Services
{
#pragma warning disable CS8600
#pragma warning disable CS8603
    public class CaregiverNoteService : ICaregiverNoteService
    {
        private readonly CRMDbContext _crmDbContext;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public CaregiverNoteService(CRMDbContext ctx, IServiceScopeFactory serviceScopeFactory)
        {
            this._crmDbContext = ctx;
            this._serviceScopeFactory = serviceScopeFactory;

        }

        /// <summary>
        ///  Save Caregiver Note
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public async Task<ResponseMessage> SaveCaregiverNote(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            int actionType = (int)Enums.ActionType.Insert;
            try
            {

                CaregiverNotes objCaregiverNotes = JsonConvert.DeserializeObject<CaregiverNotes>(requestMessage?.RequestObj.ToString());

                if (objCaregiverNotes != null)
                {
                    if (CheckedCaregiverNoteValidation(objCaregiverNotes, responseMessage))
                    {
                        if (objCaregiverNotes.CaregiverNoteID > 0)
                        {
                            CaregiverNotes existingCaregiver = await this._crmDbContext.CaregiverNotes.AsNoTracking().FirstOrDefaultAsync(x => x.CaregiverNoteID == objCaregiverNotes.CaregiverNoteID && x.Status == (int)Enums.Status.Active);
                            if (existingCaregiver != null)
                            {
                                actionType = (int)Enums.ActionType.Update;
                                objCaregiverNotes.CreatedDate = existingCaregiver.CreatedDate;
                                objCaregiverNotes.CreatedBy = existingCaregiver.CreatedBy;
                                objCaregiverNotes.UpdatedDate = DateTime.Now;
                                objCaregiverNotes.UpdatedBy = requestMessage.UserID;
                                _crmDbContext.CaregiverNotes.Update(objCaregiverNotes);                                
                            }
                        }
                        else
                        {
                            objCaregiverNotes.Status = (int)Enums.Status.Active;
                            objCaregiverNotes.CreatedDate = DateTime.Now;
                            objCaregiverNotes.CreatedBy = requestMessage.UserID;
                             await _crmDbContext.CaregiverNotes.AddAsync(objCaregiverNotes);                           
                        }
                        await _crmDbContext.SaveChangesAsync();

                        responseMessage.ResponseObj = objCaregiverNotes;
                        responseMessage.Message = MessageConstant.SavedSuccessfully;
                        responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                        //Log write
                        LogHelper.WriteLog(requestMessage.RequestObj, actionType, requestMessage.UserID, "SaveCaregiverNote");
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
                responseMessage.Message = ExceptionHelper.ProcessException(ex, actionType, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "SaveCaregiverNote");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;

            }


            return responseMessage;
        }

        /// <summary>
        ///  Get Caregiver Note List by caregiver ID
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<ResponseMessage> GetCaregiverNoteByCaregiverID(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                List<CaregiverNotes> lstCaregiverNotes = new List<CaregiverNotes>();
                int caregiverID = JsonConvert.DeserializeObject<int>(requestMessage?.RequestObj.ToString());

                lstCaregiverNotes = await _crmDbContext.CaregiverNotes.AsNoTracking().Where(x => x.CaregiverID == caregiverID && x.Status == (int)Enums.Status.Active).OrderByDescending(cn => cn.CaregiverNoteID).ToListAsync();
                responseMessage.ResponseObj = lstCaregiverNotes;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GetCaregiverNoteByCaregiverID");
            }
            catch (Exception ex)
            {
                //Process excetion, Development mode show real exception and production mode will show custom exception.
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetCaregiverNoteByCaregiverID");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

        /// <summary>
        /// validation check
        /// </summary>
        /// <param name="objCaregiverNotes"></param>
        /// <param name="responseMessage"></param>
        /// <returns></returns>

        private bool CheckedCaregiverNoteValidation(CaregiverNotes objCaregiverNotes, ResponseMessage responseMessage)
        {
            if (string.IsNullOrEmpty(objCaregiverNotes.Note))
            {
                responseMessage.Message = MessageConstant.CaregiverNote;
                return false;
            }
            return true;
        }


#pragma warning restore CS8600
#pragma warning restore CS8603
    }


}
