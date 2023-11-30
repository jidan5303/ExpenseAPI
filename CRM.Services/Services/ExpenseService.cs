using CRM.Common.Constants;
using CRM.Common.DTO;
using CRM.Common.Enums;
using CRM.Common.Helper;
using CRM.Common.Models;
using CRM.Common.VM;
using CRM.DataAccess;
using CRM.Services.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static CRM.Common.Enums.Enums;

namespace CRM.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly CRMDbContext _crmDbContext;
        private readonly IConfiguration _configuration;

        public ExpenseService(CRMDbContext ctx, IConfiguration configuration)
        {
            this._crmDbContext = ctx;
            _configuration = configuration;
        }

        /// <summary>
        /// Get All Daily Expense By Month
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public async Task<ResponseMessage> GetAllDailyExpenseByMonth(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                List<VMExpenseMonthly> lstExpenseMonthlyAll = new List<VMExpenseMonthly>();
                string monthWithYear = requestMessage.RequestObj.ToString();

                string sql = @" select e.ID as ExpenseID, e.ExpenseCategoryValue as month, e.DateTime as Date, et.Name as ExpenseType, e.Description, e.Amount, e.ExpensedBy from Expense as e
                                left join ExpenseType as et on et.ID = e.ExpenseType
                                where e.ExpenseCategoryValue = '" + monthWithYear + "' and e.Status = 1 and et.Status = 1 and et.ExpenseCategoryID = 1 order by e.DateTime desc";

                lstExpenseMonthlyAll = await _crmDbContext.VMExpenseMonthly.FromSqlRaw(sql).ToListAsync();

                responseMessage.ResponseObj = lstExpenseMonthlyAll;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GetAllMonthlyExpenseByMonth");
            }
            catch (Exception ex)
            {
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View,
                    requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetAllMonthlyExpenseByMonth");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

        public async Task<ResponseMessage> GetAllMonthlyExpenseByMonth(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                List<VMExpenseMonthly> lstExpenseMonthlyAll = new List<VMExpenseMonthly>();
                string monthWithYear = requestMessage.RequestObj.ToString();

                string sql = @" select e.ID as ExpenseID, e.ExpenseCategoryValue as month, e.DateTime as Date, et.Name as ExpenseType, e.Description, e.Amount, e.ExpensedBy from Expense as e
                                left join ExpenseType as et on et.ID = e.ExpenseType
                                where e.ExpenseCategoryValue = '" + monthWithYear + "' and e.Status = 1 and et.Status = 1 and et.ExpenseCategoryID = 2 order by e.ID desc";

                lstExpenseMonthlyAll = await _crmDbContext.VMExpenseMonthly.FromSqlRaw(sql).ToListAsync();

                responseMessage.ResponseObj = lstExpenseMonthlyAll;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GetAllMonthlyExpenseByMonth");
            }
            catch (Exception ex)
            {
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View,
                    requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetAllMonthlyExpenseByMonth");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

        /// <summary>
        /// Get Monthly Summary Report
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>

        //public async Task<ResponseMessage> GetDailySummeryReport(RequestMessage requestMessage)
        //{
        //    ResponseMessage responseMessage = new ResponseMessage();
        //    string yearMonthNumber = requestMessage.RequestObj.ToString();

        //    string sql = @"SELECT * from Expense where ExpenseCategoryValue = "


        //}
        public async Task<ResponseMessage> GetMonthlySummaryReport(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                List<VMExpenseMonthlySummary> lstExpenseMonthlyReport = new List<VMExpenseMonthlySummary>();
                string year = requestMessage.RequestObj.ToString();

                //String.Format("Today's date is {0}", DateTime.Now);

                string sqlTemp = @"SELECT ExpenseType, [{0}01] as January, [{0}02] as February, [{0}03] as March, [{0}04] as April, [{0}05] as May , [{0}06] as June "
                                + @", [{0}07] as July, [{0}08] as August, [{0}09] as September, [{0}10] as October, [{0}11] as November , [{0}12] as December "
                                + @" FROM (select  e.ExpenseCategoryValue as month, et.Name as ExpenseType, SUM(e.Amount) as SumAmount from Expense as e  --SUM(e.Amount) as SumAmount,
                                left join ExpenseType as et on et.ID = e.ExpenseType
                                where e.ExpenseCategoryValue between '{0}01' and '{0}12' and e.Status = 1 and et.Status = 1
                                group by  e.ExpenseCategoryValue, et.Name) AS SourceTable
                                PIVOT
                                (
                                SUM(SumAmount)
                                FOR month IN([{0}01], [{0}02], [{0}03] , [{0}04] , [{0}05] , [{0}06] , [{0}07] , [{0}08] , [{0}09] , [{0}10] , [{0}11] , [{0}12] )
                                ) AS PivotTable;";

                //string sqlTemp = @"SELECT ExpenseType, CAST([{0}01] AS varchar)  as January, CAST([{0}02] AS varchar)  as February, CAST([{0}03] AS varchar)  as March, CAST([{0}04] AS varchar)  as April, CAST([{0}05] AS varchar)  as May , CAST([{0}06] AS varchar)  as June "
                //                + @", CAST([{0}07] AS varchar)  as July, CAST([{0}08] AS varchar)  as August, CAST([{0}09] AS varchar)  as September, CAST([{0}10] AS varchar)  as October, CAST([{0}11] AS varchar)  as November , CAST([{0}12] AS varchar)  as December "
                //                + @" FROM (select  e.ExpenseCategoryValue as month, et.Name as ExpenseType, SUM(e.Amount) as SumAmount from Expense as e  --SUM(e.Amount) as SumAmount,
                //                left join ExpenseType as et on et.ID = e.ExpenseType
                //                where e.ExpenseCategoryValue between '{0}01' and '{0}12' and e.Status = 1 and et.Status = 1
                //                group by  e.ExpenseCategoryValue, et.Name) AS SourceTable
                //                PIVOT
                //                (
                //                SUM(SumAmount)
                //                FOR month IN([{0}01], [{0}02], [{0}03] , [{0}04] , [{0}05] , [{0}06] , [{0}07] , [{0}08] , [{0}09] , [{0}10] , [{0}11] , [{0}12] )
                //                ) AS PivotTable;";


                string sql = String.Format(sqlTemp, year);

                //string sql = @" SELECT ExpenseType, [" + year + "01] as January, [" + year + "02] as February, [" + year + "03] as March, [" + year + "04] as April, [" + year + "05] as May , [" + year + "06] as June "
                //               + @" , [" + year + "07] as July, [" + year + "08] as August, [" + year + "09] as September, [" + year + "10] as October, [" + year + "11] as November , [" + year + "12] as December "
                //               + @" FROM
                //                (select  e.ExpenseCategoryValue as month, et.Name as ExpenseType, SUM(e.Amount) as SumAmount from Expense as e  --SUM(e.Amount) as SumAmount,
                //                left join ExpenseType as et on et.ID = e.ExpenseType
                //                where e.ExpenseCategoryValue between '" + year + @"01' and '" + year + @"12' and e.Status = 1 and et.Status = 1
                //                group by  e.ExpenseCategoryValue, et.Name) AS SourceTable
                //                PIVOT
                //                (
                //                SUM(SumAmount)
                //                FOR month IN([" + year + @"01], [" + year + @"02], [" + year + @"03] , [" + year + @"04] , [" + year + @"05] , [" + year + @"06] , [" + year + @"07] , [" + year + @"08] , [" + year + @"09] , [" + year + @"10] , [" + year + @"11] , [" + year + @"12] )
                //                ) AS PivotTable;";

                lstExpenseMonthlyReport = await _crmDbContext.VMExpenseMonthlySummary.FromSqlRaw(sql).ToListAsync();
                //totalCount = await _crmDbContext.VMPatient.FromSqlRaw("SELECT [PatientID] FROM [dbo].[GetAllPatient] where" + whereQuery).CountAsync();

                responseMessage.ResponseObj = lstExpenseMonthlyReport;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GetMonthlySummaryReport");
            }
            catch (Exception ex)
            {
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View,
                    requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetMonthlySummaryReport");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

        /// <summary>
        /// Get all System User
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public async Task<ResponseMessage> GetAllExpense(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                List<Expense> lstExpense = new List<Expense>();
                int totalSkip = 0;
                totalSkip = (requestMessage.PageNumber > 0) ? requestMessage.PageNumber * requestMessage.PageRecordSize : 0;

                lstExpense = await _crmDbContext.Expense.OrderBy(x => x.ID).Skip(totalSkip).Take(requestMessage.PageRecordSize).ToListAsync();
                responseMessage.ResponseObj = lstExpense;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GetAllExpense");
            }
            catch (Exception ex)
            {
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View,
                    requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetAllExpense");
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
        public async Task<ResponseMessage> GetExpenseById(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                Expense objExpense = new Expense();
                int expenseID = JsonConvert.DeserializeObject<int>(requestMessage?.RequestObj.ToString());

                objExpense = await _crmDbContext.Expense.AsNoTracking().FirstOrDefaultAsync(x => x.ID == expenseID && x.Status == (int)Enums.Status.Active);
                responseMessage.ResponseObj = objExpense;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GetExpenseById");
            }
            catch (Exception ex)
            {
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View,
                    requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetExpenseById");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

        public async Task<ResponseMessage> GetAttachment(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                int expenseID = JsonConvert.DeserializeObject<int>(requestMessage?.RequestObj.ToString());
                var objExpense = await _crmDbContext.ExpenseAttachment.AsNoTracking().FirstOrDefaultAsync(x => x.ExpenseID == expenseID);


                if (objExpense != null)
                {
                    responseMessage.ResponseObj = objExpense;
                    responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
                }
                else
                {
                    responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
                }
            }
            catch (Exception ex)
            {
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }
            return responseMessage;
        }

        public async Task<ResponseMessage> DeleteExpense(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                int expenseID = JsonConvert.DeserializeObject<int>(requestMessage?.RequestObj.ToString());

                string sqlUpdateQuery = $"UPDATE Expense SET Status = {(int)Enums.Status.Delete} WHERE ID = {expenseID} AND Status = {(int)Enums.Status.Active};";
                await _crmDbContext.Database.ExecuteSqlRawAsync(sqlUpdateQuery);

                var objExpense = await _crmDbContext.Expense.AsNoTracking().FirstOrDefaultAsync(x => x.ID == expenseID);

                if (objExpense != null)
                {
                    responseMessage.ResponseObj = objExpense;
                    responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
                }
                else
                {
                    responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
                }
            }
            catch (Exception ex)
            {
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }
            return responseMessage;
        }

        public async Task<ResponseMessage> GetAllDailyExpense(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                List<VMExpenseMonthly> lstExpense = new List<VMExpenseMonthly>();

                string sql = @"SELECT e.ID as ExpenseID, e.ExpenseCategoryValue as month, e.DateTime as Date, et.Name as ExpenseType, e.Description, e.Amount, e.ExpensedBy 
                       FROM Expense as e
                       LEFT JOIN ExpenseType as et ON et.ID = e.ExpenseType
                       WHERE e.Status = 1 AND et.Status = 1 order by e.ID desc";

                lstExpense = await _crmDbContext.VMExpenseMonthly.FromSqlRaw(sql).ToListAsync();

                responseMessage.ResponseObj = lstExpense;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
            }
            catch(Exception ex)
            {
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }
            return responseMessage;
        }

        public async Task<ResponseMessage> GetExpenseByDate(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                string date = requestMessage.RequestObj.ToString();
                DateTime requestDateTime = DateTime.Parse(date);

                DateTime utcDateTime = requestDateTime.ToUniversalTime();
                DateTime dateOnly = utcDateTime.Date;

                List<VMExpenseMonthly> lstExpense = new List<VMExpenseMonthly>();

                string sql = @"SELECT e.ID as ExpenseID, e.ExpenseCategoryValue as month, e.DateTime as Date, et.Name as ExpenseType, e.Description, e.Amount, e.ExpensedBy 
                       FROM Expense as e
                       LEFT JOIN ExpenseType as et ON et.ID = e.ExpenseType
                       WHERE CONVERT(DATE, e.DateTime) = @dateOnly AND e.Status = 1 AND et.Status = 1 order by e.ID desc";

                lstExpense = await _crmDbContext.VMExpenseMonthly.FromSqlRaw(sql, new SqlParameter("@dateOnly", dateOnly)).ToListAsync();
                foreach(VMExpenseMonthly expense in lstExpense)
                {
                    expense.ExpenseAttachment = await _crmDbContext.ExpenseAttachment.Where(e => e.ExpenseID == expense.ExpenseID).FirstOrDefaultAsync();
                }

                responseMessage.ResponseObj = lstExpense;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
            }
            catch (Exception ex)
            {
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
        public async Task<ResponseMessage> SaveExpense(RequestMessage requestMessage, string rootURL)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                Expense objExpense = JsonConvert.DeserializeObject<Expense>(requestMessage?.RequestObj.ToString());

                int actionType = (int)Enums.ActionType.Insert;

                if (objExpense != null)
                {
                    if (CheckedValidation(objExpense, responseMessage))
                    {
                        if (objExpense.ID > 0)
                        {
                            //Update Mode
                            Expense existingExpense = await this._crmDbContext.Expense.AsNoTracking().FirstOrDefaultAsync(x => x.ID == objExpense.ID && x.Status == (int)Enums.Status.Active);
                            if (existingExpense != null)
                            {
                                actionType = (int)Enums.ActionType.Update;
                                objExpense.CreatedDate = existingExpense.CreatedDate;
                                objExpense.CreatedBy = existingExpense.CreatedBy;
                                objExpense.UpdatedDate = DateTime.Now;
                                objExpense.UpdatedBy = requestMessage.UserID;
                                _crmDbContext.Expense.Update(objExpense);

                                responseMessage.ResponseObj = objExpense;
                            }
                        }
                        else
                        {
                            //Insert Mode
                            FilePathRead objFilePath = _configuration.GetSection("Attachments").Get<FilePathRead>();
                            objExpense.DateTime = DateTime.ParseExact(objExpense.DateTimeStr, objFilePath.DateTimeFormat,
                                       System.Globalization.CultureInfo.InvariantCulture);

                            string month = string.Empty;
                            if (objExpense.DateTime.Month > 9)
                            {
                                month = objExpense.DateTime.Month.ToString();
                            }
                            else
                            {
                                month = "0" + objExpense.DateTime.Month.ToString();
                            }
                            objExpense.ExpenseCategoryValue = int.Parse(objExpense.DateTime.Year.ToString() + month);

                            objExpense.Status = (int)Enums.Status.Active;
                            objExpense.CreatedDate = DateTime.Now;
                            objExpense.CreatedBy = requestMessage.UserID;
                            var res = await _crmDbContext.Expense.AddAsync(objExpense);
                            responseMessage.ResponseObj = res.Entity;
                        }
                    }
                    await _crmDbContext.SaveChangesAsync();

                    //** save FIle in folder only at insert time
                    if (!string.IsNullOrEmpty(objExpense.FileName) && actionType == (int)Enums.ActionType.Insert)
                    {
                        //** (a) save at folder
                        string filePathsaved = string.Empty;
                        bool result = SaveFile(objExpense.FileName, rootURL, objExpense.Base64File, out filePathsaved);

                        //** (b) save at Expense Attachement Table
                        ExpenseAttachment expenseAttachmentObj = new ExpenseAttachment();
                        expenseAttachmentObj.ExpenseID = objExpense.ID;
                        expenseAttachmentObj.FileName = objExpense.FileName;
                        expenseAttachmentObj.FilePath = filePathsaved;

                        expenseAttachmentObj.Status = (int)Enums.Status.Active;
                        expenseAttachmentObj.CreatedDate = DateTime.Now;
                        expenseAttachmentObj.CreatedBy = requestMessage.UserID;
                        var res = await _crmDbContext.ExpenseAttachment.AddAsync(expenseAttachmentObj);
                        responseMessage.ResponseObj = res.Entity;

                        await _crmDbContext.SaveChangesAsync();
                    }

                    else if(!string.IsNullOrEmpty(objExpense.FileName) && actionType == (int)Enums.ActionType.Update)
                    {
                        //** (a) save at folder
                        string filePathsaved = string.Empty;
                        bool result = SaveFile(objExpense.FileName, rootURL, objExpense.Base64File, out filePathsaved);

                        ExpenseAttachment existExpenseAttachment = _crmDbContext.ExpenseAttachment.FirstOrDefault(e => e.ExpenseID == objExpense.ID);
                        if(existExpenseAttachment != null)
                        {
                            existExpenseAttachment.FileName = objExpense.FileName;
                            existExpenseAttachment.FilePath = filePathsaved;
                            existExpenseAttachment.UpdatedDate = DateTime.Now;
                            existExpenseAttachment.UpdatedBy = requestMessage.UserID;

                            _crmDbContext.Update(existExpenseAttachment);
                            await _crmDbContext.SaveChangesAsync();
                        }
                        else
                        {
                            ExpenseAttachment expenseAttachmentObj = new ExpenseAttachment();
                            expenseAttachmentObj.ExpenseID = objExpense.ID;
                            expenseAttachmentObj.FileName = objExpense.FileName;
                            expenseAttachmentObj.FilePath = filePathsaved;

                            expenseAttachmentObj.Status = (int)Enums.Status.Active;
                            expenseAttachmentObj.CreatedDate = DateTime.Now;
                            expenseAttachmentObj.CreatedBy = requestMessage.UserID;
                            var res = await _crmDbContext.ExpenseAttachment.AddAsync(expenseAttachmentObj);
                            responseMessage.ResponseObj = res.Entity;

                            await _crmDbContext.SaveChangesAsync();
                        }
                    }

                    responseMessage.Message = MessageConstant.SavedSuccessfully;
                    responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                    //Log write
                    LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "SaveExpense");
                }
                else
                {
                    responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
                    responseMessage.Message = MessageConstant.SaveFailed;
                }
            }
            catch (Exception ex)
            {
                //Exception write
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View,
                requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "SaveExpense");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }
        /// <summary>
        /// validation check
        /// </summary>
        /// <param name="objExpense"></param>
        /// <returns></returns>
        /// 

        public async Task<ResponseMessage> UpdateExpense(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();

            try
            {
                // Additional null checks
                if (requestMessage == null || requestMessage.RequestObj == null)
                {
                    responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
                    return responseMessage;
                }

                Expense objExpense = JsonConvert.DeserializeObject<Expense>(requestMessage.RequestObj.ToString());

                if (objExpense != null)
                {
                    Expense existingExpense = await this._crmDbContext.Expense.FirstOrDefaultAsync(x => x.ID == objExpense.ID && x.Status == 1);

                    if (existingExpense != null)
                    {
                        objExpense.CreatedDate = existingExpense.CreatedDate;
                        objExpense.CreatedBy = existingExpense.CreatedBy;
                        objExpense.UpdatedDate = DateTime.Now;
                        objExpense.UpdatedBy = requestMessage.UserID;

                        _crmDbContext.Entry(existingExpense).CurrentValues.SetValues(objExpense);
                        await _crmDbContext.SaveChangesAsync();

                        responseMessage.ResponseObj = objExpense;
                        responseMessage.Message = MessageConstant.SavedSuccessfully;
                        responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
                    }
                }
            }
            catch (Exception ex)
            {
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }


        #region "Private Methods"
        private bool CheckedValidation(Expense objExpense, ResponseMessage responseMessage)
        {
            //if (string.IsNullOrEmpty(objExpense.))
            //{
            //    responseMessage.Message = MessageConstant.Name_Is_Required;
            //    return false;
            //}

            //Expense existingExpense = new Expense();
            //existingExpense = _crmDbContext.Expense.Where(x => x.Name == objExpense.Name && x.ID != objExpense.ID && x.Status == (int)Enums.Status.Active).AsNoTracking().FirstOrDefault();
            //if (existingExpense != null)
            //{
            //    responseMessage.Message = MessageConstant.Duplicate_Name;
            //    return false;
            //}

            return true;
        }

        private bool SaveFile(string fileName, string rootUrl, string fileContent64, out string savedFilePath)
        {
            savedFilePath = string.Empty;
            try
            {
                FilePathRead objFilePath = _configuration.GetSection("Attachments").Get<FilePathRead>();
                string guidNo = Guid.NewGuid().ToString();

                string folderlocation = _configuration.GetSection("Attachments").GetSection("expenseImageSaveUrl").Value;

                string saveFilePath = Path.Combine(folderlocation, guidNo + "__" + fileName);
                string showFilePath = Path.Combine("", guidNo + "__" + fileName); // or you can set the desired URL here

                bool exists = System.IO.Directory.Exists(folderlocation);
                if (!exists)
                    System.IO.Directory.CreateDirectory(folderlocation);

                File.WriteAllBytesAsync(saveFilePath, Convert.FromBase64String(fileContent64.Split(",")[1]));

                string getshowUrl = _configuration.GetSection("Attachments").GetSection("caregiverImageShowUrl").Value;
                string showUrl = saveFilePath.Replace(_configuration.GetSection("Attachments").GetSection("expenseImageSaveUrl").Value, getshowUrl);

                savedFilePath = showUrl; // Set the correct file path to return

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        #endregion
    }
}

