namespace BusinessCalendar.Domain.Dto.Responses
{
    public class GetCalendarDateResponse
    {
        /// <summary>
        /// Calendar Type 
        /// </summary>
        public string Type { get; set; } = string.Empty;
        
        /// <summary>
        /// Calendar Key
        /// </summary>
        public string Key { get; set; } = string.Empty;
        
        /// <summary>
        /// Requested date
        /// </summary>
        public DateTime Date { get; set; }
        
        /// <summary>
        /// Workday or not according to requested calendar
        /// </summary>
        public bool IsWorkday { get; set; }
    }
}