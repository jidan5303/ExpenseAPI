using CRM.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Services.Interface
{
	public interface IPatientNotesService
	{
		Task<ResponseMessage> GetAllPatientNote(RequestMessage requestMessage);
		Task<ResponseMessage> SavePatientNote(RequestMessage requestMessage);
		Task<ResponseMessage> GetPatientNoteById(RequestMessage requestMessage);
	}
}
