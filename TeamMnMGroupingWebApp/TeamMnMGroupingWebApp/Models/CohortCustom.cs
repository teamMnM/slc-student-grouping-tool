using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeamMnMGroupingWebApp.Models
{
    /// <summary>
    /// custom data to store in the SLC custom entity
    /// </summary>
    public class CohortCustom
    {
        /// <summary>
        /// the list of data elements to display for students
        /// </summary>
        public IEnumerable<DataElement> dataElements { get; set; }

        /// <summary>
        /// lesson plan object
        /// </summary>
        public LessonPlan lessonPlan { get; set; }

        /// <summary>
        /// The last modified date time of the cohort in context
        /// </summary>
        public DateTime lastModifiedDate { get; set; }
    }
}