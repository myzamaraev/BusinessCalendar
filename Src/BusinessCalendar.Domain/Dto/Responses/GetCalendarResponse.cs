namespace BusinessCalendar.Domain.Dto.Responses
{
    public class GetCalendarResponse
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
        /// Calendar year
        /// </summary>
        public int Year { get; set; }
        
        /// <summary>
        /// The list of holidays
        /// </summary>
        public List<DateTime> Holidays { get; set; } = new ();
        
        /// <summary>
        /// The list of extra work days
        /// </summary>
        public List<DateTime> ExtraWorkDays { get; set; } = new ();
        
    }
}