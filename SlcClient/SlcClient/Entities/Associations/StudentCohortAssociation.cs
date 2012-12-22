using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SlcClient.Entities
{
    public class StudentCohortAssociation
    {
        public string id { get; set; }
        [Required]
        public string studentId { get; set; }
        [Required]
        public string cohortId { get; set; }

        /// <summary>
        /// The month, day, and year on which the student was first identified as part of the cohort.
        /// </summary>
        [Required]
        public DateTime beginDate { get; set; }

        /// <summary>
        /// The month, day, and year on which the student was removed as part of the cohort.
        /// </summary>
        public DateTime? endDate { get; set; }

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
    }
}