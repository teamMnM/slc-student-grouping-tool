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

        /// <summary>
        /// The staff associated with the cohort of students.
        /// </summary>
        public string staffId { get; set; }

        /// <summary>
        /// Reference to the cohort associated with the staff.
        /// </summary>
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
        /// Only serialize id if it's not null
        /// </summary>
        /// <returns>Whether id should be serialized</returns>
        public bool ShouldSerializeid()
        {
            return (id != null);
        }

        /// <summary>
        /// Only serialize endDate if it's not null
        /// </summary>
        /// <returns>Whether endDate should be serialized</returns>
        public bool ShouldSerializeendDate()
        {
            return (endDate != null);
        }
        ///// <summary>
        ///// Reference to the cohort associated with the staff.
        ///// </summary>
        //public List<Cohort> cohorts { get; set; }

        ///// <summary>
        ///// The staff associated with the cohort of students.
        ///// </summary>
        //public List<Staff> staffs { get; set; }
    }
}