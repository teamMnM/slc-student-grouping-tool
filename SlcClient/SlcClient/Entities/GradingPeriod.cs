using SlcClient.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SlcClient.Entities
{
    public class GradingPeriod
    {
        /// <summary>
        /// Month, day, and year of the first day of the grading period.
        /// </summary>
        public DateTime beginDate { get; set; }

        /// <summary>
        /// Month, day, and year of the last day of the grading period.
        /// </summary>
        public DateTime endDate { get; set; }

        /// <summary>
        /// The name of the period for which grades are reported
        /// values:First Six Weeks, Second Six Weeks, Third Six Weeks, Fourth Six Weeks, Fifth Six Weeks, Sixth Six Weeks, First Semester,
        /// Second Semester, First Summer Session, Second Summer Session, Third Summer Session, Summer Semester, First Nine Weeks, 
        /// Second Nine Weeks, Third Nine Weeks, Fourth Nine Weeks, First Trimester, Second Trimester, Third Trimester, End of Year
        /// </summary>
        public GradingPeriodType gradingPeriod { get; set; }

        /// <summary>
        /// Total days available for educational instruction during the grading period.
        /// </summary>
        public int totalInstructionalDays { get; set; }

        /// <summary>
        /// Days associated with the grading period.
        /// </summary>
        public List<CalendarDate> calendarDates { get; set; }
    }
}