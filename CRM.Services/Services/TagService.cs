using CRM.Common.Constants;
using CRM.Common.DTO;
using CRM.Common.Enums;
using CRM.Common.Helper;
using CRM.Common.Models;
using CRM.Common.VM;
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
    public class TagService : ITagService
    {
        private readonly CRMDbContext _crmDbContext;

        public TagService(CRMDbContext ctx)
        {
            this._crmDbContext = ctx;
        }

        /// <summary>
        /// Get all System User
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public async Task<ResponseMessage> GetAllTag(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                List<Tag> lstTag = new List<Tag>();
                int totalSkip = 0;
                totalSkip = (requestMessage.PageNumber > 0) ? requestMessage.PageNumber * requestMessage.PageRecordSize : 0;

                lstTag = await _crmDbContext.Tag.Where(x => x.Status == (int)Enums.Status.Active).OrderBy(x => x.TagID).Skip(totalSkip).Take(requestMessage.PageRecordSize).ToListAsync();
                responseMessage.TotalCount = lstTag.Count();

                foreach (Tag tag in lstTag)
                {
                    // get all department of each Tag
                    List<TagAndDepartmentMapping> lstTagAndDepartmentMappings = await _crmDbContext.TagAndDepartmentMapping.AsNoTracking().Where(x => x.TagID == tag.TagID).ToListAsync();

                    if (lstTag.Count > 0)
                    {
                        foreach (TagAndDepartmentMapping objTagAndDepartmentMapping in lstTagAndDepartmentMappings)
                        {
                            Department objDepartment = await _crmDbContext.Department.AsNoTracking().Where(x => x.DepartmentID == objTagAndDepartmentMapping.DepartmentID).FirstOrDefaultAsync();

                            if (objDepartment != null)
                            {
                                tag.lstDepartment.Add(objDepartment);
                            }

                        }
                    }
                }


                responseMessage.ResponseObj = lstTag;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GetAllTag");
            }
            catch (Exception ex)
            {
                //Process excetion, Development mode show real exception and production mode will show custom exception.
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetAllTag");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

        /// <summary>    
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseMessage> GetTagById(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                Tag objTag = new Tag();
                int TagID = JsonConvert.DeserializeObject<int>(requestMessage?.RequestObj.ToString());

                objTag = await _crmDbContext.Tag.FirstOrDefaultAsync(x => x.TagID == TagID && x.Status == (int)Enums.Status.Active);
                responseMessage.ResponseObj = objTag;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GetTagById");
            }
            catch (Exception ex)
            {
                //Process excetion, Development mode show real exception and production mode will show custom exception.
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetTagById");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

        /// <summary>    
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseMessage> GetTagByDepartment(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                List<VMDepartmentTag> lstTag = new List<VMDepartmentTag>();

                int DepartmentID = JsonConvert.DeserializeObject<int>(requestMessage?.RequestObj.ToString());

                lstTag = await _crmDbContext.VMDepartmentTag.Where(x=>x.DepartmentID==DepartmentID).ToListAsync();
                responseMessage.ResponseObj = lstTag;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GetTagByDepartment");
            }
            catch (Exception ex)
            {
                //Process excetion, Development mode show real exception and production mode will show custom exception.
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetTagByDepartment");
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
        public async Task<ResponseMessage> SaveTag(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            int actionType = (int)Enums.ActionType.Insert;
            try
            {
                Tag objTag = JsonConvert.DeserializeObject<Tag>(requestMessage?.RequestObj.ToString());

                if (objTag != null)
                {
                    if (CheckedValidation(objTag, responseMessage))
                    {
                        if (objTag.TagID > 0)
                        {
                            Tag existingTag = await this._crmDbContext.Tag.AsNoTracking().FirstOrDefaultAsync(x => x.TagID == objTag.TagID && x.Status == (int)Enums.Status.Active);

                            if (existingTag != null)
                            {
                                actionType = (int)Enums.ActionType.Update;
                                objTag.CreatedBy = existingTag.CreatedBy;
                                objTag.CreatedDate = existingTag.CreatedDate;
                                objTag.UpdatedDate = DateTime.Now;
                                objTag.UpdatedBy = requestMessage.UserID;
                                _crmDbContext.Tag.Update(objTag);

                            }
                        }
                        else
                        {
                            objTag.Status = (int)Enums.Status.Active;
                            objTag.CreatedDate = DateTime.Now;
                            objTag.CreatedBy = requestMessage.UserID;
                            await _crmDbContext.Tag.AddAsync(objTag);

                        }

                        await _crmDbContext.SaveChangesAsync();

                        // save departments which are mapped with this Tag
                        if (objTag.lstDepartment.Count > 0)
                        {
                            List<TagAndDepartmentMapping> lstTagAndDepartmentMappings = await _crmDbContext.TagAndDepartmentMapping.AsNoTracking().Where(x => x.TagID == objTag.TagID).ToListAsync();

                            _crmDbContext.TagAndDepartmentMapping.RemoveRange(lstTagAndDepartmentMappings);


                            foreach (Department department in objTag.lstDepartment)
                            {
                                TagAndDepartmentMapping objTagAndDepartmentMapping = new TagAndDepartmentMapping();
                                objTagAndDepartmentMapping.TagID = objTag.TagID;
                                objTagAndDepartmentMapping.DepartmentID = department.DepartmentID;

                                await _crmDbContext.TagAndDepartmentMapping.AddAsync(objTagAndDepartmentMapping);
                            }
                        }
                        await _crmDbContext.SaveChangesAsync();

                        responseMessage.ResponseObj = objTag;
                        responseMessage.Message = MessageConstant.SavedSuccessfully;
                        responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                        //Log write
                        LogHelper.WriteLog(requestMessage.RequestObj, actionType, requestMessage.UserID, "SaveTag");
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
        /// Save and update System user
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<ResponseMessage> DeleteTag(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            int actionType = (int)Enums.ActionType.Delete;

            try
            {
                Tag objTag = JsonConvert.DeserializeObject<Tag>(requestMessage?.RequestObj.ToString());

                if (objTag != null)
                {
                    Tag existingTag = await _crmDbContext.Tag.AsNoTracking().FirstOrDefaultAsync(x => x.TagID == objTag.TagID);

                    if (existingTag?.TagID > 0)
                    {
                        existingTag.Status = (int)Enums.Status.Delete;
                        existingTag.UpdatedBy = requestMessage.UserID;
                        existingTag.UpdatedDate = DateTime.Now;
                        _crmDbContext.Tag.Update(existingTag);
                    }

                    await _crmDbContext.SaveChangesAsync();

                    responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
                    responseMessage.Message = MessageConstant.DeleteSuccess;
                }
                else
                {
                    responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
                    responseMessage.Message = MessageConstant.DeleteFailed;
                }

            }
            catch (Exception ex)
            {
                //Process excetion, Development mode show real exception and production mode will show custom exception.
                responseMessage.Message = ExceptionHelper.ProcessException(ex, actionType, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "DeleteTag");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;

            }

            return responseMessage;
        }

        /// <summary>
        /// validation check
        /// </summary>
        /// <param name="objTag"></param>
        /// <returns></returns>
        private bool CheckedValidation(Tag objTag, ResponseMessage responseMessage)
        {
            if (string.IsNullOrEmpty(objTag.TagTitle))
            {
                responseMessage.Message = MessageConstant.TagTitle;
                return false;
            }
            if (string.IsNullOrEmpty(objTag.Color))
            {
                responseMessage.Message = MessageConstant.TagColor;
                return false;
            }

            return true;
        }
#pragma warning restore CS8600

    }


}
