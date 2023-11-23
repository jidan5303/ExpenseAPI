using CRM.Common.Constants;
using CRM.Common.DTO;
using CRM.Common.Enums;
using CRM.Common.Helper;
using CRM.Common.Models;
using CRM.DataAccess;
using CRM.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Services
{
#pragma warning disable CS8600
    public class KnowledgeBaseService : IKnowledgeBaseService
    {
        private readonly CRMDbContext _crmDbContext;
        private readonly IConfiguration _configuration;
        public KnowledgeBaseService(CRMDbContext ctx, IConfiguration configuration)
        {
            this._crmDbContext = ctx;
            this._configuration = configuration;
        }

        /// <summary>
        /// Get all System User
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public async Task<ResponseMessage> GetAllKnowledgeBase(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                List<KnowledgeBase> lstKnowledgeBase = new List<KnowledgeBase>();
                int totalSkip = 0;
                totalSkip = (requestMessage.PageNumber > 0) ? requestMessage.PageNumber * requestMessage.PageRecordSize : 0;

                lstKnowledgeBase = await _crmDbContext.KnowledgeBase.OrderBy(x => x.KnowledgeBaseID).Skip(totalSkip).Take(requestMessage.PageRecordSize).ToListAsync();
                responseMessage.ResponseObj = lstKnowledgeBase;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View,requestMessage.UserID, "GetAllKnowledgeBase");
            }
            catch (Exception ex)
            {
                //Process excetion, Development mode show real exception and production mode will show custom exception.
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID,JsonConvert.SerializeObject(requestMessage.RequestObj), "GetAllKnowledgeBase");
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
        public async Task<ResponseMessage> GetKnowledgeBaseById(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                KnowledgeBase objKnowledgeBase = new KnowledgeBase();
                int knowledgeBaseID = JsonConvert.DeserializeObject<int>(requestMessage?.RequestObj.ToString());

                objKnowledgeBase = await _crmDbContext.KnowledgeBase.FirstOrDefaultAsync(x => x.KnowledgeBaseID == knowledgeBaseID && x.Status == (int)Enums.Status.Active);
                responseMessage.ResponseObj = objKnowledgeBase;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View,requestMessage.UserID, "GetKnowledgeBaseById");
            }
            catch (Exception ex)
            {
                //Process excetion, Development mode show real exception and production mode will show custom exception.
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID,JsonConvert.SerializeObject(requestMessage.RequestObj), "GetKnowledgeBaseById");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

        /// <summary>
        /// Save and update Knowledge Base
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<ResponseMessage> SaveKnowledgeBase(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            int actionType = (int)Enums.ActionType.Insert;
            try
            {

                KnowledgeBase objKnowledgeBase = JsonConvert.DeserializeObject<KnowledgeBase>(requestMessage?.RequestObj.ToString());
            

                if (objKnowledgeBase != null)
                {
                    if (CheckedValidation(objKnowledgeBase, responseMessage))
                    {
                        string showUrl = String.Empty;
                        if (!string.IsNullOrEmpty(objKnowledgeBase.AttachmentContent))
                        {
                            string[] base64image = objKnowledgeBase?.AttachmentContent?.Split(',');

                            string filePath = _configuration.GetSection("KnowledgeBase").GetSection("imageKnowledgeBaseSaveUrl").Value + objKnowledgeBase?.KnowledgeBaseID.ToString() + "_" + DateTime.Now.ToString("MMddyyyyhhss") + "." + objKnowledgeBase?.Extention;
                            System.IO.File.WriteAllBytes(filePath, Convert.FromBase64String(base64image[1]));

                           //' showUrl = _configuration.GetSection("KnowledgeBase").GetSection("imageKnowledgeBaseUrl").Value + objKnowledgeBase?.KnowledgeBaseID.ToString() + "_" + DateTime.Now.ToString("MMddyyyyhhss") + "." + objKnowledgeBase?.Extention;

                            string getshowUrl = _configuration.GetSection("KnowledgeBase").GetSection("imageKnowledgeBaseUrl").Value;
                             showUrl = filePath.Replace(_configuration.GetSection("KnowledgeBase").GetSection("imageKnowledgeBaseSaveUrl").Value, getshowUrl);

                        }


                        if (objKnowledgeBase.KnowledgeBaseID > 0)
                        {
                            KnowledgeBase existingKnowledgeBase = await this._crmDbContext.KnowledgeBase.AsNoTracking().FirstOrDefaultAsync(x => x.KnowledgeBaseID == objKnowledgeBase.KnowledgeBaseID && x.Status == (int)Enums.Status.Active);
                            if (existingKnowledgeBase != null)
                            {
                                actionType = (int)Enums.ActionType.Update;
                                objKnowledgeBase.AttachmentLink = (!string.IsNullOrEmpty(showUrl))? showUrl: existingKnowledgeBase.AttachmentLink;
                                objKnowledgeBase.CreatedDate = existingKnowledgeBase.CreatedDate;
                                objKnowledgeBase.CreatedBy = existingKnowledgeBase.CreatedBy;
                                objKnowledgeBase.UpdatedDate = DateTime.Now;
                                objKnowledgeBase.UpdatedBy = requestMessage.UserID;
                                _crmDbContext.KnowledgeBase.Update(objKnowledgeBase);
                                responseMessage.ResponseObj = objKnowledgeBase;
                            }
                        }
                        else
                        {
                            objKnowledgeBase.AttachmentLink = showUrl;
                            objKnowledgeBase.Status = (int)Enums.Status.Active;
                            objKnowledgeBase.CreatedDate = DateTime.Now;
                            objKnowledgeBase.CreatedBy = requestMessage.UserID;
                            await _crmDbContext.KnowledgeBase.AddAsync(objKnowledgeBase);                           
                        }

                        await _crmDbContext.SaveChangesAsync();

                        responseMessage.ResponseObj = objKnowledgeBase;
                        responseMessage.Message = MessageConstant.SavedSuccessfully;
                        responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                        //Log write
                        LogHelper.WriteLog(requestMessage.RequestObj, actionType,requestMessage.UserID, "SaveKnowledgeBase");

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
                responseMessage.Message = ExceptionHelper.ProcessException(ex, actionType, requestMessage.UserID,JsonConvert.SerializeObject(requestMessage.RequestObj), "SaveKnowledgeBase");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;

            }


            return responseMessage;
        }


        /// <summary>
        /// for remove knowledebase.
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public async Task<ResponseMessage> RemoveKnowledgeBase(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            int actionType = (int)Enums.ActionType.Insert;
            try
            {
                KnowledgeBase objKnowledgeBase = JsonConvert.DeserializeObject<KnowledgeBase>(requestMessage?.RequestObj.ToString());
                if (objKnowledgeBase != null)
                {
                    string filePath = _configuration.GetSection("KnowledgeBase").GetSection("imageKnowledgeBaseSaveUrl").Value;

                    string showUrl = _configuration.GetSection("KnowledgeBase").GetSection("imageKnowledgeBaseUrl").Value;

                    string fileRemovePath = objKnowledgeBase.AttachmentLink.Replace(showUrl, filePath);

                    //for delete file from folder.
                    if (File.Exists(fileRemovePath))
                    {
                        File.Delete(fileRemovePath);
                    }

                    KnowledgeBase? existingKnowledgeBase = await _crmDbContext.KnowledgeBase
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.KnowledgeBaseID == objKnowledgeBase.KnowledgeBaseID);

                    actionType = (int)Enums.ActionType.Delete;
                    existingKnowledgeBase.UpdatedDate = DateTime.Now;
                    existingKnowledgeBase.UpdatedBy = requestMessage.UserID;
                    existingKnowledgeBase.Status = (int)Enums.Status.Delete;

                    _crmDbContext.KnowledgeBase.Remove(existingKnowledgeBase);
                    await _crmDbContext.SaveChangesAsync();

                    responseMessage.ResponseObj = objKnowledgeBase;
                    responseMessage.Message = MessageConstant.DeleteSuccess;
                    responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                    //Log write
                    LogHelper.WriteLog(requestMessage.RequestObj, actionType, requestMessage.UserID, "RemoveKnowledgeBaseAttachment");
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
        /// for GeKnowledgeBase By Department ID
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseMessage> GeKnowledgeBaseByDepartmentID(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {

                List<KnowledgeBase>lstKnowledgeBase = new List<KnowledgeBase>();
                int departmentID = JsonConvert.DeserializeObject<int>(requestMessage?.RequestObj.ToString());

                lstKnowledgeBase = await _crmDbContext.KnowledgeBase.Where(x => x.DepartmentID == departmentID && x.Status == (int)Enums.Status.Active).OrderBy(x=>x.KnowledgeBaseID).ThenBy(k=>k.SequenceNo).ToListAsync();
                responseMessage.ResponseObj = lstKnowledgeBase;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GeKnowledgeBaseByDepartmentID");
            }
            catch (Exception ex)
            {
                //Process excetion, Development mode show real exception and production mode will show custom exception.
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetKnowledgeBaseById");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }


        /// <summary>
        /// validation check
        /// </summary>
        /// <param name="objKnowledgeBase"></param>
        /// <returns></returns>
        private bool CheckedValidation(KnowledgeBase objKnowledgeBase, ResponseMessage responseMessage)
        {
            if (string.IsNullOrEmpty(objKnowledgeBase.KnowledgeDetail))
            {
                responseMessage.Message = MessageConstant.KnowledgeDetail;
                return false;
            }
            if (objKnowledgeBase.DepartmentID <= 0)
            {
                responseMessage.Message = MessageConstant.DepartmentName;
                return false;
            }

            return true;
        }
#pragma warning restore CS8600

    }

   
}
