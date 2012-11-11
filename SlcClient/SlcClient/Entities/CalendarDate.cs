using SlcClient.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SlcClient.Entities
{
    public class CalendarDate
    {
        /// <summary>
        /// The type of scheduled or unscheduled event for the day. 
        /// For example: Instructional day Teacher only day Holiday Make-up day Weather day Student late arrival/early dismissal ...
        /// </summary>
        public CalendarEventType calendarEvent { get; set; }

        /// <summary>
        /// The month, day and year of the calendar date.
        /// </summary>
        public DateTime date { get; set; }
    }
}