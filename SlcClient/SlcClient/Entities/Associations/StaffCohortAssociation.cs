using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SlcClient.Entities
{
    /// <summary>
    /// This association indicates the staff associated with a cohort of students.
    /// </summary>
    public class StaffCohortAssociation
    {
        public string id { get; set; }
        public string staffId { get; set; }
        public string cohortId { get; set; }

        /// <summary>
        /// Start date for the association of staff to this cohort.
        /// </summary>
        public DateTime beginDate { get; set; }

        /// <summary>
        /// End date for the association of staff to this cohort.
        /// </summary>
        public DateTime endDate { get; set; }

        /// <summary>
        /// Indicator of whether the staff has access to the student records of the cohort per district interpretation of FERPA and other privacy laws, regulations and policies.
        /// </summary>
        public bool studentRecordAccess { get; set; }

        /// <summary>
        /// Reference to the cohort associated with the staff.
        /// </summary>
        public List<Cohort> cohorts { get; set; }

        /// <summary>
        /// The staff associated with the cohort of students.
        /// </summary>
        public List<Staff> staffs { get; set; }
    }
}