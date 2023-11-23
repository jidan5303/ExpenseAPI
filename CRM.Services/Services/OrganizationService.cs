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
    public class OrganizationService : IOrganizationService
    {
        private readonly CRMDbContext _crmDbContext;

        public OrganizationService(CRMDbContext ctx)
        {
            this._crmDbContext = ctx;
        }

        /// <summary>
        /// Get all organization.
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public async Task<ResponseMessage> GetAllOrganization(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                List<Organization> lstOrganization = new List<Organization>();
                int totalSkip = 0;
                totalSkip = (requestMessage.PageNumber > 0) ? requestMessage.PageNumber * requestMessage.PageRecordSize : 0;

                lstOrganization = await _crmDbContext.Organization.OrderBy(x => x.OrganizationID).Skip(totalSkip).Take(requestMessage.PageRecordSize).ToListAsync();
                responseMessage.ResponseObj = lstOrganization;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GetAllOrganization");
            }
            catch (Exception ex)
            {
                //Process excetion, Development mode show real exception and production mode will show custom exception.
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetAllOrganization");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

        /// <summary>
        /// get Organization by id.
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseMessage> GetOrganizationById(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                Organization objOrganization = new Organization();
                int OrganizationID = JsonConvert.DeserializeObject<int>(requestMessage?.RequestObj.ToString());

                objOrganization = await _crmDbContext.Organization.AsNoTracking().FirstOrDefaultAsync(x => x.OrganizationID == OrganizationID && x.Status == (int)Enums.Status.Active);
                responseMessage.ResponseObj = objOrganization;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GetOrganizationById");
            }
            catch (Exception ex)
            {
                //Process excetion, Development mode show real exception and production mode will show custom exception.
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetOrganizationById");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

        /// <summary>
        /// Save and update Organization
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<ResponseMessage> SaveOrganization(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            int actionType = (int)Enums.ActionType.Insert;
            try
            {

                Organization objOrganization = JsonConvert.DeserializeObject<Organization>(requestMessage?.RequestObj.ToString());

                if (objOrganization != null)
                {
                    if (CheckedValidation(objOrganization, responseMessage))
                    {
                        if (objOrganization.OrganizationID > 0)
                        {
                            Organization existingOrganization = await this._crmDbContext.Organization.AsNoTracking().FirstOrDefaultAsync(x => x.OrganizationID == objOrganization.OrganizationID && x.Status == (int)Enums.Status.Active);
                            if (existingOrganization != null)
                            {
                                actionType = (int)Enums.ActionType.Update;
                                objOrganization.CreatedDate = existingOrganization.CreatedDate;
                                objOrganization.CreatedBy = existingOrganization.CreatedBy;
                                objOrganization.UpdatedDate = DateTime.Now;
                                objOrganization.UpdatedBy = requestMessage.UserID;
                                _crmDbContext.Organization.Update(objOrganization);
                            }
                        }
                        else
                        {
                            objOrganization.Status = (int)Enums.Status.Active;
                            objOrganization.CreatedDate = DateTime.Now;
                            objOrganization.CreatedBy = requestMessage.UserID;
                            await _crmDbContext.Organization.AddAsync(objOrganization);
                           
                        }

                        await _crmDbContext.SaveChangesAsync();

                        responseMessage.ResponseObj=objOrganization;
                        responseMessage.Message = MessageConstant.SavedSuccessfully;
                        responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                        //Log write
                        LogHelper.WriteLog(requestMessage.RequestObj, actionType, requestMessage.UserID, "SaveOrganization");
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
                responseMessage.Message = ExceptionHelper.ProcessException(ex, actionType, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "SaveOrganization");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;

            }


            return responseMessage;
        }
        /// <summary>
        /// validation check
        /// </summary>
        /// <param name="objOrganization"></param>
        /// <returns></returns>
        private bool CheckedValidation(Organization objOrganization, ResponseMessage responseMessage)
        {
            if (string.IsNullOrEmpty(objOrganization.OrganizationName))
            {
                responseMessage.Message = MessageConstant.OrganizationName;
                return false;
            }

            return true;
        }
#pragma warning restore CS8600

    }



}
