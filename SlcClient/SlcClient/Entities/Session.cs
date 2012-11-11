using SlcClient.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SlcClient.Entities
{
    public class Session
    {
        /// <summary>
        /// Month, day and year of the first day of the session.
        /// </summary>
        public DateTime beginDate { get; set; }

        /// <summary>
        /// Month, day and year of the last day of the session.
        /// </summary>
        public DateTime endDate { get; set; }

        /// <summary>
        /// The identifier for the school year.
        /// </summary>
        public SchoolYearType schoolYear { get; set; }

        /// <summary>
        /// The identifier for the calendar for the academic session (e.g., 2010/11, 2011 Summer).
        /// </summary>
        [StringLength(60)]
        public string sessionName { get; set; }

        /// <summary>
        /// The term for the session during the school year.
        /// </summary>
        public TermType term { get; set; }

        /// <summary>
        /// The total number of instructional days in the school calendar.
        /// </summary>
        public int totalInstructionalDays { get; set; }

        /// <summary>
        /// The grading periods that take place during the session.
        /// </summary>
        public List<GradingPeriod> gradingPeriods { get; set; }
    }
}