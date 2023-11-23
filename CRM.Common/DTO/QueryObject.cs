using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Common.DTO
{
	public class QueryObject
	{
		public object? BasicSearch { get; set; }
		public DateTime? FromDate { get; set; }
		public DateTime? ToDate { get; set; }
		public string? SearchText { get; set; }
		public List<string>? LstColumnName { get; set; }
		public List<FilterModel>? FilterModel { get; set; }
		public List<SortModel>? SortModel { get; set; }
	}

	public class FilterModel
	{
		public string? ColId { get; set; }
        public string? Operator { get; set; }
		public string? FilterType { get; set; }
		public List<Condition>? Filters { get; set; }
	}

	public class Condition
	{
		public DateTime? DateFrom { get; set; }
		public DateTime? DateTo { get; set; }
		public string? Filter { get; set; }
		public string? FilterType { get; set; }
		public string? Type { get; set; }
	}


	public class SortModel
	{
		public string? ColId { get; set; }
		public string? Sort { get; set; }
	}
}
