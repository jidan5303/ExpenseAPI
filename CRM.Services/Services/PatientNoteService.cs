using CRM.Common.Constants;
using CRM.Common.DTO;
using CRM.Common.Enums;
using CRM.Common.Helper;
using CRM.Common.Models;
using CRM.DataAccess;
using CRM.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Services.Services
{
	public class PatientNoteService : IPatientNotesService
	{
		private readonly CRMDbContext _crmDbContext;
		public PatientNoteService(CRMDbContext crmDbContext)
		{
			_crmDbContext = crmDbContext;
		}

		/// <summary>
		/// get all patient note.
		/// </summary>
		/// <param name="requestMessage"></param>
		/// <returns></returns>
		public async Task<ResponseMessage> GetAllPatientNote(RequestMessage requestMessage)
		{
			ResponseMessage responseMessage = new ResponseMessage();
			try
			{
				List<PatientNote> lstNote = new List<PatientNote>();
				int totalSkip = 0;
				totalSkip = (requestMessage.PageNumber > 0) ? requestMessage.PageNumber * requestMessage.PageRecordSize : 0;

				lstNote = await _crmDbContext.PatientNotes
					.OrderBy(x => x.PatientNoteID)
					.Skip(totalSkip)
					.Take(requestMessage.PageRecordSize)
					.Select(s=>new PatientNote()
					{
						PatientID=s.PatientID,
						PatientNoteID=s.PatientNoteID,
						Note=s.Note,
						CreatedBy=s.CreatedBy,
						CreatedDate=s.CreatedDate
					})
					.ToListAsync();
				responseMessage.ResponseObj = lstNote;
				responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

				//Log write
				LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GetAllPatientNote");
			}
			catch (Exception ex)
			{
				//Process excetion, Development mode show real exception and production mode will show custom exception.
				responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetAllTag");
				responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
			}

			return responseMessage;
		}

		public Task<ResponseMessage> GetPatientNoteById(RequestMessage requestMessage)
		{
			throw new NotImplementedException();
		}

		public async Task<ResponseMessage> SavePatientNote(RequestMessage requestMessage)
		{
			ResponseMessage responseMessage = new ResponseMessage();
			int actionType = (int)Enums.ActionType.Insert;
			try
			{
				PatientNote objPatientNote = JsonConvert.DeserializeObject<PatientNote>(requestMessage?.RequestObj.ToString());
				if (objPatientNote != null)
				{
					if (CheckedValidation(objPatientNote, responseMessage))
					{
						if (objPatientNote.PatientNoteID > 0)
						{
							PatientNote existingPatientNote = await _crmDbContext.PatientNotes
								.AsNoTracking()
								.FirstOrDefaultAsync(x => x.PatientNoteID == objPatientNote.PatientNoteID);

							if (existingPatientNote != null)
							{
								objPatientNote.CreatedBy = existingPatientNote.CreatedBy;
								objPatientNote.CreatedDate = existingPatientNote.CreatedDate;
								objPatientNote.UpdatedDate = DateTime.Now;
								objPatientNote.UpdatedBy = requestMessage.UserID;
								actionType = (int)Enums.ActionType.Update;
								_crmDbContext.PatientNotes.Update(objPatientNote);
							}
						}
						else
						{
							objPatientNote.CreatedBy=requestMessage.UserID;
							objPatientNote.CreatedDate=DateTime.Now;
							await _crmDbContext.PatientNotes.AddAsync(objPatientNote);
						
						}

						await _crmDbContext.SaveChangesAsync();

						responseMessage.ResponseObj=objPatientNote;
						responseMessage.Message = MessageConstant.SavedSuccessfully;
						responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

						//Log write
						LogHelper.WriteLog(requestMessage.RequestObj, actionType, requestMessage.UserID, "SavePatientNote");
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
		private bool CheckedValidation(PatientNote objPatientNote, ResponseMessage responseMessage)
		{
			if (string.IsNullOrEmpty(objPatientNote.Note))
			{
				responseMessage.Message = MessageConstant.PatientNote;
				return false;
			}
			return true;
		}
	}
}
