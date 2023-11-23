using CRM.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Common.QueryHelper
{
    public class BuildDynamicFilter
    {
        public string GetFilterCluse(List<FilterModel> lstfilter)
        {
            string whereQuery = string.Empty;

            string convalue = string.Empty;

            lstfilter.ForEach(filterModel =>
            {

                if (!string.IsNullOrEmpty(filterModel?.ColId) && filterModel?.ColId == "FullName")
                {
                    convalue = filterModel?.ColId;
                    filterModel.ColId = $" (LTRIM(RTRIM([FirstName])) + LTRIM(RTRIM([LastName])))";
                }

                filterModel?.Filters?.ForEach(filter =>
                {
                    int? indexValue = filterModel?.Filters.IndexOf(filter);                   
                    if (!string.IsNullOrEmpty(filterModel?.Operator) && indexValue > 0)
                    {
                        whereQuery += " " + filterModel.Operator;
                    }

                    string oparetors = (!string.IsNullOrEmpty(filterModel?.Operator) && indexValue != 0) ? "" : "and";

                    switch (filter?.FilterType)
                    {
                        case "text":
                            {
                                switch (filter?.Type)
                                {
                                    case "equals":
                                        whereQuery += $" {oparetors} {filterModel?.ColId} = '{((string.IsNullOrEmpty(convalue) && convalue == "FullName") ? filter?.Filter?.Replace(" ", "") : filter?.Filter)}'";
                                        break;
                                    case "notEqual":
                                        whereQuery += $" {oparetors} {filterModel?.ColId} != '{((string.IsNullOrEmpty(convalue) && convalue == "FullName") ? filter?.Filter?.Replace(" ", "") : filter?.Filter)}'";
                                        break;
                                    case "contains":
                                        whereQuery += $" {oparetors} {filterModel?.ColId} like '%{((string.IsNullOrEmpty(convalue) && convalue == "FullName") ? filter?.Filter?.Replace(" ", "") : filter?.Filter)}%'";
                                        break;
                                    case "notContains":
                                        whereQuery += $" {oparetors} {filterModel?.ColId} not like '%{((string.IsNullOrEmpty(convalue) && convalue == "FullName") ? filter?.Filter?.Replace(" ", "") : filter?.Filter)}%'";
                                        break;
                                    case "startsWith":
                                        whereQuery += $" {oparetors}  {filterModel?.ColId}  like '{((string.IsNullOrEmpty(convalue) && convalue == "FullName") ? filter?.Filter?.Replace(" ", "") : filter?.Filter)}%'";
                                        break;
                                    case "endsWith":
                                        whereQuery += $" {oparetors} {filterModel?.ColId}  like '%{((string.IsNullOrEmpty(convalue) && convalue == "FullName") ? filter?.Filter?.Replace(" ", "") : filter?.Filter)}'";
                                        break;
                                }

                                break;
                            }
                        case "number":
                            {
                                switch (filter?.Type)
                                {
                                    case "equals":
                                        whereQuery += $" {oparetors} {filterModel?.ColId} = {filter?.Filter}";
                                        break;           
                                    case "notEqual":     
                                        whereQuery += $" {oparetors} {filterModel?.ColId} <> {filter?.Filter}";
                                        break;           
                                    case "lessThan":     
                                        whereQuery += $" {oparetors} {filterModel?.ColId} <  {filter?.Filter}";
                                        break;
                                    case "lessThanOrEqual":
                                        whereQuery += $" {oparetors} {filterModel?.ColId} <= {filter?.Filter} ";
                                        break;
                                    case "greaterThan":
                                        whereQuery += $" {oparetors} {filterModel?.ColId} > {filter?.Filter} ";
                                        break;
                                    case "greaterThanOrEqual":
                                        whereQuery += $" {oparetors} {filterModel?.ColId} >= {filter?.Filter} ";
                                        break;
                                    case "inRange":
                                        whereQuery += $" {oparetors} {filterModel?.ColId} >= {filter?.Filter} AND {filterModel?.ColId} <= {filter?.Filter}";
                                        break;
                                    case "in":
                                        whereQuery += $" {oparetors}  {filterModel?.ColId} in ({filter?.Filter})";
                                        break;
                                    case "notIn":
                                        whereQuery += $" {oparetors} {filterModel?.ColId} not in ({filter?.Filter})";
                                        break;
                                }
                                break;

                            }
                        case "date":

                            switch (filter.Type)
                            {
                                case "equals":
                                    whereQuery += $" {oparetors} datediff(day, {filterModel?.ColId}, '{filter?.DateFrom}') = 0";
                                    break;
                                case "notEqual":
                                    whereQuery += $" {oparetors} datediff(day, {filterModel?.ColId}, '{filter?.DateFrom}') <> 0";
                                    break;
                                case "lessThan":
                                    whereQuery += $" {oparetors} datediff(day, {filterModel?.ColId}, '{filter?.DateFrom}') > 0";                                    
                                    break;
                                case "lessThanOrEqual":
                                    whereQuery += $" {oparetors} datediff(day, {filterModel?.ColId}, '{filter?.DateFrom}') >= 0";
                                    break;
                                case "greaterThan":
                                    whereQuery += $" {oparetors} datediff(day, {filterModel?.ColId}, '{filter?.DateFrom}') < 0";
                                    break;
                                case "greaterThanOrEqual":
                                    whereQuery += $" {oparetors} datediff(day, {filterModel?.ColId}, '{filter?.DateFrom}') <= 0";
                                    break;
                                case "inRange":
                                    whereQuery += $" {oparetors} {filterModel?.ColId} between '{(filter?.DateFrom?.ToString("yyyy-MM-dd"))} 00:00:00' AND  '{filter?.DateTo?.ToString("yyyy-MM-dd")} 23:59:59'";
                                    break;
                            }
                            break;
                        case "boolean":
                            whereQuery += $" {oparetors} {filterModel?.ColId} = {filter?.Filter}";
                            break;
                    }

                });
            });
            convalue = string.Empty;
            return whereQuery;
        }

        public string GenerateSortQuery(List<SortModel> lstSortModel)
        {           
            string shortQuery = " Order by ";
            lstSortModel.ForEach(sort =>
            {
                if (!string.IsNullOrEmpty(sort?.ColId) && sort?.ColId == "FullName")
                {
                   
                    sort.ColId = $" (LTRIM(RTRIM([FirstName])) + LTRIM(RTRIM([LastName])))";
                }
                int intdex = lstSortModel.IndexOf(sort);
                if (intdex == 0)
                {
                    shortQuery += $" {sort.ColId} {sort.Sort} ";
                }
                else
                {
                    shortQuery += $" , {sort.ColId} {sort.Sort} ";
                }

            });
            return shortQuery;
        }
    }
}