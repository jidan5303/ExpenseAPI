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
    public class DefaultTableService : IDefaultTableService
    {
        private readonly CRMDbContext _crmDbContext;

        public DefaultTableService(CRMDbContext ctx)
        {
            this._crmDbContext = ctx;
        }

        /// <summary>
        /// Get all default table.
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public async Task<ResponseMessage> GetAllDefaultTable(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                List<DefaultTable> lstDefaultTable = new List<DefaultTable>();
                int totalSkip = 0;
                totalSkip = (requestMessage.PageNumber > 0) ? requestMessage.PageNumber * requestMessage.PageRecordSize : 0;

                lstDefaultTable = await _crmDbContext.DefaultTable.OrderBy(x => x.DefaultTableID).Skip(totalSkip).Take(requestMessage.PageRecordSize).ToListAsync();
                responseMessage.ResponseObj = lstDefaultTable;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View,requestMessage.UserID, "GetAllDefaultTable");
            }
            catch (Exception ex)
            {
                //Process excetion, Development mode show real exception and production mode will show custom exception.
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID,JsonConvert.SerializeObject(requestMessage.RequestObj), "GetAllDefaultTable");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

        /// <summary>
        /// get default table by id.
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseMessage> GetDefaultTableById(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                DefaultTable objDefaultTable = new DefaultTable();
                int DefaultTableID = JsonConvert.DeserializeObject<int>(requestMessage?.RequestObj.ToString());

                objDefaultTable = await _crmDbContext.DefaultTable.FirstOrDefaultAsync(x => x.DefaultTableID == DefaultTableID && x.Status == (int)Enums.Status.Active);
                responseMessage.ResponseObj = objDefaultTable;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View,requestMessage.UserID, "GetDefaultTableById");
            }
            catch (Exception ex)
            {
                //Process excetion, Development mode show real exception and production mode will show custom exception.
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID,JsonConvert.SerializeObject(requestMessage.RequestObj), "GetDefaultTableById");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

        /// <summary>
        /// Save and update default table.
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<ResponseMessage> SaveDefaultTable(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            int actionType = (int)Enums.ActionType.Insert;
            try
            {

                DefaultTable objDefaultTable = JsonConvert.DeserializeObject<DefaultTable>(requestMessage?.RequestObj.ToString());

              

                if (objDefaultTable != null)
                {
                    if (CheckedValidation(objDefaultTable, responseMessage))
                    {
                        if (objDefaultTable.DefaultTableID > 0)
                        {
                            DefaultTable existingDefaultTable = await this._crmDbContext.DefaultTable.AsNoTracking().FirstOrDefaultAsync(x => x.DefaultTableID == objDefaultTable.DefaultTableID && x.Status == (int)Enums.Status.Active);
                            if (existingDefaultTable != null)
                            {
                                actionType = (int)Enums.ActionType.Update;
                                objDefaultTable.CreatedDate = existingDefaultTable.CreatedDate;
                                objDefaultTable.CreatedBy = existingDefaultTable.CreatedBy;
                                objDefaultTable.UpdatedDate = DateTime.Now;
                                objDefaultTable.UpdatedBy = requestMessage.UserID;
                                _crmDbContext.DefaultTable.Update(objDefaultTable);
                            }
                        }
                        else
                        {
                            objDefaultTable.Status = (int)Enums.Status.Active;
                            objDefaultTable.CreatedDate = DateTime.Now;
                            objDefaultTable.CreatedBy = requestMessage.UserID;
                            await _crmDbContext.DefaultTable.AddAsync(objDefaultTable);
                        
                        }

                        await _crmDbContext.SaveChangesAsync();

                        responseMessage.ResponseObj = objDefaultTable;
                        responseMessage.Message = MessageConstant.SavedSuccessfully;
                        responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                        //Log write
                        LogHelper.WriteLog(requestMessage.RequestObj, actionType,requestMessage.UserID, "SaveDefaultTable");
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
                responseMessage.Message = ExceptionHelper.ProcessException(ex, actionType, requestMessage.UserID,JsonConvert.SerializeObject(requestMessage.RequestObj), "SaveDefaultTable");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;

            }


            return responseMessage;
        }
        /// <summary>
        /// validation check
        /// </summary>
        /// <param name="objDefaultTable"></param>
        /// <returns></returns>
        private bool CheckedValidation(DefaultTable objDefaultTable, ResponseMessage responseMessage)
        {
            if (string.IsNullOrEmpty(objDefaultTable.TableName))
            {
                responseMessage.Message = MessageConstant.TableName;
                return false;
            }

            return true;
        }
#pragma warning restore CS8600

    }


}
