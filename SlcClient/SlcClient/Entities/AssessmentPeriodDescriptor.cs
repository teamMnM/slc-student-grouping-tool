using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SlcClient.Entities
{
    /// <summary>
    /// This descriptor holds the period or window in which an assessment is supposed to be administered (e.g., Beginning of Year, Middle of Year, End of Year).
    /// </summary>
    public class AssessmentPeriodDescriptor
    {
        /// <summary>
        /// The first date the assessment is to be administered.
        /// </summary>
        public DateTime beginDate { get; set; }

        /// <summary>
        /// A code or abbreviation that is used to refer to an Assessment Period.
        /// </summary>
        [StringLength(20)]
        public string codeValue { get; set; }

        /// <summary>
        /// The description of the Assessment Period.
        /// </summary>
        [StringLength(1024)]
        public string description { get; set; }

        /// <summary>
        /// The last date the assessment is to be administered.
        /// </summary>
        public DateTime endDate { get; set; }

        /// <summary>
        /// A shortened description for reference.
        /// </summary>
        [StringLength(50)]
        public string shortDescription { get; set; }

    }
}