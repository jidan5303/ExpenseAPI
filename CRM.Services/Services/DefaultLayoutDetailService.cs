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
    public class DefaultLayoutDetailService : IDefaultLayoutDetailService
    {
        private readonly CRMDbContext _crmDbContext;

        public DefaultLayoutDetailService(CRMDbContext ctx)
        {
            this._crmDbContext = ctx;
        }

        /// <summary>
        /// Get all Default Layout Detail.
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public async Task<ResponseMessage> GetAllDefaultLayoutDetail(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                List<DefaultLayoutDetail> lstDefaultLayoutDetail = new List<DefaultLayoutDetail>();
                int totalSkip = 0;
                totalSkip = (requestMessage.PageNumber > 0) ? requestMessage.PageNumber * requestMessage.PageRecordSize : 0;

                lstDefaultLayoutDetail = await _crmDbContext.DefaultLayoutDetail.OrderBy(x => x.DefaultLayoutDetailID).Skip(totalSkip).Take(requestMessage.PageRecordSize).ToListAsync();
                responseMessage.ResponseObj = lstDefaultLayoutDetail;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GetAllDefaultLayoutDetail");
            }
            catch (Exception ex)
            {
                //Process excetion, Development mode show real exception and production mode will show custom exception.
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetAllDefaultLayoutDetail");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

        /// <summary>
        /// Get DefaultLayout Detail By Id
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseMessage> GetDefaultLayoutDetailById(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                DefaultLayoutDetail objDefaultLayoutDetail = new DefaultLayoutDetail();
                int DefaultLayoutDetailID = JsonConvert.DeserializeObject<int>(requestMessage?.RequestObj.ToString());

                objDefaultLayoutDetail = await _crmDbContext.DefaultLayoutDetail.FirstOrDefaultAsync(x => x.DefaultLayoutDetailID == DefaultLayoutDetailID);
                responseMessage.ResponseObj = objDefaultLayoutDetail;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GetDefaultLayoutDetailById");
            }
            catch (Exception ex)
            {
                //Process excetion, Development mode show real exception and production mode will show custom exception.
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetDefaultLayoutDetailById");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

        /// <summary>
        /// Save and update Default Layout Detail
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<ResponseMessage> SaveDefaultLayoutDetail(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            int actionType = (int)Enums.ActionType.Insert;
            try
            {

                DefaultLayoutDetail objDefaultLayoutDetail = JsonConvert.DeserializeObject<DefaultLayoutDetail>(requestMessage?.RequestObj.ToString());

                if (objDefaultLayoutDetail != null)
                {
                    if (CheckedValidation(objDefaultLayoutDetail, responseMessage))
                    {
                        if (objDefaultLayoutDetail.DefaultLayoutDetailID > 0)
                        {
                            DefaultLayoutDetail existingDefaultLayoutDetail = await this._crmDbContext.DefaultLayoutDetail.AsNoTracking().FirstOrDefaultAsync(x => x.DefaultLayoutDetailID == objDefaultLayoutDetail.DefaultLayoutDetailID);
                            if (existingDefaultLayoutDetail != null)
                            {
                                actionType = (int)Enums.ActionType.Update;
                                _crmDbContext.DefaultLayoutDetail.Update(objDefaultLayoutDetail);
                            }
                        }
                        else
                        {
                            await _crmDbContext.DefaultLayoutDetail.AddAsync(objDefaultLayoutDetail);
                        }

                        await _crmDbContext.SaveChangesAsync();

                        responseMessage.ResponseObj = objDefaultLayoutDetail;
                        responseMessage.Message = MessageConstant.SavedSuccessfully;
                        responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                        //Log write
                        LogHelper.WriteLog(requestMessage.RequestObj, actionType, requestMessage.UserID, "SaveDefaultLayoutDetail");
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
                responseMessage.Message = ExceptionHelper.ProcessException(ex, actionType, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "SaveDefaultLayoutDetail");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;

            }

            return responseMessage;
        }
        /// <summary>
        /// validation check
        /// </summary>
        /// <param name="objDefaultLayoutDetail"></param>
        /// <returns></returns>
        private bool CheckedValidation(DefaultLayoutDetail objDefaultLayoutDetail, ResponseMessage responseMessage)
        {
            return true;
        }
#pragma warning restore CS8600

    }


}
