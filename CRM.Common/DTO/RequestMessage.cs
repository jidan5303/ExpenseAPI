namespace CRM.Common.DTO
{
    public class RequestMessage
    {
        public object RequestObj { get; set; }
        public int PageRecordSize { get; set; } = 0;
        public int PageNumber { get; set; } = 0;
        public int UserID { get; set; }

    }
}
