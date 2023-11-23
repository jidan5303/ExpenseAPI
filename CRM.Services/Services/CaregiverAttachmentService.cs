using CRM.Common.Constants;
using CRM.Common.DTO;
using CRM.Common.Enums;
using CRM.Common.Helper;
using CRM.Common.Models;
using CRM.Common.VM;
using CRM.DataAccess;
using CRM.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
#pragma warning disable CS8602
    public class CaregiverAttachmentService : ICaregiverAttachmentService
    {
        private readonly CRMDbContext _crmDbContext;
        private readonly IConfiguration _configuration;
        public CaregiverAttachmentService(CRMDbContext ctx, IConfiguration configuration)
        {
            this._crmDbContext = ctx;
            this._configuration = configuration;

        }

        /// <summary>
        ///  Get Caregiver Note List by caregiver ID
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<ResponseMessage> GetCaregiverAttachmentByCaregiverID(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                List<CaregiverAttachment> lstCaregiverAttachment = new List<CaregiverAttachment>();
                int caregiverID = JsonConvert.DeserializeObject<int>(requestMessage?.RequestObj.ToString());

                lstCaregiverAttachment = await _crmDbContext.CaregiverAttachment.AsNoTracking().Where(x => x.CaregiverID == caregiverID).OrderByDescending(ca => ca.CaregiverAttachmentID).ToListAsync();
                responseMessage.ResponseObj = lstCaregiverAttachment;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GetCaregiverAttachmentByCaregiverID");
            }
            catch (Exception ex)
            {
                //Process excetion, Development mode show real exception and production mode will show custom exception.
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetCaregiverAttachmentByCaregiverID");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }



        /// <summary>
        /// Save and update caregiver attachment.
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public async Task<ResponseMessage> SaveCaregiverAttachment(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            int actionType = (int)Enums.ActionType.Insert;
            try
            {
                CaregiverAttachment  objCaregiverAttachment = JsonConvert.DeserializeObject<CaregiverAttachment>(requestMessage.RequestObj?.ToString());
                if (objCaregiverAttachment != null)
                {
                    if (CheckedValidation(objCaregiverAttachment, responseMessage))
                    {
                        string showUrl = String.Empty;
                        if (!string.IsNullOrEmpty(objCaregiverAttachment?.AttachmentContent))
                        {
                            string[] base64image = objCaregiverAttachment?.AttachmentContent.Split(',');

                            string filePath = _configuration.GetSection("CaregiverAttachmnet").GetSection("imageSaveUrl").Value + objCaregiverAttachment?.CaregiverAttachmentID.ToString() + "_" + DateTime.Now.ToString("MMddyyyyhhss") + "." + objCaregiverAttachment?.Extention;
                            System.IO.File.WriteAllBytes(filePath, Convert.FromBase64String(base64image[1]));


                            string getshowUrl = _configuration.GetSection("CaregiverAttachmnet").GetSection("imageUrl").Value;
                            showUrl = filePath.Replace(_configuration.GetSection("CaregiverAttachmnet").GetSection("imageSaveUrl").Value, getshowUrl);


                        }


                        if (objCaregiverAttachment?.CaregiverAttachmentID > 0)
                        {
                            CaregiverAttachment existingCaregiverAttachment = await _crmDbContext.CaregiverAttachment
                                .AsNoTracking()
                                .FirstOrDefaultAsync(x => x.CaregiverAttachmentID == objCaregiverAttachment.CaregiverAttachmentID);

                            if (existingCaregiverAttachment != null)
                            {
                                actionType = (int)Enums.ActionType.Update;

                                objCaregiverAttachment.AttachmentLink = (!string.IsNullOrEmpty(showUrl))? showUrl: existingCaregiverAttachment.AttachmentLink;
                                objCaregiverAttachment.CreatedBy = existingCaregiverAttachment.CreatedBy;
                                objCaregiverAttachment.CreatedDate = existingCaregiverAttachment.CreatedDate;
                                objCaregiverAttachment.UpdatedDate = DateTime.Now;
                                objCaregiverAttachment.UpdatedBy = requestMessage.UserID;
                                _crmDbContext.CaregiverAttachment.Update(objCaregiverAttachment);
                            }
                        }
                        else
                        {

                            objCaregiverAttachment.AttachmentLink = showUrl;
                            objCaregiverAttachment.CreatedBy = requestMessage.UserID;
                            objCaregiverAttachment.CreatedDate = DateTime.Now;
                            await _crmDbContext.CaregiverAttachment.AddAsync(objCaregiverAttachment);
                        }

                        await _crmDbContext.SaveChangesAsync();

                        responseMessage.ResponseObj = objCaregiverAttachment;
                        responseMessage.Message = MessageConstant.SavedSuccessfully;
                        responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                        //Log write
                        LogHelper.WriteLog(requestMessage.RequestObj ?? "", actionType, requestMessage.UserID, "SaveCaregiverAttachment");
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
                responseMessage.Message = ExceptionHelper.ProcessException(ex, actionType, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "SaveTag");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }
            return responseMessage;
        }


        /// <summary>
        /// for remove attachment.
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public async Task<ResponseMessage> RemoveCaregiverAttachment(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            int actionType = (int)Enums.ActionType.Insert;
            try
            {
                CaregiverAttachment objCaregiverAttachment = JsonConvert.DeserializeObject<CaregiverAttachment>(requestMessage.RequestObj?.ToString());
                if (objCaregiverAttachment != null)
                {
                    string filePath = _configuration.GetSection("CaregiverAttachmnet").GetSection("imageSaveUrl").Value;

                    string showUrl = _configuration.GetSection("CaregiverAttachmnet").GetSection("imageUrl").Value;

                    string fileRemovePath = objCaregiverAttachment.AttachmentLink.Replace(showUrl, filePath);

                    //for delete file from folder.
                    if (File.Exists(fileRemovePath))
                    {
                        File.Delete(fileRemovePath);
                    }

                    CaregiverAttachment? existingCaregiverAttachment = await _crmDbContext.CaregiverAttachment
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.CaregiverAttachmentID == objCaregiverAttachment.CaregiverAttachmentID);

                    actionType = (int)Enums.ActionType.Delete;
                    existingCaregiverAttachment.UpdatedDate = DateTime.Now;
                    existingCaregiverAttachment.UpdatedBy = requestMessage.UserID;
                    existingCaregiverAttachment.Status = (int)Enums.Status.Delete;

                    _crmDbContext.CaregiverAttachment.Remove(existingCaregiverAttachment);                 
                    await _crmDbContext.SaveChangesAsync();

                    responseMessage.ResponseObj = objCaregiverAttachment;
                    responseMessage.Message = MessageConstant.DeleteSuccess;
                    responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                    //Log write
                    LogHelper.WriteLog(requestMessage.RequestObj, actionType, requestMessage.UserID, "RemoveCaregiverAttachment");
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
                responseMessage.Message = ExceptionHelper.ProcessException(ex, actionType, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "RemoveCaregiverAttachment");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }
            return responseMessage;
        }


        /// <summary>
        /// validation check.
        /// </summary>
        /// <param name="objCaregiverAttachment"></param>
        /// <param name="responseMessage"></param>
        /// <returns></returns>
        private bool CheckedValidation(CaregiverAttachment objCaregiverAttachment, ResponseMessage responseMessage)
        {
            if (string.IsNullOrEmpty(objCaregiverAttachment.AttachmentName))
            {
                responseMessage.Message = MessageConstant.PatientNote;
                return false;
            }
            else if (string.IsNullOrEmpty(objCaregiverAttachment.AttachmentContent))
            {
                responseMessage.Message = MessageConstant.PatientNote;
                return false;
            }
            return true;
        }


#pragma warning restore CS8600
#pragma warning restore CS8603

    }


}
