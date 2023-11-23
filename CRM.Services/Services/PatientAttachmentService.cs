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
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Services.Services
{
    public class PatientAttachmentService : IPatientAttachmentService
    {
        private readonly CRMDbContext _cRMDbContext;
        private readonly IConfiguration _configuration;
        public PatientAttachmentService(CRMDbContext cRMDbContext,
            IConfiguration configuration)
        {
            _cRMDbContext = cRMDbContext;
            _configuration = configuration;
        }

        /// <summary>
        /// get all patient attachment
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public async Task<ResponseMessage> GetAllPatientAttachmentList(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                List<PatientAttachment> lstAttachment = new List<PatientAttachment>();

                int totalSkip = 0;
                int patientId = Convert.ToInt32(requestMessage.RequestObj.ToString());
                totalSkip = (requestMessage.PageNumber > 0) ? requestMessage.PageNumber * requestMessage.PageRecordSize : 0;

                IQueryable<PatientAttachment> attachmentQuery = _cRMDbContext.PatientAttachment
                    .Where(x => x.PatientID == patientId);
                responseMessage.TotalCount = await attachmentQuery.CountAsync();

                attachmentQuery = attachmentQuery.OrderBy(x => x.PatientAttachmentID)
                .Skip(totalSkip)
                .Take(requestMessage.PageRecordSize);

                lstAttachment = await attachmentQuery.ToListAsync();
                responseMessage.ResponseObj = lstAttachment;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                //Log write
                LogHelper.WriteLog(requestMessage.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GetAllPatientAttachmentList");
            }
            catch (Exception ex)
            {
                //Process excetion, Development mode show real exception and production mode will show custom exception.
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetAllPatientAttachmentList");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

        /// <summary>
        /// get all patient attachment by id.
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public async Task<ResponseMessage> GetPatientAttachmentById(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                PatientAttachment? objDepartment = new PatientAttachment();
                int attachmentId = JsonConvert.DeserializeObject<int>(requestMessage.RequestObj.ToString());

                objDepartment = await _cRMDbContext.PatientAttachment
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.PatientAttachmentID == attachmentId);
                responseMessage.ResponseObj = objDepartment;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                //Log write
                LogHelper.WriteLog(requestMessage.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GetPatientAttachmentById");
            }
            catch (Exception ex)
            {
                //Process excetion, Development mode show real exception and production mode will show custom exception.
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetPatientAttachmentById");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

        /// <summary>
        /// Save and update patient attachment.
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public async Task<ResponseMessage> SavePatientAttachment(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            int actionType = (int)Enums.ActionType.Insert;
            try
            {
                PatientAttachment objPatientAttachment = JsonConvert.DeserializeObject<PatientAttachment>(requestMessage.RequestObj.ToString());
                if (objPatientAttachment != null)
                {
                    if (CheckedValidation(objPatientAttachment, responseMessage))
                    {
                        FilePathRead objFilePath = _configuration.GetSection("Attachments").Get<FilePathRead>();
                        objFilePath.SaveFilePath = Path.Combine(objFilePath.SaveFilePath, objPatientAttachment.AttachmentName);
                        objFilePath.ShowFilePath = Path.Combine(objFilePath.ShowFilePath, objPatientAttachment.AttachmentName);
                        await File.WriteAllBytesAsync(objFilePath.SaveFilePath, Convert.FromBase64String(objPatientAttachment.Base64File.Split(",")[1]));
                        if (objPatientAttachment?.PatientAttachmentID > 0)
                        {
                            PatientAttachment? objExistingPatientAttachment = await _cRMDbContext.PatientAttachment
                                .AsNoTracking()
                                .FirstOrDefaultAsync(x => x.PatientAttachmentID == objPatientAttachment.PatientAttachmentID);

                            if (objExistingPatientAttachment != null)
                            {
                                actionType = (int)Enums.ActionType.Update;

                                objPatientAttachment.CreatedBy = objExistingPatientAttachment.CreatedBy;
                                objPatientAttachment.CreatedDate = objExistingPatientAttachment.CreatedDate;
                                objPatientAttachment.UpdatedDate = DateTime.Now;
                                objPatientAttachment.UpdatedBy = requestMessage.UserID;
                                _cRMDbContext.PatientAttachment.Update(objPatientAttachment);
                            }
                        }
                        else
                        {
                            objPatientAttachment.AttachmentLink = objFilePath.ShowFilePath;
                            objPatientAttachment.CreatedBy = requestMessage.UserID;
                            objPatientAttachment.CreatedDate = DateTime.Now;
                            await _cRMDbContext.PatientAttachment.AddAsync(objPatientAttachment);
                        }

                        await _cRMDbContext.SaveChangesAsync();

                        responseMessage.ResponseObj = objPatientAttachment;
                        responseMessage.Message = MessageConstant.SavedSuccessfully;
                        responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                        //Log write
                        LogHelper.WriteLog(requestMessage.RequestObj, actionType, requestMessage.UserID, "SavePatientAttachment");
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
                responseMessage.Message = ExceptionHelper.ProcessException(ex, actionType, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "SavePatientAttachment");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }
            return responseMessage;
        }

        /// <summary>
        /// For remove Patient Attachment
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public async Task<ResponseMessage> RemovePatientAttachment(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            int actionType = (int)Enums.ActionType.Insert;
            try
            {
                PatientAttachment objPatientAttachment = JsonConvert.DeserializeObject<PatientAttachment>(requestMessage.RequestObj.ToString());
                if (objPatientAttachment != null)
                {
                    FilePathRead objFilePath = _configuration.GetSection("Attachments").Get<FilePathRead>();

                    string filePath = objPatientAttachment.AttachmentLink.Replace(objFilePath.ShowFilePath, objFilePath.SaveFilePath);
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }

                    PatientAttachment? objExistingPatientAttachment = await _cRMDbContext.PatientAttachment
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.PatientAttachmentID == objPatientAttachment.PatientAttachmentID);

                    actionType = (int)Enums.ActionType.Delete;
                    objExistingPatientAttachment.UpdatedDate = DateTime.Now;
                    objExistingPatientAttachment.UpdatedBy = requestMessage.UserID;
                    objExistingPatientAttachment.Status = (int)Enums.Status.Delete;

                    _cRMDbContext.PatientAttachment.Remove(objPatientAttachment);
                    await _cRMDbContext.SaveChangesAsync();

                    responseMessage.ResponseObj = objPatientAttachment;
                    responseMessage.Message = MessageConstant.DeleteSuccess;
                    responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                    //Log write
                    LogHelper.WriteLog(requestMessage.RequestObj, actionType, requestMessage.UserID, "RemovePatientAttachment");
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
                responseMessage.Message = ExceptionHelper.ProcessException(ex, actionType, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "RemovePatientAttachment");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }
            return responseMessage;
        }

        /// <summary>
        /// validation check.
        /// </summary>
        /// <param name="objPatientAttachment"></param>
        /// <param name="responseMessage"></param>
        /// <returns></returns>
        private bool CheckedValidation(PatientAttachment objPatientAttachment, ResponseMessage responseMessage)
        {
            if (string.IsNullOrEmpty(objPatientAttachment.AttachmentName))
            {
                responseMessage.Message = MessageConstant.PatientNote;
                return false;
            }
            else if (string.IsNullOrEmpty(objPatientAttachment.Base64File))
            {
                responseMessage.Message = MessageConstant.PatientNote;
                return false;
            }
            return true;
        }
    }
}
