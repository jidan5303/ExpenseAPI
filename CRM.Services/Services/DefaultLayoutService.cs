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
using System.Text;
using System.Threading.Tasks;

namespace CRM.Services
{
#pragma warning disable CS8600
    public class DefaultLayoutService : IDefaultLayoutService
    {
        private readonly CRMDbContext _crmDbContext;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public DefaultLayoutService(CRMDbContext ctx, IServiceScopeFactory serviceScopeFactory)
        {
            this._crmDbContext = ctx;
            this._serviceScopeFactory = serviceScopeFactory;
        }

        /// <summary>
        /// Get All DefaultLayout
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public async Task<ResponseMessage> GetAllDefaultLayout(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                List<DefaultLayout> lstDefaultLayout = new List<DefaultLayout>();
                int totalSkip = 0;
                totalSkip = (requestMessage.PageNumber > 0) ? requestMessage.PageNumber * requestMessage.PageRecordSize : 0;

                lstDefaultLayout = await _crmDbContext.DefaultLayout.OrderBy(x => x.DefaultLayoutID).Skip(totalSkip).Take(requestMessage.PageRecordSize).ToListAsync();
                responseMessage.ResponseObj = lstDefaultLayout;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GetAllDefaultLayout");
            }
            catch (Exception ex)
            {
                //Process excetion, Development mode show real exception and production mode will show custom exception.
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetAllDefaultLayout");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

        /// <summary>
        ///  Get All DefaultLayout Init Data
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public async Task<ResponseMessage> GetAllDefaultLayoutInitData(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                VMDefaultLayoutIntData objVMDefaultLayoutIntData = new VMDefaultLayoutIntData();
                List<DefaultTable> lstDefaultTable = new List<DefaultTable>();

                int totalSkip = 0;
                totalSkip = (requestMessage.PageNumber > 0) ? requestMessage.PageNumber * requestMessage.PageRecordSize : 0;
                lstDefaultTable = await _crmDbContext.DefaultTable.Where(dl => dl.ShowForLayout == true && dl.Status == (int)Enums.Status.Active).OrderBy(x => x.DefaultTableID).Skip(totalSkip).Take(requestMessage.PageRecordSize).ToListAsync();

                if (lstDefaultTable.Count() > 0)
                {
                    objVMDefaultLayoutIntData.lstDefaultTable = lstDefaultTable;

                    var lstDefaultTableColumn = _crmDbContext.DefaultTableColumn.Where(x => x.DefaultTableID == lstDefaultTable[0].DefaultTableID).AsQueryable();
                    objVMDefaultLayoutIntData.lstDefaultTableColumn = await lstDefaultTableColumn.ToListAsync();


                    List<int> lstDefaultTableColumnId = new List<int>();

                    if (objVMDefaultLayoutIntData?.lstDefaultTableColumn.Count() > 0)
                    {
                        lstDefaultTableColumnId = objVMDefaultLayoutIntData.lstDefaultTableColumn.Select(x => x.DefaultTableColumnID).Distinct().ToList();
                        var lstDefaultLayoutDetail = _crmDbContext.DefaultLayoutDetail.Where(x => lstDefaultTableColumnId.Contains((int)x.DefaultTableColumnID)).AsQueryable();
                        objVMDefaultLayoutIntData.lstDefaultLayoutDetail = await lstDefaultLayoutDetail.ToListAsync();
                    }
                }

                responseMessage.ResponseObj = objVMDefaultLayoutIntData;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GetAllDefaultLayout");
            }
            catch (Exception ex)
            {
                //Process excetion, Development mode show real exception and production mode will show custom exception.
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetAllDefaultLayout");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }


        // <summary>
        /// 
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseMessage> GeDefaultTableColumnByDefaultTableID(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                VMDefaultLayoutColumn objVMDefaultLayoutColumn = new VMDefaultLayoutColumn();

                int defaultTableID = JsonConvert.DeserializeObject<int>(requestMessage?.RequestObj.ToString());

                var lstDefaultTableColumn = _crmDbContext.DefaultTableColumn.Where(x => x.DefaultTableID == defaultTableID && x.Status == (int)Enums.Status.Active).AsQueryable();

                objVMDefaultLayoutColumn.lstDefaultTableColumn = await lstDefaultTableColumn.ToListAsync();

                if (objVMDefaultLayoutColumn?.lstDefaultTableColumn.Count() > 0)
                {
                    List<int> lstDefaultLayoutColumnID = new List<int>();
                    lstDefaultLayoutColumnID = objVMDefaultLayoutColumn?.lstDefaultTableColumn.Select(x => x.DefaultTableColumnID).Distinct().ToList();

                    var lstDefaultLayoutDetails = _crmDbContext.DefaultLayoutDetail.Where(x => lstDefaultLayoutColumnID.Contains((int)x.DefaultTableColumnID)).AsQueryable();

                    objVMDefaultLayoutColumn.lstDefaultLayoutDetail = await lstDefaultLayoutDetails.ToListAsync();
                }

                responseMessage.ResponseObj = objVMDefaultLayoutColumn;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GeDefaultTableColumnByDefaultTableID");
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
        /// 
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseMessage> GetDefaultLayoutById(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                DefaultLayout objDefaultLayout = new DefaultLayout();
                int DefaultLayoutID = JsonConvert.DeserializeObject<int>(requestMessage?.RequestObj.ToString());

                objDefaultLayout = await _crmDbContext.DefaultLayout.FirstOrDefaultAsync(x => x.DefaultLayoutID == DefaultLayoutID && x.Status == (int)Enums.Status.Active);
                responseMessage.ResponseObj = objDefaultLayout;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GetDefaultLayoutById");
            }
            catch (Exception ex)
            {
                //Process excetion, Development mode show real exception and production mode will show custom exception.
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetDefaultLayoutById");
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
        public async Task<ResponseMessage> SaveDefaultLayout(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            int actionType = (int)Enums.ActionType.Insert;
            try
            {

                DefaultLayout objDefaultLayout = JsonConvert.DeserializeObject<DefaultLayout>(requestMessage?.RequestObj.ToString());



                if (objDefaultLayout != null)
                {
                    if (CheckedValidation(objDefaultLayout, responseMessage))
                    {
                        if (objDefaultLayout.DefaultLayoutID > 0)
                        {
                            DefaultLayout existingDefaultLayout = await this._crmDbContext.DefaultLayout.AsNoTracking().FirstOrDefaultAsync(x => x.DefaultLayoutID == objDefaultLayout.DefaultLayoutID && x.Status == (int)Enums.Status.Active);
                            if (existingDefaultLayout != null)
                            {
                                actionType = (int)Enums.ActionType.Update;
                                objDefaultLayout.CreatedDate = existingDefaultLayout.CreatedDate;
                                objDefaultLayout.CreatedBy = existingDefaultLayout.CreatedBy;
                                objDefaultLayout.UpdatedDate = DateTime.Now;
                                objDefaultLayout.UpdatedBy = requestMessage.UserID;
                                _crmDbContext.DefaultLayout.Update(objDefaultLayout);
                            }
                        }
                        else
                        {
                            objDefaultLayout.Status = (int)Enums.Status.Active;
                            objDefaultLayout.CreatedDate = DateTime.Now;
                            objDefaultLayout.CreatedBy = requestMessage.UserID;
                            await _crmDbContext.DefaultLayout.AddAsync(objDefaultLayout);

                        }

                        await _crmDbContext.SaveChangesAsync();

                        responseMessage.ResponseObj = objDefaultLayout;
                        responseMessage.Message = MessageConstant.SavedSuccessfully;
                        responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                        //Log write
                        LogHelper.WriteLog(requestMessage.RequestObj, actionType, requestMessage.UserID, "SaveDefaultLayout");
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
                responseMessage.Message = ExceptionHelper.ProcessException(ex, actionType, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "SaveDefaultLayout");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;

            }


            return responseMessage;
        }

        /// <summary>
        /// Save Default Layout And Details
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<ResponseMessage> SaveDefaultLayoutAndDetails(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            int actionType = (int)Enums.ActionType.Insert;
            try
            {

                VMSaveDefaultLayout objVMSaveDefaultLayout = JsonConvert.DeserializeObject<VMSaveDefaultLayout>(requestMessage?.RequestObj.ToString());

                DefaultLayout objDefaultLayout = new DefaultLayout();

                if (objVMSaveDefaultLayout?.DefaultLayout?.TableID > 0)
                {
                    objDefaultLayout = await _crmDbContext.DefaultLayout.AsNoTracking().FirstOrDefaultAsync(x => x.TableID == objVMSaveDefaultLayout.DefaultLayout.TableID);

                    if (objDefaultLayout == null)
                    {
                        objDefaultLayout = new DefaultLayout();
                        objDefaultLayout.TableID = objVMSaveDefaultLayout?.DefaultLayout?.TableID;
                        objDefaultLayout.Description = objVMSaveDefaultLayout?.DefaultLayout?.Description;
                        objDefaultLayout.CreatedBy = requestMessage.UserID;
                        objDefaultLayout.Status = (int)Enums.Status.Active;
                        objDefaultLayout.CreatedDate = DateTime.Now;

                        await _crmDbContext.DefaultLayout.AddAsync(objDefaultLayout);
                        await _crmDbContext.SaveChangesAsync();
                    }

                    List<DefaultLayoutDetail> lstDefaultLayoutDetail = new List<DefaultLayoutDetail>();

                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetService<CRMDbContext>();
                        List<int> lstDefaultColumn = new List<int>();

                        lstDefaultLayoutDetail = await dbContext.DefaultLayoutDetail.Where(x => x.DefaultLayoutID == objDefaultLayout.DefaultLayoutID).AsNoTracking().ToListAsync();

                        if (lstDefaultLayoutDetail.Count() > 0)
                        {
                            dbContext.DefaultLayoutDetail.RemoveRange(lstDefaultLayoutDetail);
                        }
                        if (objVMSaveDefaultLayout?.lstDefaultLayoutDetail.Count() > 0)
                        {
                            foreach (DefaultLayoutDetail objDefaultLayoutDetail in objVMSaveDefaultLayout?.lstDefaultLayoutDetail)
                            {
                                objDefaultLayoutDetail.DefaultLayoutID = objDefaultLayout.TableID;
                            }


                            await dbContext.AddRangeAsync(objVMSaveDefaultLayout.lstDefaultLayoutDetail);
                        }
                        await dbContext.SaveChangesAsync();
                    }

                    responseMessage.ResponseObj = objVMSaveDefaultLayout;
                    responseMessage.Message = MessageConstant.SavedSuccessfully;
                    responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                    //Log write
                    LogHelper.WriteLog(requestMessage.RequestObj, actionType, requestMessage.UserID, "SaveDefaultLayout");

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
                responseMessage.Message = ExceptionHelper.ProcessException(ex, actionType, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "SaveDefaultLayout");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;

            }
            return responseMessage;
        }

        /// <summary>
        /// validation check
        /// </summary>
        /// <param name="objDefaultLayout"></param>
        /// <returns></returns>
        private bool CheckedValidation(DefaultLayout objDefaultLayout, ResponseMessage responseMessage)
        {
            return true;
        }
#pragma warning restore CS8600

    }



}
