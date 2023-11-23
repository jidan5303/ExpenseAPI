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
#pragma warning disable CS8602
    public class DefaultTableColumnService : IDefaultTableColumnService
    {
        private readonly CRMDbContext _crmDbContext;

        public DefaultTableColumnService(CRMDbContext ctx)
        {
            this._crmDbContext = ctx;
        }

        /// <summary>
        /// Get all default table column.
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public async Task<ResponseMessage> GetAllDefaultTableColumn(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                List<DefaultTableColumn> lstDefaultTableColumn = new List<DefaultTableColumn>();
                int totalSkip = 0;
                totalSkip = (requestMessage.PageNumber > 0) ? requestMessage.PageNumber * requestMessage.PageRecordSize : 0;

                lstDefaultTableColumn = await _crmDbContext.DefaultTableColumn.OrderBy(x => x.DefaultTableColumnID).Skip(totalSkip).Take(requestMessage.PageRecordSize).ToListAsync();
                responseMessage.ResponseObj = lstDefaultTableColumn;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GetAllDefaultTableColumn");
            }
            catch (Exception ex)
            {
                //Process excetion, Development mode show real exception and production mode will show custom exception.
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetAllDefaultTableColumn");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

        /// <summary>
        /// Get default table column by id
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseMessage> GetDefaultTableColumnById(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                DefaultTableColumn objDefaultTableColumn = new DefaultTableColumn();
                int DefaultTableColumnID = JsonConvert.DeserializeObject<int>(requestMessage?.RequestObj.ToString());

                objDefaultTableColumn = await _crmDbContext.DefaultTableColumn.FirstOrDefaultAsync(x => x.DefaultTableColumnID == DefaultTableColumnID && x.Status == (int)Enums.Status.Active);
                responseMessage.ResponseObj = objDefaultTableColumn;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GetDefaultTableColumnById");
            }
            catch (Exception ex)
            {
                //Process excetion, Development mode show real exception and production mode will show custom exception.
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetDefaultTableColumnById");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

        /// <summary>
        /// Save and update DefaultTableColumn
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<ResponseMessage> SaveDefaultTableColumn(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            int actionType = (int)Enums.ActionType.Insert;
            try
            {

                DefaultTableColumn objDefaultTableColumn = JsonConvert.DeserializeObject<DefaultTableColumn>(requestMessage?.RequestObj.ToString());



                if (objDefaultTableColumn != null)
                {
                    if (CheckedValidation(objDefaultTableColumn, responseMessage))
                    {
                        if (objDefaultTableColumn.DefaultTableColumnID > 0)
                        {
                            DefaultTableColumn existingDefaultTableColumn = await this._crmDbContext.DefaultTableColumn.AsNoTracking().FirstOrDefaultAsync(x => x.DefaultTableColumnID == objDefaultTableColumn.DefaultTableColumnID && x.Status == (int)Enums.Status.Active);
                            if (existingDefaultTableColumn != null)
                            {
                                actionType = (int)Enums.ActionType.Update;
                                objDefaultTableColumn.CreatedDate = existingDefaultTableColumn.CreatedDate;
                                objDefaultTableColumn.CreatedBy = existingDefaultTableColumn.CreatedBy;
                                objDefaultTableColumn.UpdatedDate = DateTime.Now;
                                objDefaultTableColumn.UpdatedBy = requestMessage.UserID;
                                _crmDbContext.DefaultTableColumn.Update(objDefaultTableColumn);
                            }
                        }
                        else
                        {
                            objDefaultTableColumn.Status = (int)Enums.Status.Active;
                            objDefaultTableColumn.CreatedDate = DateTime.Now;
                            objDefaultTableColumn.CreatedBy = requestMessage.UserID;
                            await _crmDbContext.DefaultTableColumn.AddAsync(objDefaultTableColumn);

                        }
                        await _crmDbContext.SaveChangesAsync();

                        responseMessage.ResponseObj = objDefaultTableColumn;
                        responseMessage.Message = MessageConstant.SavedSuccessfully;
                        responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                        //Log write
                        LogHelper.WriteLog(requestMessage.RequestObj, actionType, requestMessage.UserID, "SaveDefaultTableColumn");

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
                responseMessage.Message = ExceptionHelper.ProcessException(ex, actionType, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "SaveDefaultTableColumn");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;

            }


            return responseMessage;
        }
        /// <summary>
        /// validation check
        /// </summary>
        /// <param name="objDefaultTableColumn"></param>
        /// <returns></returns>
        private bool CheckedValidation(DefaultTableColumn objDefaultTableColumn, ResponseMessage responseMessage)
        {
            if (string.IsNullOrEmpty(objDefaultTableColumn.ColumnName))
            {
                responseMessage.Message = MessageConstant.ColumnName;
                return false;
            }

            return true;
        }
#pragma warning restore CS8600
#pragma warning restore CS8602
    }



}
