namespace CRM.Common.DTO
{
    public class ResponseMessage
    {
        public object? ResponseObj { get; set; }
        public int ResponseCode { get; set; } = 0;
        public string? Message { get; set; }
        public int TotalCount { get; set; } = 0;
    }
}
