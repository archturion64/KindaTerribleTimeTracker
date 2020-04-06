using System.ComponentModel.DataAnnotations;


namespace KTTTDataInterface
{
    /// <summary>
    /// Object that is describing a database table.
    /// </summary>
    public class WorkDayModel
    {
        /// <summary>
        /// The number of a week in the callender.
        /// </summary>
        /// <value>int between 1-53 or -1 on error</value>
        public int calWeek { get; set; }

        /// <summary>
        /// A unique entry, since one employee cannot have 2 work days with the same date.
        /// </summary>
        /// <value>unique string</value>
        [Key]
        public string date { get; set; }

        /// <summary>
        /// Starting time, set at application start and can't change for a later time.
        /// </summary>
        /// <value>string that is region/language specific</value>
        public string startTime { get; set; }

        /// <summary>
        /// Stopping work time, updated throughout the application runtime.
        /// </summary>
        /// <value>string that is region/language specific</value>
        public string endTime { get; set; }

        /// <summary>
        /// Active time of work.
        /// </summary>
        /// <value>number of hours of active work</value>
        public float hoursActive { get; set; }
    }
}
